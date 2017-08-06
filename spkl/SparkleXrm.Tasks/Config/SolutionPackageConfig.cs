namespace SparkleXrm.Tasks.Config
{
    public class SolutionPackageConfig
    {
        public string profile; 
        public string solution_uniquename; // unique name of the solution to pack/unpack
        public string packagepath; // Relative folder to unpack the package to
        public string diffpath;
        public bool increment_on_import; // Increment the minor version of the solution after importing from the package folder

        
    }
}