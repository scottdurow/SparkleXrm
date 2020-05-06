using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using SparkleXrm.Tasks.Config;
using System.IO;
using System.Diagnostics;
using DLaB.EarlyBoundGenerator;
using DLaB.Log;

namespace SparkleXrm.Tasks
{
    public class EarlyBoundClassGeneratorTask : BaseTask
    {
        public string ConectionString {get;set;}
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
            Logger.Instance.OnLog += DLaBOnLog;
            var crmSvcUtilPath = GetCrmSvcUtilPath();

            var earlyBoundTypeConfigs = config.GetEarlyBoundConfig(this.Profile);
            foreach (var earlyboundConfig in earlyBoundTypeConfigs)
            {
                var ebgConfig = DLaB.EarlyBoundGenerator.Settings.EarlyBoundGeneratorConfig.GetDefault();
                ebgConfig.ExtensionConfig.SetPopulatedValues(earlyboundConfig);
                ebgConfig.ExtensionConfig.EntitiesWhitelist = ConvertCommasToPipes(earlyboundConfig.entities);
                ebgConfig.ExtensionConfig.ActionsWhitelist = ConvertCommasToPipes(earlyboundConfig.actions);

                // Not sure if this should map or not.  
                ebgConfig.ExtensionConfig.GenerateEnumProperties = earlyboundConfig.generateOptionsetEnums;

                ebgConfig.EntityOutPath = GetFile(config.filePath, earlyboundConfig.filename, "entities.cs", earlyboundConfig.oneTypePerFile);
                ebgConfig.ActionOutPath = GetFile(config.filePath, earlyboundConfig.actionFilename, "actions.cs", earlyboundConfig.oneTypePerFile);
                ebgConfig.OptionSetOutPath = GetFile(config.filePath, earlyboundConfig.optionSetFilename, "optionsets.cs", earlyboundConfig.oneTypePerFile);
                ebgConfig.SupportsActions = earlyboundConfig.generateActions != false && !string.IsNullOrWhiteSpace(earlyboundConfig.actions);
                ebgConfig.Namespace = earlyboundConfig.classNamespace ?? "Xrm";
                ebgConfig.ConnectionString = ConectionString;
                ebgConfig.CrmSvcUtilRelativePath = crmSvcUtilPath;
                ebgConfig.RootPath = Path.GetDirectoryName(crmSvcUtilPath);
                ebgConfig.ServiceContextName = string.IsNullOrWhiteSpace(earlyboundConfig.serviceContextName) 
                    ? ebgConfig.ServiceContextName
                    : earlyboundConfig.serviceContextName;
                if (earlyboundConfig.oneTypePerFile)
                {
                    ebgConfig.ExtensionConfig.CreateOneFilePerAction = true;
                    ebgConfig.ExtensionConfig.CreateOneFilePerEntity = true;
                    ebgConfig.ExtensionConfig.CreateOneFilePerOptionSet = true;
                }


                if (string.IsNullOrEmpty(ebgConfig.ConnectionString))
                {
                    throw new Exception("ConnectionString must be supplied for CrmSvcUtil");
                }

                var logic = new Logic(ebgConfig);
                if (!earlyboundConfig.generateOptionsetEnums)
                {
                    if (ebgConfig.SupportsActions)
                    {
                        logic.CreateActions();
                    }
                    logic.CreateEntities();
                }
                else
                {
                    new Logic(ebgConfig).ExecuteAll();
                }
            }
        }

        private void DLaBOnLog(LogMessageInfo info)
        {
            _trace.WriteLine(info.ModalMessage);
            if (info.Detail != null && info.Detail != info.ModalMessage)
            {
                _trace.WriteLine(info.Detail);
            }
        }

