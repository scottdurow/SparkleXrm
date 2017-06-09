using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using SparkleXrm.Tasks.Config;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparkleXrm.Tasks
{
    public class SolutionPackagerTask : BaseTask
    {
        public string ConectionString { get; set; }
        private string _folder;
        public SolutionPackagerTask(IOrganizationService service, ITrace trace) : base(service, trace)
        {
        }

        public SolutionPackagerTask(OrganizationServiceContext ctx, ITrace trace) : base(ctx, trace)
        {
        }

        protected override void ExecuteInternal(string folder, OrganizationServiceContext ctx)
        {

            _trace.WriteLine("Searching for packager config in '{0}'", folder);
            var configs = ConfigFile.FindConfig(folder);

            foreach (var config in configs)
            {
                _trace.WriteLine("Using Config '{0}'", config.filePath);
                _folder = config.filePath;
                UnPack(ctx, config);
            }
            _trace.WriteLine("Processed {0} config(s)", configs.Count);


        }

        private void UnPack(OrganizationServiceContext ctx, ConfigFile config)
        {
            var configs = config.GetSolutionConfig(this.Profile);
            foreach (var solutionPackagerConfig in configs)
            {
                var movetoFolder = Path.Combine(config.filePath, solutionPackagerConfig.packagepath);
                string unpackPath = UnPackSolution(solutionPackagerConfig);

                // Delete existing content 
                if (Directory.Exists(movetoFolder))
                {
                    Directory.Delete(movetoFolder, true);
                }
                // Copy to the package path
                DirectoryCopy(unpackPath, movetoFolder, true);
            }
        }

        private void Diff(OrganizationServiceContext ctx, ConfigFile config)
        {
            var configs = config.GetSolutionConfig(this.Profile);
            foreach (var solutionPackagerConfig in configs)
            {
                var movetoFolder = Path.Combine(config.filePath, solutionPackagerConfig.packagepath);
                string unpackPath = UnPackSolution(solutionPackagerConfig);

                // Delete existing content 
                Directory.Delete(movetoFolder, true);

                // Copy to the package path
                DirectoryCopy(unpackPath, movetoFolder, true);
            }
        }

        private string UnPackSolution(SolutionPackageConfig solutionPackagerConfig)
        {
            
            // get random folder
               
            var targetFolder = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
          

            // Extract solution
            var request = new ExportSolutionRequest
            {
                SolutionName = solutionPackagerConfig.solution,
                ExportAutoNumberingSettings = true,
                ExportCalendarSettings = true,
                ExportCustomizationSettings = true,
                ExportEmailTrackingSettings = true,
                ExportExternalApplications = true,
                ExportGeneralSettings = true,
                ExportIsvConfig = true,
                ExportMarketingSettings = true,
                ExportOutlookSynchronizationSettings = true,
                ExportRelationshipRoles = true,
                ExportSales = true,
                Managed = false


            };

            var response = (ExportSolutionResponse)_service.Execute(request);

            // Save solution 
            var solutionZipPath = Path.GetTempFileName();
            File.WriteAllBytes(solutionZipPath, response.ExportSolutionFile);

            // locate the CrmSvcUtil package folder
            var targetfolder = DirectoryEx.GetApplicationDirectory();
            // move from spkl.v.v.v.\tools - back to packages folder
            var binPath = DirectoryEx.Search(targetfolder + @"\..\..", "SolutionPackager.exe");
            _trace.WriteLine("Target {0}", targetfolder);
            var binFolder = new FileInfo(binPath).DirectoryName;
            if (string.IsNullOrEmpty(binPath))
            {
                throw new SparkleTaskException(SparkleTaskException.ExceptionTypes.UTILSNOTFOUND, String.Format("Cannot locate SolutionPackager at '{0}' - run Install-Package Microsoft.CrmSdk.CoreTools", binPath));
            }

            // Run CrmSvcUtil 
            string parameters = String.Format(@"/action:Extract /zipfile:{0} /folder:{1} /packagetype:Unmanaged /allowWrite:Yes /allowDelete:Yes /clobber /errorlevel:Verbose /nologo /log:packagerlog.txt",
                solutionZipPath,
                targetFolder
                );

            ProcessStartInfo procStart = new ProcessStartInfo(binPath, parameters)
            {
                WorkingDirectory = binFolder,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                UseShellExecute = false,
                WindowStyle = ProcessWindowStyle.Hidden


            };

            _trace.WriteLine("Running {0} {1}", binPath, parameters);

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
            catch (Exception ex)
            {
                throw;
            }

            finally
            {
                proc.Close();
            }

            
            return targetFolder;
        }

        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }
        private void Proc_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data!=null) _trace.WriteLine(e.Data.Replace("{", "{{").Replace("}", "}}"));
        }

    }
}
