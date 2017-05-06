
using CmdLine;
using Microsoft.Crm.Sdk.Samples;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Tooling.Connector;
using SparkleXrm.Tasks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SparkleXrmTask
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Run();
            }
            catch (SparkleTaskException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> ex)
            {
                Console.WriteLine("The application terminated with an error.");
                Console.WriteLine("Timestamp: {0}", ex.Detail.Timestamp);
                Console.WriteLine("Code: {0}", ex.Detail.ErrorCode);
                Console.WriteLine("Message: {0}", ex.Detail.Message);
                if (!string.IsNullOrEmpty(ex.Detail.TraceText))
                {
                    Console.WriteLine("Plugin Trace: {0}", ex.Detail.TraceText);
                }
                if (ex.Detail.InnerFault != null)
                {
                    Console.WriteLine("Inner Fault: {0}",
                        null == ex.Detail.InnerFault ? "No Inner Fault" : "Has Inner Fault");
                }
            }
            catch (System.TimeoutException ex)
            {
                Console.WriteLine("The application terminated with an error.");
                Console.WriteLine("Message: {0}", ex.Message);
                Console.WriteLine("Stack Trace: {0}", ex.StackTrace);
                if (ex.InnerException != null)
                {
                    Console.WriteLine("Inner Fault: {0}",
                    null == ex.InnerException.Message ? "No Inner Fault" : ex.InnerException.Message);
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("The application terminated with an error.");
                Console.WriteLine(ex.Message);

                // Display the details of the inner exception.
                if (ex.InnerException != null)
                {
                    Console.WriteLine(ex.InnerException.Message);

                    FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> fe = ex.InnerException
                        as FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault>;
                    if (fe != null)
                    {
                        Console.WriteLine("Timestamp: {0}", fe.Detail.Timestamp);
                        Console.WriteLine("Code: {0}", fe.Detail.ErrorCode);
                        Console.WriteLine("Message: {0}", fe.Detail.Message);
                        if (!string.IsNullOrEmpty(fe.Detail.TraceText))
                        {
                            Console.WriteLine("Plugin Trace: {0}", fe.Detail.TraceText);
                        }
                        if (fe.Detail.InnerFault != null)
                        {
                            Console.WriteLine("Inner Fault: {0}",
                                null == fe.Detail.InnerFault ? "No Inner Fault" : "Has Inner Fault");
                        }
                    }
                }
            }
            //Console.WriteLine("Press any key...");
            //Console.ReadKey();
        }

        private static void Run()
        {
            try
            {
                var arguments = CommandLine.Parse<CommandLineArgs>();
                if (arguments.Task.Length < 3)
                {
                    throw new CommandLineException("Invalid Command");
                }
                
                if (arguments.Connection == null)
                {
                    // No Connection is supplied to ask for connection on command line 
                    ServerConnection serverConnect = new ServerConnection();
                    ServerConnection.Configuration config = serverConnect.GetServerConfiguration();
                    arguments.Connection = BuildConnectionString(config);

                    using (var serviceProxy = new OrganizationServiceProxy(config.OrganizationUri, config.HomeRealmUri, config.Credentials, config.DeviceCredentials))
                    {
                        // This statement is required to enable early-bound type support.
                        serviceProxy.EnableProxyTypes();
                        RunTask(arguments, serviceProxy);
                    }
                }
                else
                {
                    // Does the connection contain a password prompt?
                    var passwordMatch = Regex.Match(arguments.Connection, "Password=[*]+", RegexOptions.IgnoreCase);
                    if (passwordMatch.Success)
                    {
                        // Prompt for password
                        Console.WriteLine("Password required for connection {0}", arguments.Connection);
                        Console.Write("Password:");
                        var password = ConsoleEx.ReadPassword('*');
                        arguments.Connection = arguments.Connection.Replace(passwordMatch.Value, "Password=" + password);
                    }

                    using (var serviceProxy = new CrmServiceClient(arguments.Connection))
                    {
                        RunTask(arguments, serviceProxy);
                    }
                }
            }
            catch (CommandLineException exception)
            {
                Console.WriteLine(exception.ArgumentHelp.Message);
                Console.WriteLine(exception.ArgumentHelp.GetHelpText(Console.BufferWidth));
            }
        }

        private static string BuildConnectionString(ServerConnection.Configuration config)
        {
            //string onlineRegion, organisationName;
            //bool isOnPrem;
            //Utilities.GetOrgnameAndOnlineRegionFromServiceUri(config.OrganizationUri, out onlineRegion, out organisationName, out isOnPrem);
            string connectionString;

            // On prem connection
            // AuthType = AD; Url = http://contoso:8080/Test;

            // AuthType=AD;Url=http://contoso:8080/Test; Domain=CONTOSO; Username=jsmith; Password=passcode

            // Office 365 
            // AuthType = Office365; Username = jsmith@contoso.onmicrosoft.com; Password = passcode; Url = https://contoso.crm.dynamics.com

            // IFD
            // AuthType=IFD;Url=http://contoso:8080/Test; HomeRealmUri=https://server-1.server.com/adfs/services/trust/mex/;Domain=CONTOSO; Username=jsmith; Password=passcode

            switch (config.EndpointType)
            {
                case AuthenticationProviderType.ActiveDirectory:
                    connectionString = String.Format("AuthType=AD;Url={0}", config.OrganizationUri);

                    break;
                case AuthenticationProviderType.Federation:
                    connectionString = String.Format("AuthType=IFD;Url={0}", config.OrganizationUri);
                    break;

                case AuthenticationProviderType.OnlineFederation:
                    connectionString = String.Format("AuthType=Office365;Url={0}", config.OrganizationUri);
                    break;

                default:
                    throw new SparkleTaskException(SparkleTaskException.ExceptionTypes.AUTH_ERROR, String.Format("Unsupported Endpoint Type {0}", config.EndpointType.ToString()));
            }

            if (config.Credentials != null && config.Credentials.Windows != null)
            {
                if (!String.IsNullOrEmpty(config.Credentials.Windows.ClientCredential.Domain))
                {
                    connectionString += ";DOMAIN=" + config.Credentials.Windows.ClientCredential.Domain;
                }

                if (!String.IsNullOrEmpty(config.Credentials.Windows.ClientCredential.UserName))
                {
                    connectionString += ";Username=" + config.Credentials.Windows.ClientCredential.UserName;
                }

                if (!String.IsNullOrEmpty(config.Credentials.Windows.ClientCredential.Password))
                {
                    connectionString += ";Password=" + config.Credentials.Windows.ClientCredential.Password;
                }
                else if (config.Credentials.Windows.ClientCredential.SecurePassword != null && config.Credentials.Windows.ClientCredential.SecurePassword.Length > 0)
                {
                    var password = new System.Net.NetworkCredential(string.Empty, config.Credentials.Windows.ClientCredential.SecurePassword).Password;
                    connectionString += ";Password=" + password;
                }
            }

            if (config.Credentials != null)
            {
                if (!String.IsNullOrEmpty(config.Credentials.UserName.UserName))
                {
                    connectionString += ";Username=" + config.Credentials.UserName.UserName;
                }
                if (!String.IsNullOrEmpty(config.Credentials.UserName.Password))
                {
                    connectionString += ";Password=" + config.Credentials.UserName.Password;
                }
            }

            if (config.HomeRealmUri != null)
            {
                connectionString += ";HomeRealmUri=" + config.HomeRealmUri.ToString();
            }

            return connectionString;
        }

        private static void RunTask(CommandLineArgs arguments, IOrganizationService service)
        {
            if (arguments.Path == null)
            {
              
                arguments.Path = Directory.GetCurrentDirectory();
            }
            else
            {
                arguments.Path = Path.Combine(Directory.GetCurrentDirectory(), arguments.Path);
            }

            var trace = new TraceLogger();
            BaseTask task = null;
            switch (arguments.Task.ToLower())
            {
                case "plugins":
                    trace.WriteLine("Deploying Plugins");
                    task = new DeployPluginsTask(service, trace);
                    break;

                case "workflow":
                    trace.WriteLine("Deploying Custom Workflow Activities");
                    task = new DeployWorkflowActivitiesTask(service, trace);
                    break;

                case "webresources":
                    trace.WriteLine("Deploying WebResources");
                    task = new DeployWebResourcesTask(service, trace);
                    break;

                case "instrument":
                    trace.WriteLine("Downloading Plugin/Workflow Activity Metadata");
                    task = new DownloadPluginMetadataTask(service, trace);
                    break;
               
                case "get-webresources":
                    trace.WriteLine("Downloading Webresources");
                    task = new DownloadWebresourceConfigTask(service, trace);
                    task.Prefix = arguments.Prefix;
                    break;
                case "earlybound":
                    trace.WriteLine("Generating early bound types");
                    var earlyBound = new EarlyBoundClassGeneratorTask(service, trace);
                    task = earlyBound;
                    earlyBound.ConectionString = arguments.Connection;
                    break;
            }

            if (arguments.Profile != null)
                task.Profile = arguments.Profile;
            if (task != null)
            {
                task.Execute(arguments.Path);
            }
            else
                throw new SparkleTaskException(SparkleTaskException.ExceptionTypes.NO_TASK_SUPPLIED, "Task not recognised. Please consult help!");            
        }
    }
}
