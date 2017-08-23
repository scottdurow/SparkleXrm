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
            var configs = ConfigFile.FindConfig(folder);

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
            var targetfolder = DirectoryEx.GetApplicationDirectory();
            
            // If we are running in VS, then move up past bin/Debug
            if (targetfolder.Contains(@"bin\Debug") || targetfolder.Contains(@"bin\Release"))
            {
                targetfolder += @"\..";
            }

            // move from spkl.v.v.v.\tools - back to packages folder
            var crmsvcutilPath = DirectoryEx.Search(targetfolder + @"\..\..", "crmsvcutil.exe");
            _trace.WriteLine("Target {0}", targetfolder);
            var crmsvcutilFolder = new FileInfo(crmsvcutilPath).DirectoryName;
            if (string.IsNullOrEmpty(crmsvcutilPath))
            {
                throw new SparkleTaskException(SparkleTaskException.ExceptionTypes.UTILSNOTFOUND, String.Format("Cannot locate CrmSvcUtil at '{0}' - run Install-Package Microsoft.CrmSdk.CoreTools", crmsvcutilPath));
            }

            // Copy the filtering assembly
            var filteringAssemblyPathString = DirectoryEx.Search(targetfolder + @"\..\..", "spkl.CrmSvcUtilExtensions.dll");
         
            if (string.IsNullOrEmpty(filteringAssemblyPathString))
            {
                throw new SparkleTaskException(SparkleTaskException.ExceptionTypes.UTILSNOTFOUND, String.Format("Cannot locate spkl.CrmSvcUtilExtensions.dll at '{0}' ", crmsvcutilPath));
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
                string configXml = String.Format(@"<?xml version=""1.0"" encoding=""utf-8"" ?>
                        <configuration>
                            <entities>{0}</entities>
                            <messages>{1}</messages>
                            <picklistEnums>{2}</picklistEnums>
                            <stateEnums>{3}</stateEnums>
                            <globalEnums>{4}</globalEnums>
                        </configuration>", earlyboundconfig.entities,
                        earlyboundconfig.actions,
                        earlyboundconfig.generateOptionsetEnums.ToString().ToLower(),
                        earlyboundconfig.generateStateEnums.ToString().ToLower(),
                        earlyboundconfig.generateGlobalOptionsets.ToString().ToLower()
                        );

                // Copy the filtering assembly to the CrmSvcUtil folder
                File.WriteAllText(Path.Combine(crmsvcutilFolder, "spkl.crmsvcutil.config"), configXml);

                if (string.IsNullOrEmpty(this.ConectionString))
                    throw new Exception("ConnectionString must be supplied for CrmSvcUtil");

                // Run CrmSvcUtil 
                string parameters = String.Format(@"/connstr:""{0}"" /out:""{1}"" /namespace:""{2}"" /serviceContextName:""{3}"" /GenerateActions:""{4}"" /codewriterfilter:""spkl.CrmSvcUtilExtensions.FilteringService,spkl.CrmSvcUtilExtensions"" /codewritermessagefilter:""spkl.CrmSvcUtilExtensions.MessageFilteringService,spkl.CrmSvcUtilExtensions"" /codegenerationservice:""spkl.CrmSvcUtilExtensions.CodeGenerationService, spkl.CrmSvcUtilExtensions"" /metadataproviderqueryservice:""spkl.CrmSvcUtilExtensions.MetadataProviderQueryService,spkl.CrmSvcUtilExtensions""",
                    this.ConectionString,
                    Path.Combine(this._folder, earlyboundconfig.filename),
                    earlyboundconfig.classNamespace,
                    earlyboundconfig.serviceContextName,
                    !String.IsNullOrEmpty(earlyboundconfig.actions));

                ProcessStartInfo procStart = new ProcessStartInfo(crmsvcutilPath, parameters)
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
                catch 
                {
                    throw;
                }

                finally
                {
                    exitCode = proc.ExitCode;
                   
                    proc.Close();
                }

                if (exitCode!=0)
                {
                    throw new SparkleTaskException(SparkleTaskException.ExceptionTypes.CRMSVCUTIL_ERROR, String.Format("CrmSvcUtil exited with error {0}", exitCode));
                }
               
            }
        }

        private void Proc_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            _trace.WriteLine(e.Data);
        }
    }
}
