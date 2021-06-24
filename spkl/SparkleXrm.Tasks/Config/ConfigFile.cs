using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SparkleXrm.Tasks.Config
{
    public class ConfigFile
    {
        public List<WebresourceDeployConfig> webresources;
        public List<PluginDeployConfig> plugins;
        public List<EarlyBoundTypeConfig> earlyboundtypes;
        public List<SolutionPackageConfig> solutions;

        [JsonIgnore]
        public string filePath;

        public virtual void Save()
        {
            var file = Path.Combine(filePath, "spkl.json");

            /* No need to make a backup copy - with Git you can always undo a commit to revert any changes */
            //if (File.Exists(file))
            //{
            //    File.Copy(file, file + DateTime.Now.ToString("yyyyMMddHHmmss") + ".bak");
            //}
            File.WriteAllText(file, Newtonsoft.Json.JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            }));
        }

        public virtual SolutionPackageConfig[] GetSolutionConfig(string profile)
        {
            if (solutions == null)
                return new SolutionPackageConfig[0];

            SolutionPackageConfig[] config = null;
            if (profile == "default")
            {
                profile = null;
            }
            if (profile != null)
            {
                config = solutions.Where(c => c.profile != null && c.profile.Split(',').Contains(profile)).ToArray();
            }
            else
            {
                // Default profile or empty
                config = solutions.Where(c => c.profile == null || c.profile.Split(',').Contains("default") || String.IsNullOrWhiteSpace(c.profile)).ToArray();
            }

            return config;
        }

        public virtual EarlyBoundTypeConfig[] GetEarlyBoundConfig(string profile)
        {
            if (earlyboundtypes == null)
                return new EarlyBoundTypeConfig[] { new EarlyBoundTypeConfig(){
                    filename ="Entities.cs",
                    entities = "account,contact",
                    classNamespace = "Xrm",
                    generateOptionsetEnums = true,
                    generateStateEnums = true
                } };

            EarlyBoundTypeConfig[] config = null;
            if (profile == "default")
            {
                profile = null;
            }
            if (profile != null)
            {
                config = earlyboundtypes.Where(c => c.profile != null && c.profile.Replace(" ", "").Split(',').Contains(profile)).ToArray();
            }
            else
            {
                // Default profile or empty
                config = earlyboundtypes.Where(c => c.profile == null || c.profile.Replace(" ", "").Split(',').Contains("default") || String.IsNullOrWhiteSpace(c.profile)).ToArray();
            }

            return config;
        }

        public virtual WebresourceDeployConfig[] GetWebresourceConfig(string profile)
        {
            if (webresources == null)
                return new WebresourceDeployConfig[0];

            WebresourceDeployConfig[] config = null;
            if (profile == "default")
            {
                profile = null;
            }
            if (profile != null)
            {
                config = webresources.Where(c => c.profile != null && c.profile.Replace(" ", "").Split(',').Contains(profile)).ToArray();
            }
            else
            {
                // Default profile or empty
                config = webresources.Where(c => c.profile == null || c.profile.Replace(" ", "").Split(',').Contains("default") || String.IsNullOrWhiteSpace(c.profile)).ToArray();
            }

            return config;
        }

        public virtual PluginDeployConfig[] GetPluginsConfig(string profile)
        {
            PluginDeployConfig[] config = null;
            if (plugins == null)
                return new PluginDeployConfig[0];

            if (profile == "default")
            {
                profile = null;
            }

            if (profile != null)
            {
                config = plugins.Where(c => c.profile != null && c.profile.Replace(" ", "").Split(',').Contains(profile)).ToArray();
            }
            else
            {
                // Default profile or empty
                config = plugins.Where(c => c.profile == null || c.profile.Replace(" ", "").Split(',').Contains("default") || String.IsNullOrWhiteSpace(c.profile)).ToArray();
            }

            return config;
        }

        public virtual List<string> GetAssemblies(PluginDeployConfig plugin)
        {
            var file = plugin.assemblypath;

            List<string> assemblies;
            var extension = Path.GetExtension(file);

            if (extension == "") file = Path.Combine(file, "*.dll");

            assemblies = ServiceLocator.DirectoryService.Search(this.filePath, file);
            return assemblies;
        }
    }

    public class ConfigFileService : IConfigFileService
    {
        public List<ConfigFile> FindConfig(string folder, bool raiseErrorIfNotFound = true)
        {
            List<string> configfilePath = null;
            // search for the config file - using path or absolute location
            if (folder.EndsWith("spkl.json") && File.Exists(folder))
            {
                configfilePath = new List<string> { folder };
            }
            else
            {
                configfilePath = ServiceLocator.DirectoryService.Search(folder, "spkl.json");
            }

            if (raiseErrorIfNotFound && (configfilePath == null || configfilePath.Count == 0))
            {
                throw new SparkleTaskException(SparkleTaskException.ExceptionTypes.CONFIG_NOTFOUND, String.Format("Cannot find spkl.json in at '{0}' - make sure it is in the same folder or sub-folder as spkl.exe or provide a [path]", folder));
            }

            var results = new List<ConfigFile>();

            foreach (var configPath in configfilePath)
            {
                // Check valid path and this is not the nuget package folder
                if (configPath != null && !Regex.IsMatch(configPath, @"packages\\spkl"))
                {
                    var config = Newtonsoft.Json.JsonConvert.DeserializeObject<ConfigFile>(File.ReadAllText(configPath));
                    config.filePath = Path.GetDirectoryName(configPath);
                    results.Add(config);
                }
            }

            if (results.Count == 0)
            {
                results.Add(new ConfigFile
                {
                    filePath = folder
                });
            }

            return results;
        }
    }
}