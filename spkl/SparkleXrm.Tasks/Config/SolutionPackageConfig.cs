using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;

namespace SparkleXrm.Tasks.Config
{
    public enum PackageType
    {
        unmanaged,
        managed
    }
    public class SolutionPackageConfig
    {
        public string profile; 
        public string solution_uniquename; // unique name of the solution to pack/unpack
        public string packagepath; // Relative folder to unpack the package to
        [JsonConverter(typeof(StringEnumConverter))]
        public PackageType packagetype;
        public string diffpath;
        public bool increment_on_import; // Increment the minor version of the solution after importing from the package folder
        public List<SolutionPackageMap> map; // Map source files to the package folder
    }
}