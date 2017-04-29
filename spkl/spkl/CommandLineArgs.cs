using CmdLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparkleXrmTask
{
    [CommandLineArguments(Program = "spkl", Title = "SpakleXrm Deployment Tasks", Description = "Let SparkleXrm do the work for you!")]
    public class CommandLineArgs
    {
        [CommandLineParameter(Command = "?", Default = false, Description = "Show Help", Name = "Help", IsHelp = true)]
        public bool Help { get; set; }

        [CommandLineParameter(Name = "task", ParameterIndex = 1, Required = true, Description = @"
plugins = Deploy Plugins
workflow = Deploy Worflows
webresources = Deploy webresources
instrument = Download plugin/workflows and add code attributes to existing classes
get-webresources = Download webresources and match to the local files to create a spkl.json file for deployment")]

        public string Task { get; set; }

        [CommandLineParameter(Name = "path", ParameterIndex = 2, Required = false, Description = "Optional path to spkl.json file")]
        public string Path { get; set; }

        [CommandLineParameter(Name = "connection", ParameterIndex = 3, Required = false, Description = "Optional Connection String")]
        public string Connection { get; set; }

        [CommandLineParameter(Name = "profile", Command = "p", ParameterIndex = 4, Required = false, Description = "Optional profile name. If ommitted, default will be used")]
        public string Profile { get; set; }

        [CommandLineParameter(Name = "solution prefix", Command= "s",  Required = false, Description = "Optional Prefix to filter webresources when downloading the config.")]
        public string Prefix { get; set; }
    }
}
