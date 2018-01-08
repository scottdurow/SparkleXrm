using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using SparkleXrm.Tasks.Config;
using SparkleXrm.Tasks.Tasks.WebresourceDependencies;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
namespace SparkleXrm.Tasks
{
    public class DownloadWebresourceConfigTask : BaseTask
    {

        public DownloadWebresourceConfigTask(IOrganizationService service, ITrace trace) : base(service, trace)
        {
        }
        public DownloadWebresourceConfigTask(OrganizationServiceContext ctx, ITrace trace) : base(ctx, trace)
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
                // Add a webresource seciton
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
                else
                {
                    var duplicate = new SparkleTaskException(SparkleTaskException.ExceptionTypes.DUPLICATE_FILE, String.Format("Duplicate file in webresource config '{0}'. Config at '{1}'", file.file, filePath));
                    throw duplicate;
                }
            }

            var webresources = DirectoryEx.Search(filePath, "*.js|*.htm|*.css|*.xap|*.png|*.jpeg|*.jpg|*.gif|*.ico|*.xml", null);

            // check there is a prefix supplied
            if (string.IsNullOrWhiteSpace(Prefix))
            {
                throw new SparkleTaskException(SparkleTaskException.ExceptionTypes.MISSING_PREFIX, "Please supply the prefix for your webresources e.g. /p:new");
            }
            // Get a list of all webresources!
            var matchList = ctx.GetWebresources().ToDictionary(w => w.Name.ToLower().Replace(Prefix + (Prefix.EndsWith("_") ? "" : "_"), ""));

            //todo
            //foreach (var item in matchList)
            //{
            //    _trace.WriteLine($"libName:{item.Key}");
            //}

            //throw new Exception("Stop here");



            var webresourceConfig = config.GetWebresourceConfig(null).FirstOrDefault();
            if (webresourceConfig == null)
                throw new Exception("Cannot find webresource section in spkl.json");

            string rootPath = Path.Combine(config.filePath, webresourceConfig.root != null ? webresourceConfig.root : "");
            foreach (var filename in webresources)
            {
                _trace.WriteLine("Found '{0}'", filename);
                var file = filename.Replace("\\", "/").ToLower();

                // Find if there are any matches
                var match = matchList.Keys.Where(w => file.Contains(w)).FirstOrDefault();

                if (match != null)
                {
                    _trace.WriteLine(String.Format("Found webresource {0}", match));
                    var webresource = matchList[match];
                    var webresourcePath = filename.Replace(rootPath, "").TrimStart('\\').TrimStart('/');










                    //todo - handle webresource dependencies
                    var serializer = new XmlSerializer(typeof(Dependencies));

                    var dependencyXml = webresource.DependencyXml;
                    Dependencies dependencyObj = null;
                    if (!string.IsNullOrEmpty(dependencyXml))
                    {
                        using (var sr = new StringReader(dependencyXml))
                        {
                            dependencyObj = (Dependencies)serializer.Deserialize(sr);
                        }
                    }

                    if (dependencyObj != null)
                    {
                        //handle libraries
                        var libraries = dependencyObj.Dependency.FirstOrDefault(x => x.componentType.Equals("WebResource"));


                        //handle attributes
                        var attributes = dependencyObj.Dependency.FirstOrDefault(x => x.componentType.Equals("Attribute"));
                    }




                    // is it already in the config file
                    var existingMatch = existingWebResources.Keys.Where(w => webresource.Name.Contains(w)).FirstOrDefault();
                    if (existingMatch != null)
                    {
                        continue;
                    }

                    var webresourceFile = new WebResourceFile
                    {
                        uniquename = webresource.Name,
                        file = webresourcePath,
                        displayname = "" + webresource.DisplayName,
                        description = "" + webresource.Description
                    };

                    // If the displayname is the same as the uniquename then we only need the unique name
                    if (webresourceFile.displayname == webresourceFile.uniquename)
                        webresourceFile.displayname = null;
                    newWebResources.Add(webresourceFile);
                }
            }

            if (webresourceConfig.files == null)
                webresourceConfig.files = new List<WebResourceFile>();
            webresourceConfig.files.AddRange(newWebResources);
            config.Save();
        }

        private string[] ExtractPath(string webresourceName)
        {
            return webresourceName.Split('/');
        }
    }
}
