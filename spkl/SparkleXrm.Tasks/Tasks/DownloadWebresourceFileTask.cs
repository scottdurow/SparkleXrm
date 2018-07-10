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

namespace SparkleXrm.Tasks
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
                var configs = ServiceLocator.ConfigFileFactory.FindConfig(filePath);
                config = configs[0];
            }
            catch (Exception ex)
            {
                throw new SparkleTaskException(SparkleTaskException.ExceptionTypes.CONFIG_NOTFOUND, $"spkl.json not found at '{filePath}':{ex.Message}");
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
                if (file?.uniquename == null) continue;

                string key = file.uniquename.ToLower();
                if (!existingWebResources.ContainsKey(key))
                {
                    existingWebResources[key] = file;
                }
            }

            var webresourceConfig = config.GetWebresourceConfig(this.Profile).FirstOrDefault();
            if (webresourceConfig == null)
            {
                config.webresources = new List<WebresourceDeployConfig>()
                {
                    new WebresourceDeployConfig()
                    {
                        files = new List<WebResourceFile>()
                    }
                };
            }

            var solutions = config.GetSolutionConfig(this.Profile);
            if (solutions == null || solutions.Length == 0)
                throw new SparkleTaskException(SparkleTaskException.ExceptionTypes.CONFIG_NOTFOUND, "Solution section not found in spkl.json. This is needed to determine where to store the webresources!");

            string rootPath = Path.Combine(config.filePath, webresourceConfig.root != null ? webresourceConfig.root : "");

            foreach (var solution in solutions)
            {
                var downloadedWebresources = ServiceLocator.Queries.GetWebresourcesInSolution(_context, solution.solution_uniquename);

                if (downloadedWebresources.Count == 0)
                    throw new SparkleTaskException(SparkleTaskException.ExceptionTypes.NO_WEBRESOURCES_FOUND, $"No webresources found to download in the solution '{solution.solution_uniquename}'");

                var maps = solution.map.Where(e => e.from.StartsWith("WebResources"));
                if (maps == null || maps.Count() == 0)
                    throw new SparkleTaskException(SparkleTaskException.ExceptionTypes.CONFIG_NOTFOUND, $"No maps section in the solution packager config. This is needed to determine where to store the webresources!");

                var webresourceRootFolder = String.IsNullOrEmpty(webresourceConfig.root) ?
                    filePath : Path.Combine(filePath, webresourceConfig.root);
                foreach (var resource in downloadedWebresources)
                {
                    var shortName = resource.Name;
                    var name = $"WebResources\\{shortName}".Replace("/", "\\");
                    var path = name;
                    var nameFolder = Path.GetDirectoryName(name);
                    var nameFile = Path.GetFileName(name);

                    foreach (var map in maps)
                    {
                        string from = RemoveTrailingFolderSeperator(map.from);
                        var fromfolder = Path.GetDirectoryName(map.from);
                        var fromfile = Path.GetFileName(map.from);

                        var to = Path.GetFullPath(Path.Combine(webresourceRootFolder, RemoveTrailingFolderSeperator(map.to)));

                        if (map.map == MapTypes.file && Wildcard(fromfile, nameFile))
                        {
                            path = Path.GetFullPath(Path.Combine(webresourceRootFolder, to));
                            break;
                        }
                        else if (map.map == MapTypes.path && name.StartsWith(fromfolder, StringComparison.InvariantCultureIgnoreCase)
                            && (fromfile == "*.*" || Wildcard(fromfile, nameFile)))
                        {
                            path = Path.GetFullPath(Path.Combine(webresourceRootFolder, name.Replace(fromfolder, to)));
                            break;
                        }
                        // fail through
                        path = Path.GetFullPath(Path.Combine(webresourceRootFolder, name));
                    }

                    var content = Convert.FromBase64String(resource.Content);

                    if (-1 != path.IndexOfAny(Path.GetInvalidPathChars()))
                        continue;

                    ServiceLocator.DirectoryService.SaveFile(path, content, Overwrite);

                    if (!existingWebResources.ContainsKey(resource.Name.ToLower()))
                    {
                        var configFolder = config.filePath;
                        configFolder += configFolder.EndsWith("\\") ? String.Empty : "\\";

                        var relFilePath = new Uri(configFolder).MakeRelativeUri(new Uri(path)).ToString();
                        relFilePath = relFilePath.Replace("/", "\\");
                        newWebResources.Add(new WebResourceFile()
                        {
                            uniquename = shortName,
                            displayname = resource.DisplayName,
                            description = resource.Description,
                            file = relFilePath
                        });
                        Console.WriteLine($"Added to spkl.json: {relFilePath}");
                    }
                }
            }

            if (webresourceConfig.files == null)
                webresourceConfig.files = new List<WebResourceFile>();
            webresourceConfig.files.AddRange(newWebResources);
            config.Save();

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



        private string RemoveTrailingFolderSeperator(string folder)
        {
            //Todo... this should be handled better with better comparison, mapping, etc.
            folder = folder.Replace("\\**", "");//.Replace("*.*","");
            while ((int)folder[folder.Length - 1] == (int)Path.DirectorySeparatorChar || (int)folder[folder.Length - 1] == (int)Path.AltDirectorySeparatorChar)
                folder = folder.Substring(0, folder.Length - 1);
            return folder;
        }

    }
}
