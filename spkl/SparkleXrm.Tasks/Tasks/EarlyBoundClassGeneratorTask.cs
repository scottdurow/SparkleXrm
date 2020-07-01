﻿using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using SparkleXrm.Tasks.Config;
using System.IO;
using System.Diagnostics;
using DLaB.EarlyBoundGenerator;
using DLaB.Log;
using SparkleXrm.Tasks.CrmSvcUtil;
using System.Linq;

namespace SparkleXrm.Tasks
{
    public class EarlyBoundClassGeneratorTask : BaseTask
    {
        public string ConnectionString { get; set; }
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
            var earlyBoundTypeConfigs = config.GetEarlyBoundConfig(Profile);
            foreach (var earlyboundConfig in earlyBoundTypeConfigs)
            {
                if (earlyboundConfig.useEarlyBoundGeneratorApi)
                {
                    _trace.WriteLine("Using DLaB.EarlyBoundGenerator.Api");
                    ProcessEbgGeneration(config, earlyboundConfig);
                }
                else
                {
                    CreateEarlyBoundTypesSpkl(ctx, config, earlyboundConfig);
                }
            }
        }

        #region DLaB.EarlyBoundGenerator.Api

        private void ProcessEbgGeneration(ConfigFile config, EarlyBoundTypeConfig earlyboundConfig)
        {
            Logger.Instance.OnLog += DLaBOnLog;
            var crmSvcUtilPath = GetCrmSvcUtilPathDLaB();
            var ebgConfig = DLaB.EarlyBoundGenerator.Settings.EarlyBoundGeneratorConfig.GetDefault();
            ebgConfig.MaskPassword = ConnectionString != null && ConnectionString.ToLower().Contains("password");
            ebgConfig.ExtensionConfig.SetPopulatedValues(earlyboundConfig);
            ebgConfig.ExtensionConfig.EntitiesWhitelist = ConvertCommasToPipes(earlyboundConfig.entities);
            ebgConfig.ExtensionConfig.ActionsWhitelist = ConvertCommasToPipes(earlyboundConfig.actions);
            ebgConfig.ExtensionConfig.GenerateEnumProperties = earlyboundConfig.generateOptionsetEnums;
            ebgConfig.EntityOutPath = GetFile(config.filePath, earlyboundConfig.filename, "entities.cs", earlyboundConfig.oneTypePerFile);
            ebgConfig.ActionOutPath = GetFile(config.filePath, earlyboundConfig.actionFilename, "actions.cs", earlyboundConfig.oneTypePerFile);
            ebgConfig.OptionSetOutPath = GetFile(config.filePath, earlyboundConfig.optionSetFilename, "optionsets.cs", earlyboundConfig.oneTypePerFile);
            ebgConfig.SupportsActions = earlyboundConfig.generateActions != false && !string.IsNullOrWhiteSpace(earlyboundConfig.actions);
            ebgConfig.Namespace = earlyboundConfig.classNamespace ?? "Xrm";
            ebgConfig.ConnectionString = ConnectionString;
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

        private string GetCrmSvcUtilPathDLaB()
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

        #endregion  // Early Bound Generator

        #region spkl Early Bound Generator 

        public void CreateEarlyBoundTypesSpkl(OrganizationServiceContext ctx, ConfigFile config, EarlyBoundTypeConfig earlyboundconfig)
        {
            var folder = config.filePath;

            // locate the CrmSvcUtil package folder
            var targetfolder = ServiceLocator.DirectoryService.GetApplicationDirectory();

            // If we are running in VS, then move up past bin/Debug
            if (targetfolder.Contains(@"bin\Debug") || targetfolder.Contains(@"bin\Release"))
            {
                targetfolder += @"\..";
            }
            _trace.WriteLine("Target {0}", targetfolder);

            // move from spkl.v.v.v.\tools - back to packages folder
            var crmsvcutilPaths = ServiceLocator.DirectoryService.Search(targetfolder + @"\..\..", @"crmsvcutil.exe");

            // Get the latest version of coretools from the coretools folder
            var crmsvcutilPath = (from f in crmsvcutilPaths
                                  where f.ToLower().EndsWith(@"content\bin\coretools\crmsvcutil.exe")
                                  orderby f descending
                                  select f).FirstOrDefault();


            if (string.IsNullOrEmpty(crmsvcutilPath))
            {
                throw new SparkleTaskException(SparkleTaskException.ExceptionTypes.UTILSNOTFOUND,
                    $"Cannot locate CrmSvcUtil at '{crmsvcutilPath}' - run Install-Package Microsoft.CrmSdk.CoreTools");
            }
            var crmsvcutilFolder = new FileInfo(crmsvcutilPath).DirectoryName;
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

            if (string.IsNullOrEmpty(earlyboundconfig.entities) && earlyboundconfig.entityCollection?.Length > 0)
            {
                earlyboundconfig.entities = string.Join(",", earlyboundconfig.entityCollection);
            }

            if (string.IsNullOrEmpty(earlyboundconfig.actions) && earlyboundconfig.actionCollection?.Length > 0)
            {
                earlyboundconfig.actions = string.Join(",", earlyboundconfig.actionCollection);
            }

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

            if (string.IsNullOrEmpty(this.ConnectionString))
                throw new Exception("ConnectionString must be supplied for CrmSvcUtil");

            // Run CrmSvcUtil 
            var parameters =
                $@"/connstr:""{this.ConnectionString}"" /out:""{
                    Path.Combine(folder, earlyboundconfig.filename)
                }"" /namespace:""{earlyboundconfig.classNamespace}"" /serviceContextName:""{
                    earlyboundconfig.serviceContextName
                }"" /GenerateActions:""{
                    !String.IsNullOrEmpty(earlyboundconfig.actions)
                }"" /codewriterfilter:""spkl.CrmSvcUtilExtensions.FilteringService,spkl.CrmSvcUtilExtensions"" /codewritermessagefilter:""spkl.CrmSvcUtilExtensions.MessageFilteringService,spkl.CrmSvcUtilExtensions"" /codegenerationservice:""spkl.CrmSvcUtilExtensions.CodeGenerationService, spkl.CrmSvcUtilExtensions"" /metadataproviderqueryservice:""spkl.CrmSvcUtilExtensions.MetadataProviderQueryService,spkl.CrmSvcUtilExtensions""";

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

            if (exitCode != 0)
            {
                throw new SparkleTaskException(SparkleTaskException.ExceptionTypes.CRMSVCUTIL_ERROR, $"CrmSvcUtil exited with error {exitCode}");
            }

            // Now that crmsvcutil has created the earlybound class file let's split it into separate files if this what the user wants
            if (earlyboundconfig.oneTypePerFile)
            {
                _trace.WriteLine("oneTypePerFile=true : Splitting types into separate files...");
                SplitCrmSvcUtilOutputFileIntoOneFilePerType(
                    Path.Combine(folder, earlyboundconfig.filename),
                    Path.Combine(folder, Path.GetDirectoryName(earlyboundconfig.filename)),
                    earlyboundconfig.classNamespace);
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

            // Remove the original single code file generated by crmsvcutil	
            File.Delete(earlyboundconfigFilename);
        }

        #endregion
    }
}
