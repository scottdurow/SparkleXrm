using System.Collections.Generic;

namespace SparkleXrm.Tasks.Config
{
    public class WebResourceFile
    {
        public string uniquename;
        public string displayname;
        public string file;
        public string description;
        public WebresourceDependencies dependencies;
    }

    public class WebresourceDependencies
    {
        public List<LibraryDependency> libraries;
        public List<AttributeDependency> attributes;
    }

    public class LibraryDependency
    {
        public string uniquename;
        public string file;
        public string description;
        public string displayname;
    }

    public class AttributeDependency
    {
        public string attributename;
        public string entityname;
    }
}