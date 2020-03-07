using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using SparkleXrm.Tasks.Config;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SparkleXrm.Tasks
{
    public class SolutionPackagerTask : BaseTask
    {
        public string ConectionString { get; set; }
        private string _folder;
        public string command;
        public SolutionPackagerTask(IOrganizationService service, ITrace trace) : base(service, trace)
        {
        }

        public SolutionPackagerTask(OrganizationServiceContext ctx, ITrace trace) : base(ctx, trace)
        {
        }

        protected override void ExecuteInternal(string folder, OrganizationServiceContext ctx)
        {
            
            _trace.WriteLine("Searching for packager config in '{0}'", folder);
            var configs = ServiceLocator.ConfigFileFactory.FindConfig(folder);

            foreach (var config in configs)
            {
                _trace.WriteLine("Using Config '{0}'", config.filePath);
                _folder = config.filePath;
                switch (command)
                {
                    case "unpack":
                        UnPack(ctx, config);
                        break;
                    case "unpacksolution":
                        UnPackFromSolutionZip(config);
                        break;
                    case "pack":
                        Pack(ctx, config, false);
                        break;
                    case "import":
                        var solutionZipTempPath = Path.GetTempFileName();
                        Pack(ctx, config, true);
                        break; 
                }
                
            }
            _trace.WriteLine("Processed {0} config(s)", configs.Count);


        }

        public void UnPack(OrganizationServiceContext ctx, ConfigFile config)
        {
            var configs = config.GetSolutionConfig(this.Profile);
            foreach (var solutionPackagerConfig in configs)
            {
                // check solution exists
                var solution = GetSolution(solutionPackagerConfig.solution_uniquename);
                var movetoFolder = Path.Combine(config.filePath, solutionPackagerConfig.packagepath);
                var unpackPath = UnPackSolution(solutionPackagerConfig, movetoFolder);

            }
        }

        public void UnPackFromSolutionZip(ConfigFile config)
        {
            var configs = config.GetSolutionConfig(this.Profile);
            foreach (var solutionPackagerConfig in configs)
            {
                var solutionZip = Path.Combine(config.filePath, solutionPackagerConfig.solutionpath);
                var movetoFolder = Path.Combine(config.filePath, solutionPackagerConfig.packagepath);
                UnpackSolutionZip(solutionPackagerConfig, movetoFolder, solutionZip);
            }
        }

        public void Pack(OrganizationServiceContext ctx, ConfigFile config, bool import)
        {
            var configs = config.GetSolutionConfig(this.Profile);
            foreach (var solutionPackagerConfig in configs)
            {
                var packageFolder = Path.Combine(config.filePath, solutionPackagerConfig.packagepath);
                var solutionZipPath = "solution.zip";

                var version = GetSolutionVersion(packageFolder);

                // Create the solution zip in the root or the location specified in the spkl.json
                if (solutionPackagerConfig.solutionpath != null)
                {
                    solutionZipPath = String.Format(solutionPackagerConfig.solutionpath, 
                        version.Major, 
                        version.Minor>-1 ? version.Minor : 0, 
                        version.Build > -1 ? version.Build : 0,
                        version.Revision > -1 ? version.Revision : 0);
                }

                solutionZipPath = Path.Combine(config.filePath, solutionZipPath);
                var solutionLocation = PackSolution(config.filePath, solutionPackagerConfig, solutionZipPath);

                _trace.WriteLine("Solution Packed to '{0}'", solutionLocation);
               
                if (import)
                { 
                    // Import solution into Dynamics
                    ImportSolution(solutionLocation);
                }
            }
        }
        private void Diff(OrganizationServiceContext ctx, ConfigFile config)
        {
            var configs = config.GetSolutionConfig(this.Profile);
            foreach (var solutionPackagerConfig in configs)
            {
                var movetoFolder = Path.Combine(config.filePath, solutionPackagerConfig.packagepath);
                var unpackPath = GetRandomFolder();
                unpackPath = UnPackSolution(solutionPackagerConfig, unpackPath);

                // Delete existing content 
                Directory.Delete(movetoFolder, true);

                // Copy to the package path
                DirectoryCopy(unpackPath, movetoFolder, true);
            }
        }

        public Solution GetSolution(string uniqueName)
        {
            //Check whether it already exists
            var queryCheckForSampleSolution = new QueryExpression
            {
                EntityName = SparkleXrm.Tasks.Solution.EntityLogicalName,
                ColumnSet = new ColumnSet("uniquename","version"),
                Criteria = new FilterExpression()
            };
            queryCheckForSampleSolution.Criteria.AddCondition("uniquename", ConditionOperator.Equal, uniqueName);

            //Create the solution if it does not already exist.
            var querySampleSolutionResults = this._service.RetrieveMultiple(queryCheckForSampleSolution);

            if (querySampleSolutionResults.Entities.Count == 0)
                throw new Exception(String.Format("Solution unique name '{0}' does not exist", uniqueName));

            return querySampleSolutionResults.Entities[0].ToEntity<Solution>();
        }

        
        private void ExportManagedSolution(SolutionPackageConfig config, string filePath)
        {
            var request = new ExportSolutionRequest
            {
                SolutionName = config.solution_uniquename,
                ExportAutoNumberingSettings = false,
                ExportCalendarSettings = false,
                ExportCustomizationSettings = false,
                ExportEmailTrackingSettings = false,
                ExportExternalApplications = false,
                ExportGeneralSettings = false,
                ExportIsvConfig = false,
                ExportMarketingSettings = false,
                ExportOutlookSynchronizationSettings = false,
                ExportRelationshipRoles = false,
                ExportSales = false,
                Managed = true
            };

            var response = (ExportSolutionResponse)_service.Execute(request);

            // Save solution 
            using (var fs = File.Create(filePath))
            {
                fs.Write(response.ExportSolutionFile, 0, response.ExportSolutionFile.Length);
            }            
        }

        private void ExportUnmanagedSolution(SolutionPackageConfig config, string filePath)
        {   
            var request = new ExportSolutionRequest
            {
                SolutionName = config.solution_uniquename,
                ExportAutoNumberingSettings = false,
                ExportCalendarSettings = false,
                ExportCustomizationSettings = false,
                ExportEmailTrackingSettings = false,
                ExportExternalApplications = false,
                ExportGeneralSettings = false,
                ExportIsvConfig = false,
                ExportMarketingSettings = false,
                ExportOutlookSynchronizationSettings = false,
                ExportRelationshipRoles = false,
                ExportSales = false,
                Managed = false                    
            };

            var response = (ExportSolutionResponse)_service.Execute(request);

            // Save solution 
            using (var fs = File.Create(filePath))
            {
                fs.Write(response.ExportSolutionFile, 0, response.ExportSolutionFile.Length);
            }
                
        }

        private void ImportSolution(string solutionPath)
        {
            _trace.WriteLine("Importing solution '{0}'...", solutionPath);
            var solutionBytes = File.ReadAllBytes(solutionPath);

            var request = new ImportSolutionRequest();
            request.OverwriteUnmanagedCustomizations = true;
            request.PublishWorkflows = true;
            request.CustomizationFile = solutionBytes;
            request.ImportJobId = Guid.NewGuid();
            var asyncExecute = new ExecuteAsyncRequest()
            {
                Request = request

            };
            var response = (ExecuteAsyncResponse)_service.Execute(asyncExecute);

            var asyncoperationid = response.AsyncJobId;
            var importComplete = false;
            var importStartedOn = DateTime.Now;
            var importError = String.Empty;
            do
            {
                try
                {
                    if (DateTime.Now.Subtract(importStartedOn).Minutes > 15)
                    {
                        throw new SparkleTaskException(SparkleTaskException.ExceptionTypes.IMPORT_ERROR, "Timeout whilst uploading solution\nThe import process has timed out after 15 minutes.");
                    }

                    // Query the job until completed
                    var job = _service.Retrieve("asyncoperation", asyncoperationid, new ColumnSet(new System.String[] { "statuscode", "message","friendlymessage" }));

                    var statuscode = job.GetAttributeValue<OptionSetValue>("statuscode");

                    switch (statuscode.Value)
                    {
                        case 30:
                            importComplete = true;
                            importError = "";
                            break;
                        case 32: // Cancelled
                        case 31:
                            importComplete = true;
                            importError = job.GetAttributeValue<string>("message") + "\n" + job.GetAttributeValue<string>("friendlymessage");
                            break;
                    }
                    _trace.Write(".");
                }
                catch 
                {
                   // The import job can be locked or not yet created
                   // so don't do anything and just wait...
                }
                Thread.Sleep(new TimeSpan(0, 0, 2));
            }
            while (!importComplete);

            if (!string.IsNullOrEmpty(importError))
            {
                throw new SparkleTaskException(SparkleTaskException.ExceptionTypes.IMPORT_ERROR, importError);
            }
            _trace.WriteLine("\nSolution Import Completed. Now publishing....");
            // Publish
            var publishRequest = new PublishAllXmlRequest();
            var publishResponse = (PublishAllXmlResponse)_service.Execute(publishRequest);
            _trace.WriteLine("Solution Publish Completed");



        }
        private string GetRandomFolder()
        {
            // Get random folder
            var targetFolder = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            return targetFolder;
        }

        private string UnPackSolution(SolutionPackageConfig solutionPackagerConfig, string targetFolder)
        {
            // For the "both" option, SolutionPackager expects the managed and umanaged exports
            // to exist as zip files in the same file folder, and have the same name except that the
            // managed version will have the _managed suffix prior to the .zip extension.
            var tempFilePath = Path.GetTempFileName();
            var unmanagedSolutionZipPath = tempFilePath.Replace(".tmp",".zip"); 
            var managedSolutionZipPath = tempFilePath.Replace(".tmp","_managed.zip");            
            File.Delete(tempFilePath);

            switch (solutionPackagerConfig.packagetype)
            {
                case PackageType.managed:
                    ExportManagedSolution(solutionPackagerConfig, managedSolutionZipPath);
                    UnpackSolutionZip(solutionPackagerConfig, targetFolder, managedSolutionZipPath);
                    break;
                case PackageType.unmanaged:
                    ExportUnmanagedSolution(solutionPackagerConfig, unmanagedSolutionZipPath);
                    UnpackSolutionZip(solutionPackagerConfig, targetFolder, unmanagedSolutionZipPath);
                    break;
                default: //both-managed or both-unmanaged
                    ExportUnmanagedSolution(solutionPackagerConfig, unmanagedSolutionZipPath);
                    ExportManagedSolution(solutionPackagerConfig, managedSolutionZipPath);
                    UnpackSolutionZip(solutionPackagerConfig, targetFolder, unmanagedSolutionZipPath);
                    break;
            }

            

            return targetFolder;
        }

        private void UnpackSolutionZip(SolutionPackageConfig solutionPackagerConfig, string targetFolder, string solutionZipPath)
        {
            var binPath = GetPackagerFolder();
            var binFolder = new FileInfo(binPath).DirectoryName;

            // Create packager map.xml
            CreateMapFile(solutionPackagerConfig, Path.Combine(binFolder, "packager_map.xml"));

            // Run CrmSvcUtil 
            var parameters = String.Format(@"/action:Extract /zipfile:""{0}"" /folder:""{1}"" /packagetype:{2} /allowWrite:{3} /allowDelete:{4} /errorlevel:Verbose /nologo /log:packagerlog.txt /map:packager_map.xml{5}",
                solutionZipPath,
                targetFolder,
                (solutionPackagerConfig.packagetype == PackageType.both_unmanaged_import 
                  || solutionPackagerConfig.packagetype == PackageType.both_managed_import) ? "both" : solutionPackagerConfig.packagetype.ToString(),
                solutionPackagerConfig.allowwrite == false ? "No" : "Yes",
                solutionPackagerConfig.allowdelete == false ? "No" : "Yes",
                solutionPackagerConfig.clobber == false ? "" : " /clobber");

            RunPackager(binPath, binFolder, parameters);
        }


        private string PackSolution(string rootPath, SolutionPackageConfig solutionPackagerConfig, string solutionZipPath)
        {
            // Get location of source xml files
            var packageFolder = Path.Combine(rootPath, solutionPackagerConfig.packagepath);

            if (solutionPackagerConfig.increment_on_import)
            {
                var solution = GetSolution(solutionPackagerConfig.solution_uniquename);
                // Increment version in the package to upload
                // We increment the version in CRM already incase the solution package version is not correct
                IncrementVersion(solution.Version, packageFolder);
            }

            var binPath = GetPackagerFolder();
            var binFolder = new FileInfo(binPath).DirectoryName;

            // Create packager map.xml
            CreateMapFile(solutionPackagerConfig, Path.Combine(binFolder, "packager_map.xml"));

            // Run CrmSvcUtil 
            var parameters = String.Format(@"/action:Pack /zipfile:""{0}"" /folder:""{1}"" /packagetype:{2} /errorlevel:Verbose /nologo /log:packagerlog.txt /map:packager_map.xml",
                solutionZipPath,
                packageFolder,
                (solutionPackagerConfig.packagetype == PackageType.both_unmanaged_import
                  || solutionPackagerConfig.packagetype == PackageType.both_managed_import) ? "both" : solutionPackagerConfig.packagetype.ToString()
                );

            RunPackager(binPath, binFolder, parameters);

            // When package type is both_managed_import then SolutionPackager will create
            // two zip files. Need to pass back the name of the he managed version which
            // has "_managed" in the name right before the .zip extension.
            if(solutionPackagerConfig.packagetype == PackageType.both_managed_import)
            {
                return solutionZipPath.Replace(".zip", "_managed.zip");
            }

            return solutionZipPath;
        }

        private Version GetSolutionVersion(string packageFolder)
        {
            try
            {
                string solutionXmlPath = Path.Combine(packageFolder, @"Other\Solution.xml");

                Version version = null;

                using (var stream = new StreamReader(solutionXmlPath))
                {
                    var document = XDocument.Load(stream);
                    var versionNode = document.Root
                          .Descendants()
                          .Where(element => element.Name == "Version")
                          .FirstOrDefault();

                    if (versionNode != null)
                    {
                        version = new Version(versionNode.Value);

                    }
                }

                return version;
            }
            catch (Exception ex)
            {
                throw new Exception("Could not read solution version:" + ex.Message, ex);
            }
        }

        private void IncrementVersion(string currentVersion, string packageFolder)
        {  
            // Update the solution.xml
            string solutionXmlPath = Path.Combine(packageFolder, @"Other\Solution.xml");
            string newVersion = "";
            XDocument document;

            using (var stream = new StreamReader(solutionXmlPath))
            {
                document = XDocument.Load(stream);
            }

            var versionNode = document.Root
                        .Descendants()
                        .Where(element => element.Name == "Version")
                        .First();

            // Increment the last part of the build version
            var parts = currentVersion.Split('.');
            int buildVersion = 0;
            if (int.TryParse(parts[parts.Length - 1], out buildVersion))
            {
                buildVersion++;
                parts[parts.Length - 1] = buildVersion.ToString();
                newVersion = string.Join(".", parts);
                versionNode.Value = newVersion;
                document.Save(solutionXmlPath, SaveOptions.None);
            }
            else
            {
                throw new Exception(string.Format("Could not increment version '{0}'", currentVersion));
            }
            _trace.WriteLine("Incremented solution version from '{0}' to '{1}'", currentVersion, newVersion);
        }

        private string GetPackagerFolder()
        {
            // locate the CrmSvcUtil package folder
            var targetfolder = ServiceLocator.DirectoryService.GetApplicationDirectory();

            // If we are running in VS, then move up past bin/Debug
            if (targetfolder.Contains(@"bin\Debug") || targetfolder.Contains(@"bin\Release"))
            {
                targetfolder += @"\..";
            }

            // move from spkl.v.v.v.\tools - back to packages folder
            var binPath = ServiceLocator.DirectoryService.SimpleSearch(targetfolder + @"\..\..", "SolutionPackager.exe");
            _trace.WriteLine("Target {0}", targetfolder);
            
            if (string.IsNullOrEmpty(binPath))
            {
                throw new SparkleTaskException(SparkleTaskException.ExceptionTypes.UTILSNOTFOUND, String.Format("Cannot locate SolutionPackager at '{0}' - run Install-Package Microsoft.CrmSdk.CoreTools", binPath));
            }
            return binPath;
        }

        private void RunPackager(string binPath, string workingFolder, string parameters)
        {
            var procStart = new ProcessStartInfo(binPath, parameters)
            {
                WorkingDirectory = workingFolder,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                UseShellExecute = false,
                WindowStyle = ProcessWindowStyle.Hidden
            };

            _trace.WriteLine("Running {0} {1}", binPath, parameters);

            Process proc = null;
            var exitCode = 0;
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
                throw new SparkleTaskException(SparkleTaskException.ExceptionTypes.SOLUTIONPACKAGER_ERROR, String.Format("Solution Packager exited with error {0}", exitCode));
            }
        }

        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            var dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            var dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            var files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                var temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    var temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }
        private void Proc_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data!=null) _trace.WriteLine(e.Data.Replace("{", "{{").Replace("}", "}}"));
        }

        private void CreateMapFile(SolutionPackageConfig packConfig, string path)
        {
            // Create mapping xml with relative paths
            /*
            <?xml version="1.0" encoding="utf-8"?>
            <Mapping>
                <!-- Match specific named files to an alternate folder -->
                <!--<FileToFile map="Plugins.dll" to="..\..\Plugins\bin\**\Plugins.dll" />
                <FileToFile map="CRMDevTookitSampleWorkflow.dll" to="..\..\Workflow\bin\**\CRMDevTookitSample.Workflow.dll" />-->
                <!-- Match any file in and under WebResources to an alternate set of sub-folders -->
                <FileToPath map="PluginAssemblies\**\*.*" to="..\..\Plugins\bin\**" />
                <FileToPath map="WebResources\*.*" to="..\..\Webresources\Webresources\**" />
                <FileToPath map="WebResources\**\*.*" to="..\..\Webresources\Webresources\**" />
            </Mapping>
            */
            var mappingDoc = new XDocument();
            var mappings = new XElement("Mapping");
            mappingDoc.Add(mappings);

            if (packConfig != null && packConfig.map != null)
            {
                foreach (var map in packConfig.map)
                {
                    switch (map.map)
                    {
                        case MapTypes.file:
                            mappings.Add(new XElement("FileToFile",
                                new XAttribute("map", map.from),
                                new XAttribute("to", map.to)));
                            break;
                        case MapTypes.folder:
                            mappings.Add(new XElement("Folder",
                                new XAttribute("map", map.from),
                                new XAttribute("to", map.to)));
                            break;
                        case MapTypes.path:
                            mappings.Add(new XElement("FileToPath",
                             new XAttribute("map", map.from),
                             new XAttribute("to", map.to)));
                            break;
                    }
                }
            }
            else
            {
                _trace.WriteLine("No file mappings found in spkl.json");
            }
            _trace.WriteLine("Map xml created at '{0}'",path);
            File.WriteAllText(path, mappingDoc.ToString());
        }
    }
}
