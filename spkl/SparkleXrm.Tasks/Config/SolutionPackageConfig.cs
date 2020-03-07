using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;

namespace SparkleXrm.Tasks.Config
{
    public enum PackageType
    {
        unmanaged,
        managed,
        both_unmanaged_import,
        both_managed_import
    }
    public class SolutionPackageConfig
    {
        public string profile; 
        public string solution_uniquename; // unique name of the solution to pack/unpack
        public string packagepath; // Relative folder to unpack the package to
        public string solutionpath; // Path and name of the solution - e.g. solution/MySolution_{0}_{1}_{2}_{3}.zip
        [JsonConverter(typeof(StringEnumConverter))]
        public PackageType packagetype;
        public string diffpath;
        public bool increment_on_import; // Increment the minor version of the solution after importing from the package folder
        public List<SolutionPackageMap> map; // Map source files to the package folder
        public bool clobber = true;
        public bool allowwrite = true;
        public bool allowdelete = true;
    }
}