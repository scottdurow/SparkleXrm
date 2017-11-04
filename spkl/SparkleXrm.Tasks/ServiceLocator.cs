using SparkleXrm.Tasks.Config;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparkleXrm.Tasks
{
    public class ServiceLocator
    {
        public static ServiceContainer ServiceProvider;

        static ServiceLocator()
        {
            Init();
        }
        public static void Reset()
        {
            ServiceProvider = new ServiceContainer();
        }
        public static void Init()
        {
            // Add standard services
            Reset();
            ServiceProvider.AddService(typeof(IConfigFileService), new ConfigFileService());
            ServiceProvider.AddService(typeof(IDirectoryService), new DirectoryService());
            ServiceProvider.AddService(typeof(IQueries), new Queries());
        }

        public static IConfigFileService ConfigFileFactory
        {
            get
            {
                return (IConfigFileService)ServiceProvider.GetService(typeof(IConfigFileService));
            }
        }

        public static IQueries Queries
        {
            get
            {
                return (IQueries)ServiceProvider.GetService(typeof(IQueries));
            }
        }
        public static IDirectoryService DirectoryService
        {
            get
            {
                return (IDirectoryService)ServiceProvider.GetService(typeof(IDirectoryService));
            }
        }
        
    }
}
