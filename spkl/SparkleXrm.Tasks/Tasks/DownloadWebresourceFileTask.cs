using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using SparkleXrm.Tasks.Config;
using Microsoft.Xrm.Sdk.Query;
using System.IO;

namespace SparkleXrm.Tasks.Tasks
{
    public class DownloadWebresourceFileTask : BaseTask
    {

        public bool Overwrite { get; set; }


        public DownloadWebresourceFileTask(IOrganizationService service, ITrace trace) : base(service, trace)
        {
        }

        public DownloadWebresourceFileTask(OrganizationServiceContext context, ITrace trace) : base(context, trace)
        {
        }

        protected override void ExecuteInternal(string filePath, OrganizationServiceContext ctx)
        {

            _trace.WriteLine("Searching for webresources in '{0}'", filePath);

            ConfigFile config = null;
            try
            {
                var configs = ConfigFile.FindConfig(filePath);
                config = configs[0];
            }
            catch
            {
                config = new ConfigFile()
                {
                    filePath = filePath,
                };
            }

            if (config.webresources == null || config.webresources.Count == 0)
            {
                // Add a webresource section
                config.webresources = new List<WebresourceDeployConfig> {new WebresourceDeployConfig
                    {
                        files = new List<WebResourceFile>()
                    }
                };
            }

            var newWebResources = new List<WebResourceFile>();

            var files = config.webresources.Where(a => a.files != null).SelectMany(a => a.files);
            Dictionary<string, WebResourceFile> existingWebResources = new Dictionary<string, WebResourceFile>();
            foreach (var file in files)
            {
                string key = file.uniquename.ToLower();
                if (!existingWebResources.ContainsKey(key))
                {
                    existingWebResources[key] = file;
                }
            }

            var webresourceConfig = config.GetWebresourceConfig(null).FirstOrDefault();
            if (webresourceConfig == null)
                throw new Exception("Cannot find webresource section in spkl.json");

            string rootPath = Path.Combine(config.filePath, webresourceConfig.root != null ? webresourceConfig.root : "");

            //Console.WriteLine($"rootPath: {rootPath}");

            foreach (var solution in config.solutions)
            {
                var maps = solution.map.Where(e => e.from.StartsWith("WebResources"));
                foreach (var resource in GetResourcesFromSolution(solution.solution_uniquename))
                {
                    string shortName = resource.GetAttributeValue<string>("name"),
                        name = $"WebResources\\{shortName}".Replace("/", "\\"),
                        path = name,
                        namefolder,
                        namefile;

                    SplitFileAndFolder(name, out namefolder, out namefile);

                    foreach (var map in maps)
                    {
                        string from = RemoveTrailingFolderSeperator(map.from),
                            fromfolder, 
                            fromfile;

                        SplitFileAndFolder(from, out fromfolder, out fromfile);

                        var to = Path.GetFullPath(Path.Combine(filePath, RemoveTrailingFolderSeperator(map.to)));

                        if (map.map == MapTypes.file && Wildcard(fromfile,namefile))
                        {
                            path = Path.GetFullPath(Path.Combine(filePath, to));
                            break;
                        }
                        else if (map.map==MapTypes.path && name.StartsWith(fromfolder, StringComparison.InvariantCultureIgnoreCase)
                            && (fromfile=="*.*" || Wildcard(fromfile,namefile)))
                        { 
                            path = Path.GetFullPath(Path.Combine(filePath, name.Replace(fromfolder, to)));
                            break;
                        }
                        //fail through
                        path = Path.GetFullPath(Path.Combine(filePath, name));
                    }

                    //Console.WriteLine($"FileName: {filename}");

                    var content = Convert.FromBase64String(resource.GetAttributeValue<string>("content"));

                    if (-1 != path.IndexOfAny(Path.GetInvalidPathChars()))
                        continue;

                    SaveFile(path, content, Overwrite);
                        
                    if (!existingWebResources.ContainsKey(resource.GetAttributeValue<string>("name")))
                    {
                        newWebResources.Add(new WebResourceFile()
                        {
                            uniquename = shortName,
                            displayname = resource.GetAttributeValue<string>("displayname"),
                            description = resource.GetAttributeValue<string>("description"),
                            file = path.Replace(rootPath, "").TrimStart('\\').TrimStart('/')
                        });
                    }
                    

                }
            }

            var webresources = DirectoryEx.Search(filePath, "*.js|*.htm|*.css|*.xap|*.png|*.jpeg|*.jpg|*.gif|*.ico|*.xml|*.svg", null);
            
            if (webresourceConfig.files == null)
                webresourceConfig.files = new List<WebResourceFile>();
            webresourceConfig.files.AddRange(newWebResources);
            config.Save();

        }

