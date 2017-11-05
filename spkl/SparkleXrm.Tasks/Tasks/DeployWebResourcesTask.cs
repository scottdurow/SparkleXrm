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
            var configs = ServiceLocator.ConfigFileFactory.FindConfig(folder,true);
            foreach (var config in configs)
            {
                _trace.WriteLine("Using Config '{0}'", config.filePath);
                var configSections = config.GetWebresourceConfig(this.Profile);
                if (configSections==null)
                {
                    _trace.WriteLine("No webresource config found");
                    return;
                }
                foreach (var webresources in configSections)
                {
                    var ids = DeployWebresources(ctx, config, webresources);
                    _trace.WriteLine("Publishing {0} webresources...", webresources.files.Count);
                    PublishWebresources(ids);
                    _trace.WriteLine("Publish complete.");
                }
                _trace.WriteLine("Processed {0} section(s)", configSections.Length);
            }

            _trace.WriteLine("Processed {0} config(s)", configs.Count);
        }

        public IEnumerable<Guid> DeployWebresources(OrganizationServiceContext ctx, ConfigFile config, WebresourceDeployConfig webresources)
        {
            var webresourceRoot = Path.Combine(config.filePath, "" + webresources.root);

            this.Solution = webresources.solution;

            var webresourcesToPublish = new List<Guid>(webresources.files.Count);

            foreach (var file in webresources.files)
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
                    webresource = new WebResource();
                var fullPath = Path.Combine(webresourceRoot, file.file);
                var filecontent = Convert.ToBase64String(File.ReadAllBytes(fullPath));

                // Update
                webresource.Name = file.uniquename;
                webresource.DisplayName = file.displayname;
                webresource.Description = file.description;
                webresource.Content = filecontent;

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
                }
                webresource.WebResourceType = filetype;
                if (webresource.Id == Guid.Empty)
                {
                    _trace.WriteLine("Creating Webresource '{0}' -> '{1}'", file.file, file.uniquename);
                    // create
                    webresource.Id = _service.Create(webresource);
                }
                else
                {
                    _trace.WriteLine("Updating Webresource '{0}' -> '{1}'", file.file, file.uniquename);
                    // Update
                    _service.Update(webresource);
                }
                ;

                // Add to solution
                if (Solution != null)
                {
                    AddWebresourceToSolution(Solution, webresource);
                }

                webresourcesToPublish.Add(webresource.Id);
            }
            _trace.WriteLine("Deployed {0} webresource(s)", webresources.files.Count);

            return webresourcesToPublish;
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
