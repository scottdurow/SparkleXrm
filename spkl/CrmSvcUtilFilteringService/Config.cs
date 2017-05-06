using SparkleXrm.Tasks;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace spkl.CrmSvcUtilExtensions
{
    public class Config
    {
        private static Dictionary<string, string> _settings;
        private static Dictionary<string, string> GetSettings(string path)
        {
            Console.WriteLine("Loading config from '{0}'", path);
            var document = XDocument.Load(path);

            var root = document.Root;
            var results =
              root
                .Elements()
                .ToDictionary(element => element.Name.ToString(), element => element.Value);

            return results;

        }
        
        static Config()
        {
            string path = DirectoryEx.GetApplicationDirectory();
            var configPath = new FileInfo(Path.Combine(path, "spkl.crmsvcutil.config"));
            if (!configPath.Exists)
                throw new Exception(String.Format("Cannot find config file at '{0}'",configPath.FullName));

            _settings = GetSettings(configPath.FullName);
        }

        public static string GetConfig(string name)
        {
            if (_settings.ContainsKey(name))
            {
                return _settings[name];
            }
            else
                return null;
            
        }

        public static List<string> GetEntities()
        {
            var entityList = _settings["entities"];
            if (entityList == null)
                throw new Exception("Cannot find the list of entities in the config file");

            return new List<string>(entityList.Replace(" ", "").Split(',')); ;
        }

        public static List<string> GetMessageFilter()
        {
            var messageList = _settings["messages"];
            if (messageList == null)
                throw new Exception("Cannot find the list of messages in the config file");

            return new List<string>(messageList.Replace(" ", "").Split(',')); ;
        }
    }
}
