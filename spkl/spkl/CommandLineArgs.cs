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
workflow = Deploy Workflows
webresources = Deploy webresources
instrument = Download plugin/workflows and add code attributes to existing classes
get-webresources = Download webresources and match to the local files to create a spkl.json file for deployment
download-webresources = Download webresources and add missing local files to the spkl.json file for deployment
earlybound = Create earlybound types for C# development
unpack = Download and unpack a solution from Dynamics 365 into xml files & folders
unpacksolution = Download and unpack a Dynamic 365 solution zip from the filesystem into xml files & folders
pack = Packs an unpacked solution into a new Solution Zip - grabbing the latest assemblies and webresources from their mapped locations
import = Packs a solution as per the 'pack' task, and then imports into Dynamics 365
")]

        public string Task { get; set; }

        [CommandLineParameter(Name = "Overwrite", Command = "o", Required = false, Description = "Optional overwrite webresource files when downloading webresources")]
        public bool Overwrite { get; set; }

        [CommandLineParameter(Name = "path", ParameterIndex = 2, Required = false, Description = "Optional path to spkl.json file")]
        public string Path { get; set; }

        [CommandLineParameter(Name = "connection", ParameterIndex = 3, Required = false, Description = "Optional Connection String")]
        public string Connection { get; set; }

        [CommandLineParameter(Name = "profile", Command = "p", ParameterIndex = 4, Required = false, Description = "Optional profile name. If ommitted, default will be used")]
        public string Profile { get; set; }

        [CommandLineParameter(Name = "solution prefix", Command= "s",  Required = false, Description = "Optional Prefix to filter webresources when downloading the config.")]
        public string Prefix { get; set; }

        [CommandLineParameter(Name = "Wait for keypress", Command = "w", Required = false, Description = "Optional wait for a key press at the end of task run")]
        public bool WaitForKey { get; set; }

        [CommandLineParameter(Name = "Ignore Windows login", Command = "i", Required = false, Description = "Optional flag to ignore logged in windows credentials during discovery and always ask for username/password.")]
        public bool IgnoreLocalPrincipal { get; set; }

        [CommandLineParameter(Name = "Exclude Plugin Steps", Command = "e", Required = false, Description = "Exclude plugin steps when deploying plugins")]
        public bool ExcludePluginSteps { get; set; }
    }
}
