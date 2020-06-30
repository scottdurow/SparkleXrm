using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SparkleXrm.Tasks.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparkleXrm.Tasks
{
    public class DeployWebResourcesTask : BaseTask
    {
        public DeployWebResourcesTask(IOrganizationService service, ITrace trace) : base(service, trace)
        {
        }

        protected override void ExecuteInternal(string folder, OrganizationServiceContext ctx)
        {
            // Iterate through it and register/update each webresource
            var configs = ServiceLocator.ConfigFileFactory.FindConfig(folder, true);
            foreach (var config in configs)
            {
                _trace.WriteLine("Using Config '{0}'", config.filePath);
                var configSections = config.GetWebresourceConfig(this.Profile);
                if (configSections == null)
                {
                    _trace.WriteLine("No webresource config found");
                    return;
                }
                foreach (var webresources in configSections)
                {
                    List<Guid> ids = DeployWebresources(ctx, config, webresources);
                    _trace.WriteLine("Publishing {0} webresources...", ids.Count);
                    PublishWebresources(ids);
                    _trace.WriteLine("Publish complete.");
                }
                _trace.WriteLine("Processed {0} section(s)", configSections.Length);
            }

            _trace.WriteLine("Processed {0} config(s)", configs.Count);
        }

        public List<Guid> DeployWebresources(OrganizationServiceContext ctx, ConfigFile config, WebresourceDeployConfig webresources)
        {
            var webresourceRoot = Path.Combine(config.filePath, "" + webresources.root);

            this.Solution = webresources.solution;

            bool autodetect = false;
            if (webresources.autodetect != null)
            {
                switch (webresources.autodetect.ToLower())
                {
                    case "yes":
                    case "true":
                        autodetect = true;
                        break;
                    case "no":
                    case "false":
                        autodetect = false;
                        break;
                    default:
                        throw new SparkleTaskException(SparkleTaskException.ExceptionTypes.WEBRESOURCE_ERROR, $"Invalid autodetect settng found: '{webresources.autodetect}'");
                }
            }
            var webresourcesToPublish = new List<Guid>();

            if (autodetect)
            {
                AutoDetect(ctx, webresourceRoot, webresources, webresourcesToPublish);
            }
            else
            {
                foreach (var file in webresources.files)
                {
                    DeployWebResource(ctx, webresourceRoot, webresourcesToPublish, file);
                }
            }
            _trace.WriteLine("Deployed {0} webresource(s)", webresourcesToPublish.Count);
            return webresourcesToPublish;
        }

        private void AutoDetect(OrganizationServiceContext ctx, string webresourceRoot, WebresourceDeployConfig webresources, List<Guid> webresourcesToPublish)
        {
            if (Solution == null)
            {
                throw new SparkleTaskException(SparkleTaskException.ExceptionTypes.WEBRESOURCE_ERROR, $"Solution name required for AutoDetect.");
            }
            if (webresources.deleteaction != null)
            {
                switch (webresources.deleteaction.ToLower())
                {
                    case "delete":
                        webresources.deleteaction = webresources.deleteaction.ToLower();
                        break;
                    case "remove":
                        webresources.deleteaction = webresources.deleteaction.ToLower();
                        break;
                    case "no":
                    case "none":
                    case "leave":
                    case "nothing":
                        webresources.deleteaction = "no";
                        break;
                    default:
                        throw new SparkleTaskException(SparkleTaskException.ExceptionTypes.WEBRESOURCE_ERROR, $"Invalid deleteaction settng found: '{webresources.deleteaction}'");
                }
            }
            else
            {
                webresources.deleteaction = "no";
            }
            if (!Directory.Exists(webresourceRoot))
            {
                throw new SparkleTaskException(SparkleTaskException.ExceptionTypes.WEBRESOURCE_ERROR, $"WebResource source folder not found: '{webresourceRoot}'");
            }

            // Get existing solution WRs
            List<WebResource> curWebResources = ServiceLocator.Queries.GetWebresourcesInSolution(ctx, this.Solution);
            _trace.WriteLine(string.Format("{0} Current WebResourceCount: {1}", this.Solution, curWebResources.Count));

            // Make sure the delimiters are all the same & remove any trailing delims
            webresourceRoot = webresourceRoot.Replace('/', '\\');
            while (webresourceRoot.Substring(webresourceRoot.Length - 1, 1) == "\\")
            {
                webresourceRoot = webresourceRoot.Substring(0, webresourceRoot.Length - 1);
            }
            
            DeployDirectory(ctx, webresourceRoot, webresourceRoot, webresourcesToPublish, curWebResources);
            _trace.WriteLine(string.Format("{0} WebResources no longer present in directory", curWebResources.Count));

            if (webresources.deleteaction == "delete" || webresources.deleteaction == "remove")
            {
                bool errEncountered = false;
                foreach (var curWebResource in curWebResources)
                {
                    try
                    {
                        if (webresources.deleteaction == "delete")
                        {
                            _trace.WriteLine(string.Format("Deleting '{0}'", curWebResource.Name));
                            _service.Delete(WebResource.EntityLogicalName, curWebResource.Id);
                        }
                        else
                        {
                            _trace.WriteLine(string.Format("Remove from solution '{0}'", curWebResource.Name));
                            RemoveWebresourceFromSolution(this.Solution, curWebResource);
                        }
                    }
                    catch (Exception ex)
                    {
                        errEncountered = true;
                        _trace.WriteLine(string.Format("Error processing Delete/Remove - {0}", ex.Message));
                    }
                }
                if (errEncountered)
                {
                    throw new SparkleTaskException(SparkleTaskException.ExceptionTypes.WEBRESOURCE_ERROR, $"Error encountered processing {webresources.deleteaction} for missing files.");
                }
            }
        }

        private void DeployDirectory(OrganizationServiceContext ctx, string webresourceRoot, string directory, List<Guid> webresourcesToPublish, List<WebResource> curWebResources)
        {
            //_trace.WriteLine("DeployDirectory: " + directory);
            var webResSplit = webresourceRoot.Split('\\');

            var fullPath = Path.Combine(webresourceRoot, directory);

            string[] fileEntries = Directory.GetFiles(directory);
            foreach (string fileName in fileEntries)
            {
                // Strip WebResource root from the filename
                string relativePathFileName = string.Join("\\", fileName.Split('\\').Skip(webResSplit.Count()));
                WebResourceFile file = new WebResourceFile
                {
                    file = relativePathFileName
                };
                DeployWebResource(ctx, webresourceRoot, webresourcesToPublish, file);

                WebResource webResourceListItem = curWebResources.FirstOrDefault(wr => wr.Name.ToLower() == relativePathFileName.ToLower().Replace("\\", "/"));
                if (webResourceListItem != null)
                {
                    curWebResources.Remove(webResourceListItem);
                }
            }

            // Recurse into subdirectories of this directory.
            string[] subdirectoryEntries = Directory.GetDirectories(directory);
            foreach (string subdirectory in subdirectoryEntries)
            {
                DeployDirectory(ctx, webresourceRoot, subdirectory, webresourcesToPublish, curWebResources);
            }
        }

        private void DeployWebResource(OrganizationServiceContext ctx, string webresourceRoot, List<Guid> webresourcesToPublish, WebResourceFile file)
        {
            if (file.uniquename == null)
            {
                // Make the name uniquename the same as the file name
                file.uniquename = file.file.Replace("\\", "/");
            }
            if (file.displayname == null)
            {
                // make the same as the unique name
                file.displayname = file.uniquename;
            }
            WebResource webresource = ServiceLocator.Queries.GetWebResource(ctx, file.uniquename);

            if (webresource == null)
            {
                webresource = new WebResource();
            }
            var fullPath = Path.Combine(webresourceRoot, file.file);
            var filecontent = Convert.ToBase64String(File.ReadAllBytes(fullPath));

            if (webresource.Content == filecontent)
            {
                _trace.WriteLine($"{file.uniquename} has not changed, skipping...");
                return;
            }

            // Update
            webresource.Name = file.uniquename;
            webresource.DisplayName = file.displayname;
            webresource.Description = file.description;
            webresource.Content = filecontent;

            if (file.languagecode.HasValue)
                webresource.LanguageCode = file.languagecode;

            if (webresource.Id == Guid.Empty)
            {
                var webResourceFileInfo = new FileInfo(fullPath);
                webresource_webresourcetype filetype = webresource_webresourcetype.Script_JScript;
                switch (webResourceFileInfo.Extension.ToLower().TrimStart('.'))
                {
                    case "html":
                    case "htm":
                        filetype = webresource_webresourcetype.Webpage_HTML;
                        break;
                    case "js":
                        filetype = webresource_webresourcetype.Script_JScript;
                        break;
                    case "png":
                        filetype = webresource_webresourcetype.PNGformat;
                        break;
                    case "gif":
                        filetype = webresource_webresourcetype.GIFformat;
                        break;
                    case "jpg":
                    case "jpeg":
                        filetype = webresource_webresourcetype.JPGformat;
                        break;
                    case "css":
                        filetype = webresource_webresourcetype.StyleSheet_CSS;
                        break;
                    case "ico":
                        filetype = webresource_webresourcetype.ICOformat;
                        break;
                    case "xml":
                        filetype = webresource_webresourcetype.Data_XML;
                        break;
                    case "xsl":
                    case "xslt":
                        filetype = webresource_webresourcetype.StyleSheet_XSL;
                        break;
                    case "xap":
                        filetype = webresource_webresourcetype.Silverlight_XAP;
                        break;
                    case "svg":
                        filetype = webresource_webresourcetype.Vectorformat_SVG;
                        break;
                    case "resx":
                        filetype = webresource_webresourcetype.Resourceformat_RESX;
                        break;
                    default:
                        _trace.WriteLine("File extension '{0}' unexpected -> '{1}'", webResourceFileInfo.Extension, file.file);
                        return;
                }
                webresource.WebResourceType = filetype;

                _trace.WriteLine("Creating Webresource '{0}' -> '{1}'", file.file, file.uniquename);
                // Create
                webresource.Id = _service.Create(webresource);
            }
            else
            {
                _trace.WriteLine("Updating Webresource '{0}' -> '{1}'", file.file, file.uniquename);
                // Update
                _service.Update(webresource);
            }

            // Add to solution
            if (Solution != null)
            {
                AddWebresourceToSolution(Solution, webresource);
            }

            webresourcesToPublish.Add(webresource.Id);
        }

        private void AddWebresourceToSolution(string solutionName, WebResource webresource)
        {
            // Find solution
            AddSolutionComponentRequest addToSolution = new AddSolutionComponentRequest()
            {
                AddRequiredComponents = true,
                ComponentType = (int)componenttype.WebResource,
                ComponentId = webresource.Id,
                SolutionUniqueName = solutionName
            };
            _trace.WriteLine("Adding to solution '{0}'", solutionName);
            _service.Execute(addToSolution);
        }

        private void RemoveWebresourceFromSolution(string solutionName, WebResource webresource)
        {
            // Find solution
            RemoveSolutionComponentRequest removeFromSolution = new RemoveSolutionComponentRequest()
            {
                ComponentType = (int)componenttype.WebResource,
                ComponentId = webresource.Id,
                SolutionUniqueName = solutionName
            };
            _trace.WriteLine(string.Format("Solution: '{0}' Removing: '{1}'", solutionName, webresource.Name));
            _service.Execute(removeFromSolution);
        }

        public void PublishWebresources(IEnumerable<Guid> guids)
        {
            if (!guids.Any()) return;

            var stringGuids = guids.Select(g => g.ToString());
            var webresources = string.Join("</webresource><webresource>", stringGuids);

            // Publish
            var publish = new PublishXmlRequest
            {
                ParameterXml =
                    "<importexportxml><webresources>" +
                    "<webresource>" + webresources + "</webresource>" +
                    "</webresources></importexportxml>"
            };
            _service.Execute(publish);
        }

    }
}
