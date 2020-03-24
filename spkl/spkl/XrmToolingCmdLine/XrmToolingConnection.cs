using Microsoft.Xrm.Tooling.Connector;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;

namespace SparkleXrmTask.XrmToolingCmdLine
{
    public class XrmToolingConnection
    {
        /// <summary>
        /// Connection configurations - no credentials or tokens are stored here
        /// </summary>
        public static readonly string ConnectionsFilePath = Path.Combine(
             Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "spkl"),
             "Connections.json");

        /// <summary>
        /// Token Cache is used to store the Xrm Tooling tokens so user is not always prompted to login where possible
        /// </summary>
        public static readonly string TokenPath = Path.Combine(
             Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "spkl"),
             "TokenCache");

        public string ConnectionString { get; internal set; }
        private List<SavedConnection> _savedConnections = new List<SavedConnection>();

        public XrmToolingConnection()
        {
            LoadSettings();
        }

        public CrmServiceClient Connect()
        {
            var connectionStringRoot = $@"AuthType=OAuth;
                      AppId=51f81489-12ee-4a9e-aaae-a2591f45987d;
                      RedirectUri=app://58145B91-0C36-4500-8554-080854F2AC97;
                      TokenCacheStorePath={XrmToolingConnection.TokenPath}";

            // Show list of saved connections
            // We don't store the credentials but instead use the Xrm.Tooling token cache
            var configNumber = -1;
            if (_savedConnections.Count > 0)
            {
                var i = 1;
                Console.WriteLine("(0) Add New Server Configuration");
                foreach (SavedConnection connection in _savedConnections)
                {
                    Console.WriteLine($"({i}) {connection.EnvironmentUrl},  {connection.DisplayName},  {connection.UserName}");
                    i++;
                }

                Console.Write($"\nSpecify the saved server configuration number (0-{_savedConnections.Count}) [{_savedConnections.Count}] : ");
                var input = Console.ReadLine();
                Console.WriteLine();
                if (input == string.Empty)
                {
                    input = _savedConnections.Count.ToString();
                }

                if (!int.TryParse(input, out configNumber))
                {
                    configNumber = -1;
                }
            }

            SavedConnection selectedConnection;
            var loginPrompt = "Auto";
            var newConnection = configNumber <= 0;
            if (newConnection)
            {
                selectedConnection = new SavedConnection();
                var envUrl = ReadLine("Environment/Organization Url (e.g. org123.crm.dynamics.com)", regex: @"^(?<!http)([^\s:\/]+)(\.crm[0-9]*\.dynamics\.com[\/]?)$");
                envUrl = envUrl.TrimEnd('/');
                selectedConnection.EnvironmentUrl = envUrl;
                // If new connection - set LoginPrompt=Always
                loginPrompt = "Always";
            }
            else
            {
                selectedConnection = _savedConnections[configNumber - 1];
                // Move the saved connection to the end so it's default next time
                _savedConnections.Remove(selectedConnection);
                _savedConnections.Add(selectedConnection);
                SaveSettings();
                connectionStringRoot += $";UserName={selectedConnection.UserName}";
            }

            var connectionString = $@"{connectionStringRoot};Url=https://{selectedConnection.EnvironmentUrl};LoginPrompt={loginPrompt}";
            var client = new CrmServiceClient(connectionString);
            if (client.IsReady)
            {
                if (newConnection)
                {
                    selectedConnection.EnvironmentUrl = client.CrmConnectOrgUriActual.Host;
                    selectedConnection.UserName = client.OAuthUserId;
                    selectedConnection.DisplayName = client.ConnectedOrgFriendlyName;
                    connectionString += $";UserName={selectedConnection.UserName}";
                    // Is this replacing an existing connection?
                    _savedConnections.RemoveAll(c => c.EnvironmentUrl.Equals(selectedConnection.EnvironmentUrl, StringComparison.InvariantCultureIgnoreCase) &&
                        c.UserName.Equals(selectedConnection.UserName, StringComparison.InvariantCultureIgnoreCase));
                    _savedConnections.Add(selectedConnection);
                    SaveSettings();
                }
                this.ConnectionString = connectionString;
                return client;
            }
            else
            {
                throw new Exception($"Cannot connect: {client.LastCrmError}", client.LastCrmException);
            }
        }

        private void LoadSettings()
        {
            if (File.Exists(XrmToolingConnection.ConnectionsFilePath))
            {
                string configJson = File.ReadAllText(XrmToolingConnection.ConnectionsFilePath);
                _savedConnections = JsonConvert.DeserializeObject<List<SavedConnection>>(configJson);
            }
        }

        private void SaveSettings()
        {
            string configJson = JsonConvert.SerializeObject(_savedConnections);
            File.WriteAllText(XrmToolingConnection.ConnectionsFilePath, configJson);
        }

        private string ReadLine(string prompt, string regex = null, string defaultValue = null)
        {
            if (defaultValue != null)
            {
                prompt += $"[{defaultValue}]";
            }

            bool isValid = true;
            string returnedValue = defaultValue;
            do
            {
                Console.Write(prompt + ": ");
                string input = Console.ReadLine();
                if (input.Length > 0)
                {
                    if (regex != null && Regex.IsMatch(input, regex))
                    {
                        returnedValue = input;
                        isValid = true;
                    }
                    else
                    {
                        Console.WriteLine("\nInput invalid");
                        isValid = false;
                    }
                }
                else if (defaultValue == null)
                {
                    Console.WriteLine("\nInput Required");
                    isValid = false;
                }
            } while (!isValid);
            Console.Write("\n");
            return returnedValue;
        }
    }
}