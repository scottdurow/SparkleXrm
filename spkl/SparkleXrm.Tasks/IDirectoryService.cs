using System.Collections.Generic;

namespace SparkleXrm.Tasks
{
    public interface IDirectoryService
    {
        string GetApplicationDirectory();
        string Search(string path, string search);
        List<string> Search(string path, string search, List<string> matches);
    }
}