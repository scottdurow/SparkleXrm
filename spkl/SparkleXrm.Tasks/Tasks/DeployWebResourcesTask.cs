using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using SparkleXrm.Tasks.Config;
using SparkleXrm.Tasks.Tasks.WebresourceDependencies;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

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
            var configs = ConfigFile.FindConfig(folder, true);
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

                WebResource webresource = ctx.GetWebResource(file.uniquename);
                if (webresource == null)
                {
                    webresource = new WebResource();
                }

                var fullPath = Path.Combine(webresourceRoot, file.file);
                var filecontent = Convert.ToBase64String(File.ReadAllBytes(fullPath));

                // Update
                webresource.Name = file.uniquename;
                webresource.DisplayName = file.displayname;
                webresource.Description = file.description;
                webresource.Content = filecontent;

                var webResourceFileInfo = new FileInfo(fullPath);
                WebResourceWebResourceType filetype = WebResourceWebResourceType.Script_JScript;
                switch (webResourceFileInfo.Extension.ToLower().TrimStart('.'))
                {
                    case "html":
                    case "htm":
                        filetype = WebResourceWebResourceType.Webpage_HTML;
                        break;
                    case "js":
                        filetype = WebResourceWebResourceType.Script_JScript;
                        break;
                    case "png":
                        filetype = WebResourceWebResourceType.PNGformat;
                        break;
                    case "gif":
                        filetype = WebResourceWebResourceType.GIFformat;
                        break;
                    case "jpg":
                    case "jpeg":
                        filetype = WebResourceWebResourceType.JPGformat;
                        break;
                    case "css":
                        filetype = WebResourceWebResourceType.StyleSheet_CSS;
                        break;
                    case "ico":
                        filetype = WebResourceWebResourceType.ICOformat;
                        break;
                    case "xml":
                        filetype = WebResourceWebResourceType.Data_XML;
                        break;
                    case "xsl":
                    case "xslt":
                        filetype = WebResourceWebResourceType.StyleSheet_XSL;
                        break;
                    case "xap":
                        filetype = WebResourceWebResourceType.Silverlight_XAP;
                        break;
                }

                //todo
                List<WebResource> libraryDependencies = null;
                List<AttributeObj> attributeDependencies = null;

                if (file.dependencies != null)
                {
                    //process dependency libraries
                    var libraryDependenciesFromConfig = file.dependencies.libraries;
                    if (libraryDependenciesFromConfig != null)
                    {
                        libraryDependencies = new List<WebResource>();
                        foreach (var library in libraryDependenciesFromConfig)
                        {
                            var libraryDependency = ctx.GetWebResource(library.uniquename);
                            if (libraryDependency == null)
                            {
                                libraryDependency = new WebResource();

                                var libraryFullPath = Path.Combine(webresourceRoot, library.file);
                                var libraryFilecontent = Convert.ToBase64String(File.ReadAllBytes(libraryFullPath));
                                var libraryFileInfo = new FileInfo(libraryFullPath);

                                libraryDependency.Name = library.uniquename;
                                libraryDependency.DisplayName = library.file;
                                libraryDependency.Content = libraryFilecontent;
                                libraryDependency.Description = library.description;

                                var libraryWebResType = WebResourceWebResourceType.Script_JScript;

                                switch (libraryFileInfo.Extension.ToLower().TrimStart('.'))
                                {
                                    case "js":
                                        libraryWebResType = WebResourceWebResourceType.Script_JScript;
                                        break;
                                    case "css":
                                        libraryWebResType = WebResourceWebResourceType.StyleSheet_CSS;
                                        break;
                                    case "html":
                                    case "htm":
                                        libraryWebResType = WebResourceWebResourceType.Webpage_HTML;
                                        break;
                                    case "resx":
                                        libraryWebResType = WebResourceWebResourceType.String_RESX;
                                        break;
                                    case "xml":
                                        libraryWebResType = WebResourceWebResourceType.Data_XML;
                                        break;
                                    default: throw new NotSupportedException("Supported files are: js, css, htm/html, resx, xml.");
                                }

                                libraryDependency.WebResourceType = new OptionSetValue((int)libraryWebResType);

                                _trace.WriteLine("Creating Library Dependency '{0}' -> '{1}'", library.file, library.uniquename);
                                libraryDependency.Id = _service.Create(libraryDependency);
                            }

                            libraryDependencies.Add(libraryDependency);
                        }
                    }

                    //process dependency attributes
                    var attributeDependenciesFromConfig = file.dependencies.attributes;
                    if (attributeDependenciesFromConfig != null)
                    {
                        attributeDependencies = new List<AttributeObj>();

                        foreach (var attr in attributeDependenciesFromConfig)
                        {
                            var entityName = attr.entityname;
                            var attributeName = attr.attributename;

                            var attrReq = new RetrieveAttributeRequest
                            {
                                EntityLogicalName = entityName,
                                LogicalName = attributeName,
                                RetrieveAsIfPublished = true
                            };

                            AttributeMetadata attrMetadata = null;

                            try
                            {
                                attrMetadata = ((RetrieveAttributeResponse)_service.Execute(attrReq)).AttributeMetadata;
                            }
                            catch (Exception ex)
                            {
                                //var message = "'" + attributeName + "' does not exist in '" + entityName + "' entity";
                                _trace.WriteLine(ex.Message);
                                continue;
                            }

                            if (attrMetadata != null)
                            {
                                var newAttributeObj = new AttributeObj();
                                newAttributeObj.AttributeId = attrMetadata.MetadataId.Value.ToString();
                                newAttributeObj.AttributeName = attrMetadata.LogicalName;
                                newAttributeObj.EntityName = attrMetadata.EntityLogicalName;

                                attributeDependencies.Add(newAttributeObj);
                            }
                        }
                    }
                }
                else
                {
                    _trace.WriteLine("No Processing dependencies for webresource '{0}'", file.uniquename);
                }

                webresource.WebResourceType = new OptionSetValue((int)filetype);
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

                AddDependencyWebresourcesToWebresource(webresource, libraryDependencies, attributeDependencies);

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

        private void AddDependencyWebresourcesToWebresource(WebResource webresource, List<WebResource> libraryDependencies, List<AttributeObj> attributeDependencies)
        {
            if (libraryDependencies == null) return;

            _trace.WriteLine("Updating dependnecies for webresource with name: '{0}'", webresource.Name);

            //todo
            var isDependencyXmlEmpty = false;
            var dependencyXml = webresource.DependencyXml;

            var serializer = new XmlSerializer(typeof(Dependencies));

            //create obj from xml
            Dependencies dependencyObj = null;
            if (string.IsNullOrEmpty(dependencyXml))
            {
                isDependencyXmlEmpty = true;

                var componentTypes = new List<Dependency>();
                componentTypes.Add(new Dependency() { componentType = "WebResource", Library = new List<Library>() });
                componentTypes.Add(new Dependency() { componentType = "Attribute", Attribute = new List<Tasks.WebresourceDependencies.Attribute>() });

                dependencyObj = new Dependencies();
                dependencyObj.Dependency = componentTypes;
            }
            else
            {
                using (var sr = new StringReader(dependencyXml))
                {
                    dependencyObj = (Dependencies)serializer.Deserialize(sr);
                }
            }

            //change dependencyXml
            var newDependencyXml = "";

            var webresourceToUpdate = new WebResource();
            webresourceToUpdate.Id = webresource.Id;

            //process componentType = WebResource
            var newLibraryDependencies = new List<Library>();
            var webResourceComponentType = dependencyObj.Dependency.FirstOrDefault(x => x.componentType == "WebResource");

            foreach (var libDep in libraryDependencies)
            {
                var lib = webResourceComponentType.Library.FirstOrDefault(x => x.name == libDep.Name);
                if (lib == null)
                {
                    var newLib = new Library();
                    newLib.name = libDep.Name;
                    newLib.displayName = libDep.DisplayName;
                    newLib.libraryUniqueId = "{" + Guid.NewGuid().ToString() + "}";
                    newLib.languagecode = string.Empty;
                    newLib.description = libDep.Description;
                    newLibraryDependencies.Add(newLib);
                }
            }

            webResourceComponentType.Library.AddRange(newLibraryDependencies);

            //process componentType = Attribute
            var newAttributeDependencies = new List<Tasks.WebresourceDependencies.Attribute>();
            var attributeComponentType = dependencyObj.Dependency.FirstOrDefault(x => x.componentType == "Attribute");

            if (attributeDependencies != null)
            {
                foreach (var attrDep in attributeDependencies)
                {
                    var attr = attributeComponentType.Attribute.FirstOrDefault(x => x.attributeName == attrDep.AttributeName && x.entityName == attrDep.EntityName);
                    if (attr == null)
                    {
                        var newAttribute = new Tasks.WebresourceDependencies.Attribute();
                        newAttribute.attributeId = attrDep.AttributeId;
                        newAttribute.attributeName = attrDep.AttributeName;
                        newAttribute.entityName = attrDep.EntityName;
                        newAttributeDependencies.Add(newAttribute);
                    }
                }
            }

            if (newAttributeDependencies.Count > 0)
            {
                attributeComponentType.Attribute.AddRange(newAttributeDependencies);
            }

            //compose new dependency xml
            using (var sw = new StringWriter())
            {
                var settings = new XmlWriterSettings();
                settings.Indent = false;
                settings.OmitXmlDeclaration = true;
                settings.NewLineHandling = NewLineHandling.None;
                using (XmlWriter writer = XmlWriter.Create(sw, settings))
                {
                    var emptyNamepsaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
                    serializer.Serialize(writer, dependencyObj, emptyNamepsaces);
                    newDependencyXml = sw.ToString();
                }
            }

            //update
            //_trace.WriteLine($"newXml:{newDependencyXml}");
            webresourceToUpdate.DependencyXml = newDependencyXml;
            _service.Update(webresourceToUpdate);
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
