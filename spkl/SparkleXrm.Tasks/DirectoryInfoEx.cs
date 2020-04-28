using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.Extensions.FileSystemGlobbing.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SparkleXrm.Tasks
{
    public class DirectoryService : IDirectoryService
    {
        public string GetApplicationDirectory()
        {
            string path = new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName;

            return path;
        }

        public string SimpleSearch(string path, string search)
        {
            try
            {
                foreach (string f in Directory.GetFiles(path, search, SearchOption.AllDirectories))
                {
                    return f;
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        public List<string> Search(string path, string search)
        {
            var matcher = new Matcher(StringComparison.InvariantCultureIgnoreCase);
            if (search.StartsWith("..") || search.StartsWith("**") || search.StartsWith("\\"))
            {
                matcher.AddInclude(search);
            }
            else
            {
                foreach (var item in search.Split('|'))
                {
                    matcher.AddInclude("**/" + item);
                }
            }
            
            var globbMatch = matcher.Execute(new DirectoryInfoWrapper(new DirectoryInfo(path)));

            var matchList = new List<string>();
            foreach (var file in globbMatch.Files)
            {
                var fullFilePath = Path.Combine(path, file.Path.Replace("/", "\\"));
                var fileInfo = new FileInfo(fullFilePath);
                matchList.Add(fileInfo.FullName);
            }
            return matchList;

        }

        public void SaveFile(string filename, byte[] content, bool overwrite)
        {
            var fileInfo = new FileInfo(filename);
            if (!fileInfo.Directory.Exists) fileInfo.Directory.Create();

            if (File.Exists(filename) && !overwrite)
            {
                Console.WriteLine($"File already exists: {filename}");
                return;
            }

            using (var writer = new BinaryWriter((Stream)new FileStream(filename, FileMode.Create)))
            {
                writer.Write(content, 0, content.Length);
                writer.Close();
            }

            Console.WriteLine($"Downloaded: {filename}");
        }
    }
}
