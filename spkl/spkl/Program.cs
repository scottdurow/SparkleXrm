using CmdLine;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Crm.Sdk.Samples;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Tooling.Connector;
using SparkleXrm.Tasks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SparkleXrmTask
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("spkl Task Runner v" + Assembly.GetEntryAssembly().GetName().Version + "\tTasks v" + Assembly.GetAssembly(typeof(SparkleXrm.Tasks.BaseTask)).GetName().Version);

            Console.ForegroundColor = ConsoleColor.Gray;
            bool error = false;
            CommandLineArgs arguments = null;
            try
            {
                arguments = CommandLine.Parse<CommandLineArgs>();

                Run(arguments);
            }
            catch (CommandLineException exception)
            {
                Console.WriteLine(exception.ArgumentHelp.Message);
                Console.WriteLine(exception.ArgumentHelp.GetHelpText(Console.BufferWidth));
            }
            catch (SparkleTaskException ex)
            {
                Console.WriteLine(ex.Message);
                error = true;
            }
            catch (FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> ex)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("The application terminated with an error.");
                Console.WriteLine("Timestamp: {0}", ex.Detail.Timestamp);
                Console.WriteLine("Code: {0}", ex.Detail.ErrorCode);
                Console.WriteLine("Message: {0}", ex.Detail.Message);
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine(ex.StackTrace);

                if (!string.IsNullOrEmpty(ex.Detail.TraceText))
                {
                    Console.WriteLine("Plugin Trace: {0}", ex.Detail.TraceText);
                }
                if (ex.Detail.InnerFault != null)
                {
                    Console.WriteLine("Inner Fault: {0}",
                        null == ex.Detail.InnerFault ? "No Inner Fault" : "Has Inner Fault");
                }
                error = true;
                Console.ForegroundColor = ConsoleColor.White;
            }
            catch (System.TimeoutException ex)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("The application terminated with an error.");
                Console.WriteLine("Message: {0}", ex.Message);
                Console.WriteLine("Stack Trace: {0}", ex.StackTrace);
                if (ex.InnerException != null)
                {
                    Console.WriteLine("Inner Fault: {0}",
                    null == ex.InnerException.Message ? "No Inner Fault" : ex.InnerException.Message);
                }
                error = true;
                Console.ForegroundColor = ConsoleColor.White;
            }
            catch (System.Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("The application terminated with an error.");
                Console.WriteLine(ex.Message);
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine(ex.StackTrace);

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
                error = true;
                Console.ForegroundColor = ConsoleColor.White;
            }
            finally
            {
                if (error)
                {
                    Environment.ExitCode = 1;
                }
            }
            if (arguments != null && arguments.WaitForKey == true)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Press any key...");
                Console.ReadKey();
            }
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        private static void Run(CommandLineArgs arguments)
        {
            try
            {
                if (arguments.Task.Length < 3)
                {
                    throw new CommandLineException("Invalid Command");
                }
                var trace = new TraceLogger();

                if (arguments.Connection == null)
                {
                    // No Connection is supplied to ask for connection on command line
                    ServerConnection serverConnect = new ServerConnection();
                    ServerConnection.Configuration config = serverConnect.GetServerConfiguration(arguments.IgnoreLocalPrincipal);

                    arguments.Connection = BuildConnectionString(config);

                    using (var serviceProxy = new OrganizationServiceProxy(config.OrganizationUri, config.HomeRealmUri, config.Credentials, config.DeviceCredentials))
                    {
                        // This statement is required to enable early-bound type support.
                        serviceProxy.EnableProxyTypes();
                        serviceProxy.Timeout = new TimeSpan(1, 0, 0);
                        RunTask(arguments, serviceProxy, trace);
                    }
                }
                else if (arguments.Connection == "")
                {
                    // Support for tasks that require no connection string such as pack
                    RunTask(arguments, null, trace);
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
                    CrmServiceClient.MaxConnectionTimeout = new TimeSpan(1, 0, 0);
                    using (var serviceProxy = new CrmServiceClient(arguments.Connection))
                    {
                        if (!serviceProxy.IsReady)
                        {
                            throw new SparkleTaskException(SparkleTaskException.ExceptionTypes.AUTH_ERROR, String.Format("Error connecting to the Organization Service: {0}", serviceProxy.LastCrmError));
                        }

                        if (!serviceProxy.IsReady)
                        {
                            trace.WriteLine("Not Ready {0} {1}", serviceProxy.LastCrmError, serviceProxy.LastCrmException);
                        }

                        RunTask(arguments, serviceProxy, trace);
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

        private static void RunTask(CommandLineArgs arguments, IOrganizationService service, ITrace trace)
        {
            if (arguments.Path == null)
            {
                arguments.Path = Directory.GetCurrentDirectory();
            }
            else
            {
                // Strip trailing \
                arguments.Path = arguments.Path.TrimEnd('\\');
                arguments.Path = Path.Combine(Directory.GetCurrentDirectory(), arguments.Path);
            }

            BaseTask task = null;
            string command = arguments.Task.ToLower();
            switch (command)
            {
                case "whoami":
                    trace.WriteLine("Who Am I?");
                    var whoAmIResponse = service.Execute(new WhoAmIRequest()) as WhoAmIResponse;
                    Console.WriteLine($"OrgId:'${whoAmIResponse.OrganizationId}' UserId:'${whoAmIResponse.UserId}'");
                    return;

                case "plugins":
                    trace.WriteLine("Deploying Plugins");
                    task = new DeployPluginsTask(service, trace)
                    { ExcludePluginSteps = arguments.ExcludePluginSteps };
                    break;

                case "workflow":
                    trace.WriteLine("Deploying Custom Workflow Activities");
                    task = new DeployWorkflowActivitiesTask(service, trace)
                    {
                      ExcludePluginSteps = arguments.ExcludePluginSteps
                    };
                    break;

                case "pluginandworkflows":
                    trace.WriteLine("Deploying Plugin and workflows");
                    task = new DeployPluginsAndWorkflowTask(service, trace)
                    {
                      ExcludePluginSteps = arguments.ExcludePluginSteps
                    };
                    break;

                case "webresources":
                    trace.WriteLine("Deploying WebResources");
                    task = new DeployWebResourcesTask(service, trace);
                    break;

                case "instrument":
                    trace.WriteLine("Downloading Plugin/Workflow Activity Metadata");
                    task = new DownloadPluginMetadataTask(service, trace);
                    break;

                case "download-webresources":
                    trace.WriteLine("Downloading Webresources");
                    task = new DownloadWebresourceFileTask(service, trace)
                    { Overwrite = arguments.Overwrite };
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

                case "unpack":
                    trace.WriteLine("Unpacking solution");
                    var packager = new SolutionPackagerTask(service, trace);
                    packager.command = command;
                    task = packager;
                    break;

                case "unpacksolution":
                    trace.WriteLine("Unpacking solution Zip");
                    var unpackfromsolution = new SolutionPackagerTask(service, trace);
                    unpackfromsolution.command = command;
                    task = unpackfromsolution;
                    break;

                case "pack":
                    trace.WriteLine("Packing Solution");
                    var pack = new SolutionPackagerTask(service, trace);
                    pack.command = command;
                    task = pack;
                    break;

                case "import":
                    trace.WriteLine("Packing & Import Solution");
                    var import = new SolutionPackagerTask(service, trace);
                    import.command = command;
                    task = import;
                    break;

                case "export":
                    trace.WriteLine("Exporting Solution");
                    var export = new SolutionPackagerTask(service, trace);
                    export.command = command;
                    task = export;
                    break;

                case "compare":
                    trace.WriteLine("Comparing Solution");
                    var compare = new SolutionPackagerTask(service, trace);
                    compare.command = command;
                    task = compare;
                    break;
            }

            if (task != null)
            {
                if (arguments.Profile != null)
                    task.Profile = arguments.Profile;
                task.Execute(arguments.Path);
            }
            else
                throw new SparkleTaskException(SparkleTaskException.ExceptionTypes.NO_TASK_SUPPLIED, String.Format("Task '{0}' not recognised. Please consult help!", arguments.Task.ToLower()));
        }
    }
}