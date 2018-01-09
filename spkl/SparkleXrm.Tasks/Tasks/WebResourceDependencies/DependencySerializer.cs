using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Linq;
using System;

namespace SparkleXrm.Tasks.Tasks.WebresourceDependencies
{
    public class DependencySerializer
    {
        public static Dependencies GetDependencyObj(XmlSerializer serializer, string dependencyXml)
        {
            Dependencies dependencyObj = null;
            if (string.IsNullOrEmpty(dependencyXml))
            {
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

            return dependencyObj;
        }

        public static Dependency GetComponentType(Dependencies dependencyObj, string componentType)
        {
            return dependencyObj.Dependency.FirstOrDefault(x => x.componentType == componentType);
        }

        public static Library GetLibrary(List<Library> libraries, string libraryName)
        {
            return libraries.FirstOrDefault(x => x.name == libraryName);
        }

        public static Attribute GetAttribute(List<Attribute> attributes, string attributeName, string entityName)
        {
            return attributes.FirstOrDefault(x => x.attributeName == attributeName && x.entityName == entityName);
        }

        public static Library CreateLibrary(WebResource webResource)
        {
            var newLib = new Library();
            newLib.name = webResource.Name;
            newLib.displayName = webResource.DisplayName;
            newLib.libraryUniqueId = "{" + Guid.NewGuid().ToString() + "}";
            newLib.languagecode = string.Empty;
            newLib.description = webResource.Description;

            return newLib;
        }

        public static Attribute CreateAttribute(AttributeObj attrObj)
        {
            var newAttribute = new Attribute();
            newAttribute.attributeId = attrObj.AttributeId;
            newAttribute.attributeName = attrObj.AttributeName;
            newAttribute.entityName = attrObj.EntityName;

            return newAttribute;
        }
    }
}
