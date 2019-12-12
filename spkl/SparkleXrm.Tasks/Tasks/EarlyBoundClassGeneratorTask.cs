using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using SparkleXrm.Tasks.Config;
using System.IO;
using System.Diagnostics;
using SparkleXrm.Tasks.CrmSvcUtil;

namespace SparkleXrm.Tasks
{
    public class EarlyBoundClassGeneratorTask : BaseTask
    {
        public string ConectionString {get;set;}
        private string _folder;
        public EarlyBoundClassGeneratorTask(IOrganizationService service, ITrace trace) : base(service, trace)
        {
        }

        public EarlyBoundClassGeneratorTask(OrganizationServiceContext ctx, ITrace trace) : base(ctx, trace)
        {
        }

        protected override void ExecuteInternal(string folder, OrganizationServiceContext ctx)
        {
           
            _trace.WriteLine("Searching for plugin config in '{0}'", folder);
            var configs = ServiceLocator.ConfigFileFactory.FindConfig(folder);

            foreach (var config in configs)
            {
                _trace.WriteLine("Using Config '{0}'", config.filePath);
               
                CreateEarlyBoundTypes(ctx, config);
            }
            _trace.WriteLine("Processed {0} config(s)", configs.Count);
  
        }

        public void CreateEarlyBoundTypes(OrganizationServiceContext ctx, ConfigFile config)
        {
            _folder = config.filePath;

            // locate the CrmSvcUtil package folder
            var targetfolder = ServiceLocator.DirectoryService.GetApplicationDirectory();
            
            // If we are running in VS, then move up past bin/Debug
            if (targetfolder.Contains(@"bin\Debug") || targetfolder.Contains(@"bin\Release"))
            {
                targetfolder += @"\..";
            }

            // move from spkl.v.v.v.\tools - back to packages folder
            var crmsvcutilPath = ServiceLocator.DirectoryService.SimpleSearch(targetfolder + @"\..\..", "crmsvcutil.exe");
            _trace.WriteLine("Target {0}", targetfolder);
            var crmsvcutilFolder = new FileInfo(crmsvcutilPath).DirectoryName;
            if (string.IsNullOrEmpty(crmsvcutilPath))
            {
                throw new SparkleTaskException(SparkleTaskException.ExceptionTypes.UTILSNOTFOUND,
                    $"Cannot locate CrmSvcUtil at '{crmsvcutilPath}' - run Install-Package Microsoft.CrmSdk.CoreTools");
            }

            // Copy the filtering assembly
            var filteringAssemblyPathString = ServiceLocator.DirectoryService.SimpleSearch(targetfolder + @"\..\..", "spkl.CrmSvcUtilExtensions.dll");
         
            if (string.IsNullOrEmpty(filteringAssemblyPathString))
            {
                throw new SparkleTaskException(SparkleTaskException.ExceptionTypes.UTILSNOTFOUND,
                    $"Cannot locate spkl.CrmSvcUtilExtensions.dll at '{crmsvcutilPath}' ");
            }
            var filteringAssemblyPath = new FileInfo(filteringAssemblyPathString);
            var targetFilteringPath = Path.Combine(crmsvcutilFolder, "spkl.CrmSvcUtilExtensions.dll");

            if (filteringAssemblyPath.FullName != targetFilteringPath)
            {
                File.Copy(filteringAssemblyPath.FullName, targetFilteringPath, true);
            }

            var earlyBoundTypeConfigs = config.GetEarlyBoundConfig(this.Profile);
            foreach (var earlyboundconfig in earlyBoundTypeConfigs)
            {
                // Create config and copy to the CrmSvcUtil folder
                var configXml = $@"<?xml version=""1.0"" encoding=""utf-8"" ?>
                        <configuration>
                            <entities>{earlyboundconfig.entities}</entities>
                            <messages>{earlyboundconfig.actions}</messages>
                            <picklistEnums>{
                        earlyboundconfig.generateOptionsetEnums.ToString().ToLower()
                    }</picklistEnums>
                            <stateEnums>{earlyboundconfig.generateStateEnums.ToString().ToLower()}</stateEnums>
                            <globalEnums>{earlyboundconfig.generateGlobalOptionsets.ToString().ToLower()}</globalEnums>
                        </configuration>";

                // Copy the filtering assembly to the CrmSvcUtil folder
                File.WriteAllText(Path.Combine(crmsvcutilFolder, "spkl.crmsvcutil.config"), configXml);

                if (string.IsNullOrEmpty(this.ConectionString))
                    throw new Exception("ConnectionString must be supplied for CrmSvcUtil");

                // Run CrmSvcUtil 
                var parameters =
                    $@"/connstr:""{this.ConectionString}"" /out:""{
                        Path.Combine(this._folder, earlyboundconfig.filename)
                    }"" /namespace:""{earlyboundconfig.classNamespace}"" /serviceContextName:""{
                        earlyboundconfig.serviceContextName
                    }"" /GenerateActions:""{
                        !String.IsNullOrEmpty(earlyboundconfig.actions)
                        }"" /namingservice:""spkl.CrmSvcUtilExtensions.NamingService,spkl.CrmSvcUtilExtensions"" /codewriterfilter:""spkl.CrmSvcUtilExtensions.FilteringService,spkl.CrmSvcUtilExtensions"" /codewritermessagefilter:""spkl.CrmSvcUtilExtensions.MessageFilteringService,spkl.CrmSvcUtilExtensions"" /codegenerationservice:""spkl.CrmSvcUtilExtensions.CodeGenerationService, spkl.CrmSvcUtilExtensions"" /metadataproviderqueryservice:""spkl.CrmSvcUtilExtensions.MetadataProviderQueryService,spkl.CrmSvcUtilExtensions""";

                var procStart = new ProcessStartInfo(crmsvcutilPath, parameters)
                {
                    WorkingDirectory = crmsvcutilFolder,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    WindowStyle = ProcessWindowStyle.Hidden
                };

                _trace.WriteLine("Running {0} {1}", crmsvcutilPath, parameters);
                var exitCode = 0;
                Process proc = null;
                try
                {
                    proc = Process.Start(procStart);
                    proc.OutputDataReceived += Proc_OutputDataReceived;
                    proc.ErrorDataReceived += Proc_OutputDataReceived;
                    proc.BeginOutputReadLine();
                    proc.BeginErrorReadLine();
                    proc.WaitForExit(20 * 60 * 60 * 1000);
                    proc.CancelOutputRead();
                    proc.CancelErrorRead();
                }
                finally
                {
                    exitCode = proc.ExitCode;
                   
                    proc.Close();
                }

                if (exitCode!=0)
                {
                    throw new SparkleTaskException(SparkleTaskException.ExceptionTypes.CRMSVCUTIL_ERROR, $"CrmSvcUtil exited with error {exitCode}");
                }

                // Now that crmsvcutil has created the earlybound class file let's split it into separate files if this what the user wants
                if (earlyboundconfig.oneTypePerFile)
                {
                    _trace.WriteLine("oneTypePerFile=true : Splitting types into separate files...");
                    SplitCrmSvcUtilOutputFileIntoOneFilePerType(
                        Path.Combine(_folder, earlyboundconfig.filename),
                        Path.Combine(_folder, Path.GetDirectoryName(earlyboundconfig.filename)),
                        earlyboundconfig.classNamespace);
                }
            }
        }

        private void Proc_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            _trace.WriteLine(e.Data);
        }

        private void SplitCrmSvcUtilOutputFileIntoOneFilePerType(string earlyboundconfigFilename, string destinationDirectoryPath, string typeNamespace)
        {
            var sourceCode = File.ReadAllText(earlyboundconfigFilename);

            var sourceCodeManipulator = new SourceCodeSplitter(_trace);
            sourceCodeManipulator.WriteToSeparateFiles(destinationDirectoryPath, sourceCode, typeNamespace);
        }
    }
}
