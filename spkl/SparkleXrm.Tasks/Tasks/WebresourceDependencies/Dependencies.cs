using System.Collections.Generic;
using System.Xml.Serialization;

namespace SparkleXrm.Tasks.Tasks.WebresourceDependencies
{
    // Helper classes for Webresource Dependency support
    public class Attribute
    {
        [XmlAttribute]
        public string attributeId { get; set; }
        [XmlAttribute]
        public string attributeName { get; set; }
        [XmlAttribute]
        public string entityName { get; set; }
    }

    public class Library
    {
        [XmlAttribute]
        public string name { get; set; }
        [XmlAttribute]
        public string displayName { get; set; }
        [XmlAttribute]
        public string languagecode { get; set; }
        [XmlAttribute]
        public string description { get; set; }
        [XmlAttribute]
        public string libraryUniqueId { get; set; }
    }

    public class Dependency
    {
        [XmlAttribute]
        public string componentType { get; set; }

        [XmlElement("Library")]
        public List<Library> Library { get; set; }

        [XmlElement("Attribute")]
        public List<Attribute> Attribute { get; set; }
    }

    [XmlRoot("Dependencies")]
    public class Dependencies
    {
        [XmlElement("Dependency")]
        public List<Dependency> Dependency { get; set; }
    }

    public class AttributeObj
    {
        public string AttributeId { get; set; }
        public string AttributeName { get; set; }
        public string EntityName { get; set; }
    }
}