        private static void SplitFileAndFolder(string filepath, out string folder, out string file)
        {
            file = filepath;
            folder = string.Empty;
            var slash = filepath.LastIndexOf('\\');
            if (slash < 0) return;
            if (filepath.Last()=='\\')
            {
                folder = filepath;
                file = string.Empty;
                return;
            }

            file = filepath.Substring(slash + 1);
            folder = filepath.Substring(0, slash);
            return;
        }

        public Boolean Wildcard(string pattern, string input)
        {
            if (String.Compare(pattern, input) == 0)
            {
                return true;
            }
            else if (pattern.Length == 0)
            {
                return false;
            }
            else if (String.IsNullOrEmpty(input))
            {
                return String.IsNullOrEmpty(pattern.Trim('*'));
            }
            else if (pattern[0] == '*')
            {
                if (Wildcard(pattern.Substring(1), input))
                {
                    return true;
                }
                else
                {
                    return Wildcard(pattern, input.Substring(1));
                }
            }
            else if (pattern[0] == '?' || pattern[0] == input[0])
            {
                return Wildcard(pattern.Substring(1), input.Substring(1));
            }
            return false;
        }

        private static void SaveFile(string filename, byte[] content, bool overwrite)
        {
            var fileInfo = new FileInfo(filename);
            if (!fileInfo.Directory.Exists) fileInfo.Directory.Create();

                

            if (File.Exists(filename) && !overwrite)
            {
                Console.WriteLine($"File already exists: {filename}");
                return;
            }

            using (var writer = new BinaryWriter((Stream)new FileStream(filename, FileMode.Create)))
            {
                writer.Write(content, 0, content.Length);
                writer.Close();
            }

            Console.WriteLine($"Downloaded: {filename}");
        }

        private string RemoveTrailingFolderSeperator(string folder)
        {
            //Todo... this should be handled better with better comparison, mapping, etc.
            folder = folder.Replace("\\**", "");//.Replace("*.*","");
            while ((int)folder[folder.Length - 1] == (int)Path.DirectorySeparatorChar || (int)folder[folder.Length - 1] == (int)Path.AltDirectorySeparatorChar)
                folder = folder.Substring(0, folder.Length - 1);
            return folder;
        }

        private List<Entity> GetResourcesFromSolution(string name)
        {
            var fetch = $@"<fetch>
              <entity name='webresource'>
                <attribute name='name' />
                <attribute name='displayname' />
                <attribute name='description' />
                <attribute name='content' />
                <attribute name='webresourcetype' />
                <filter>
                    <condition attribute='ishidden' operator='eq' value='false' />
                    <condition attribute='iscustomizable' operator='eq' value ='true' />
                </filter>
                <link-entity name='solutioncomponent' from='objectid' to='webresourceid' link-type='inner'>
                  <filter>
                    <condition attribute='componenttype' operator='eq' value='61' />
                  </filter>
                  <link-entity name='solution' from='solutionid' to='solutionid' link-type='inner'>
                    <filter>
                      <condition attribute='uniquename' operator='eq' value='{name}' />
                    </filter>
                  </link-entity>
                </link-entity>
              </entity>
            </fetch>";

            //Console.WriteLine($"Query:\r\n\r\n{fetch}\r\n\r\n");

            var resources = _service.RetrieveMultiple(new FetchExpression(fetch));

            if (resources == null || resources.Entities.Count == 0) return new List<Entity>();

            return resources.Entities.ToList();
        }
    }
}
