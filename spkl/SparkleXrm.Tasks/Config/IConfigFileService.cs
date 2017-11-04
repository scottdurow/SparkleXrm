using System.Collections.Generic;

namespace SparkleXrm.Tasks.Config
{
    public interface IConfigFileService
    {
        List<ConfigFile> FindConfig(string folder, bool raiseErrorIfNotFound = true);
    }
}