        private string GetFile(string path, string configured, string fileTypeName, bool createOneFilePerType)
        {
            path = Path.Combine(path, string.IsNullOrWhiteSpace(configured) 
                ? fileTypeName 
                : configured);
            if (createOneFilePerType && Path.GetExtension(path).ToLower() == ".cs")
            {
                path = Path.GetDirectoryName(path) + "\\" + Path.GetFileNameWithoutExtension(path) + "\\";
            }
            return path;
        }


        private string ConvertCommasToPipes(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }
            return value.Replace(',', '|');
        }

        //private void RunProcess(string crmSvcUtilPath, string parameters, string crmSvcUtilFolder)
        //{
        //    var procStart = new ProcessStartInfo(crmSvcUtilPath, parameters)
        //    {
        //        WorkingDirectory = crmSvcUtilFolder,
        //        RedirectStandardOutput = true,
        //        RedirectStandardError = true,
        //        CreateNoWindow = true,
        //        UseShellExecute = false,
        //        WindowStyle = ProcessWindowStyle.Hidden
        //    };
        //
        //    _trace.WriteLine("Running {0} {1}", crmSvcUtilPath, parameters);
        //    var exitCode = 0;
        //    Process proc = null;
        //    try
        //    {
        //        proc = Process.Start(procStart);
        //        proc.OutputDataReceived += Proc_OutputDataReceived;
        //        proc.ErrorDataReceived += Proc_OutputDataReceived;
        //        proc.BeginOutputReadLine();
        //        proc.BeginErrorReadLine();
        //        proc.WaitForExit(20 * 60 * 60 * 1000);
        //        proc.CancelOutputRead();
        //        proc.CancelErrorRead();
        //    }
        //    finally
        //    {
        //        exitCode = proc.ExitCode;
        //
        //        proc.Close();
        //    }
        //
        //    if (exitCode != 0)
        //    {
        //        throw new SparkleTaskException(SparkleTaskException.ExceptionTypes.CRMSVCUTIL_ERROR, $"CrmSvcUtil exited with error {exitCode}");
        //    }
        //}

        private string GetCrmSvcUtilPath()
        {
            // locate the CrmSvcUtil package folder
            var targetFolder = ServiceLocator.DirectoryService.GetApplicationDirectory();

            // If we are running in VS, then move up past bin/Debug
            if (targetFolder.Contains(@"bin\Debug") || targetFolder.Contains(@"bin\Release"))
            {
                targetFolder += @"\..";
            }

            // move from spkl.v.v.v.\tools - back to packages folder
            var path = Path.GetFullPath(targetFolder + @"\..\..");
            _trace.WriteLine("Target {0}", path);
            var dLaBCrmSvcUtilExtPath = ServiceLocator.DirectoryService.SimpleSearch(path, "DLaB.CrmSvcUtilExtensions.dll");
            if (string.IsNullOrEmpty(dLaBCrmSvcUtilExtPath))
            {
                throw new SparkleTaskException(SparkleTaskException.ExceptionTypes.UTILSNOTFOUND,
                    $"Cannot locate DLaB.CrmSvcUtilExtensions.dll anywhere within '{path}' - run Install-Package DLaB.EarlyBoundGenerator.Api");
            }

            var crmSvcUtilFolder = new FileInfo(dLaBCrmSvcUtilExtPath).DirectoryName;
            var crmSvcUtilPath = ServiceLocator.DirectoryService.SimpleSearch(crmSvcUtilFolder, "CrmSvcUtil.exe");
            if (string.IsNullOrEmpty(crmSvcUtilPath))
            {
                throw new SparkleTaskException(SparkleTaskException.ExceptionTypes.UTILSNOTFOUND,
                    $"Cannot locate CrmSvcUtil.exe at '{crmSvcUtilFolder}' - run Install-Package DLaB.EarlyBoundGenerator.Api");
            }

            return crmSvcUtilPath;
        }

        //private void Proc_OutputDataReceived(object sender, DataReceivedEventArgs e)
        //{
        //    _trace.WriteLine(e.Data);
        //}
    }
}
