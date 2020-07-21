using Microsoft.Xrm.Tooling.Connector;
using Microsoft.Xrm.Tooling.CrmConnectControl;
using System;
using System.Windows;
using System.Windows.Threading;

namespace SparkleXrmTask
{
    /// <summary>
    /// Interaction logic for CRMLoginForm.xaml
    /// </summary>
    public partial class CRMLoginForm : Window
    {
        #region Vars
        /// <summary>
        /// Microsoft.Xrm.Tooling.Connector services
        /// </summary>
        private CrmServiceClient CrmSvc;
        /// <summary>
        /// Bool flag to determine if there is a connection 
        /// </summary>
        private bool bIsConnectedComplete;
        /// <summary>
        /// CRM Connection Manager component. 
        /// </summary>
        private CrmConnectionManager mgr;
        /// <summary>
        ///  This is used to allow the UI to reset w/out closing 
        /// </summary>
        private bool resetUiFlag;
        #endregion

        #region Properties
        /// <summary>
        /// CRM Connection Manager 
        /// </summary>
        public CrmConnectionManager CrmConnectionMgr { get { return mgr; } }

        public string AppId
        {
            get;
            set;
        }

        public Uri RedirectUri
        {
            get;
            set;
        }

        #endregion

        #region Event
        /// <summary>
        /// Raised when a connection to CRM has completed. 
        /// </summary>
        public event EventHandler ConnectionToCrmCompleted;
        #endregion


        public CRMLoginForm()
        {
            InitializeComponent();
            //// Should be used for testing only.
            //ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) =>
            //{
            //    MessageBox.Show("CertError");
            //    return true;
            //};
        }

        /// <summary>
        /// Raised when the window loads for the first time. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            /*
				This is the setup process for the login control, 
				The login control uses a class called CrmConnectionManager to manage the interaction with CRM, this class and also be queried as later points for information about the current connection. 
				In this case, the login control is referred to as CrmLoginCtrl
			 */

            // Set off flag. 
            bIsConnectedComplete = false;

            // Init the CRM Connection manager.. 
            mgr = new CrmConnectionManager
            {
                UseUserLocalDirectoryForConfigStore = true,
                ClientId = AppId,
                RedirectUri = RedirectUri,
                ParentControl = CrmLoginCtrl
            };
            mgr.UseUserLocalDirectoryForConfigStore = true;
            CrmLoginCtrl.SetGlobalStoreAccess(mgr);
            CrmLoginCtrl.SetControlMode(ServerLoginConfigCtrlMode.FullLoginPanel);
            CrmLoginCtrl.ConnectionCheckBegining += CrmLoginCtrl_ConnectionCheckBegining;
            CrmLoginCtrl.ConnectErrorEvent += CrmLoginCtrl_ConnectErrorEvent;
            CrmLoginCtrl.ConnectionStatusEvent += CrmLoginCtrl_ConnectionStatusEvent;
            CrmLoginCtrl.UserCancelClicked += CrmLoginCtrl_UserCancelClicked;
            //if (!mgr.RequireUserLogin())
            //{
            //    if (MessageBox.Show("Credentials already saved in configuration\nChoose Yes to Auto Login or No to Reset Credentials", "Auto Login", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            //    {
            //        CrmLoginCtrl.IsEnabled = false;
            //        mgr.ServerConnectionStatusUpdate += mgr_ServerConnectionStatusUpdate;
            //        mgr.ConnectionCheckComplete += mgr_ConnectionCheckComplete;
            //        mgr.ConnectToServerCheck();
            //        CrmLoginCtrl.ShowMessageGrid();
            //    }
            //}
        }

        #region Events

        /// <summary>
        /// Updates from the Auto Login process. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mgr_ServerConnectionStatusUpdate(object sender, ServerConnectStatusEventArgs e)
        {
            // The Status event will contain information about the current login process,  if Connected is false, then there is not yet a connection. 
            // Set the updated status of the loading process. 
            Dispatcher?.Invoke(DispatcherPriority.Normal,
                new Action(() =>
                    {
                        Title = string.IsNullOrWhiteSpace(e.StatusMessage) ? e.ErrorMessage : e.StatusMessage;
                    }
                ));
        }

        /// <summary>
        /// Complete Event from the Auto Login process
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mgr_ConnectionCheckComplete(object sender, ServerConnectStatusEventArgs e)
        {
            // The Status event will contain information about the current login process,  if Connected is false, then there is not yet a connection. 
            // Unwire events that we are not using anymore, this prevents issues if the user uses the control after a failed login. 
            ((CrmConnectionManager)sender).ConnectionCheckComplete -= mgr_ConnectionCheckComplete;
            ((CrmConnectionManager)sender).ServerConnectionStatusUpdate -= mgr_ServerConnectionStatusUpdate;

            if (!e.Connected)
            {
                // if its not connected pop the login screen here. 
                if (e.MultiOrgsFound)
                    MessageBox.Show("Unable to Login to CRM using cached credentials. Org Not found", "Login Failure");
                else
                    MessageBox.Show("Unable to Login to CRM using cached credentials", "Login Failure");

                resetUiFlag = true;
                CrmLoginCtrl.GoBackToLogin();
                // Bad Login Get back on the UI. 
                Dispatcher.Invoke(DispatcherPriority.Normal,
                       new Action(() =>
                       {
                           Title = "Failed to Login with cached credentials.";
                           MessageBox.Show(Title, "Notification from ConnectionManager", MessageBoxButton.OK, MessageBoxImage.Error);
                           CrmLoginCtrl.IsEnabled = true;
                       }
                        ));
                resetUiFlag = false;
            }
            else
            {
                // Good Login Get back on the UI 
                if (e.Connected && !bIsConnectedComplete)
                    ProcessSuccess();
            }

        }

        /// <summary>
        ///  Login control connect check starting. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CrmLoginCtrl_ConnectionCheckBegining(object sender, EventArgs e)
        {
            bIsConnectedComplete = false;
            Dispatcher.Invoke(DispatcherPriority.Normal,
                               new Action(() =>
                               {
                                   Title = "Starting Login Process. ";
                                   CrmLoginCtrl.IsEnabled = true;
                               }
                                   ));
        }

        /// <summary>
        /// Login control connect check status event. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CrmLoginCtrl_ConnectionStatusEvent(object sender, ConnectStatusEventArgs e)
        {
            //Here we are using the bIsConnectedComplete bool to check to make sure we only process this call once. 
            if (e.ConnectSucceeded && !bIsConnectedComplete)
                ProcessSuccess();

        }

        /// <summary>
        /// Login control Error event. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CrmLoginCtrl_ConnectErrorEvent(object sender, ConnectErrorEventArgs e)
        {
            //MessageBox.Show(e.ErrorMessage, "Error here");
        }

        /// <summary>
        /// Login Control Cancel event raised. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CrmLoginCtrl_UserCancelClicked(object sender, EventArgs e)
        {
            if (!resetUiFlag)
                Close();
        }

        #endregion

        /// <summary>
        /// This raises and processes Success
        /// </summary>
        private void ProcessSuccess()
        {
            resetUiFlag = true;
            bIsConnectedComplete = true;
            CrmSvc = mgr.CrmSvc;
            CrmLoginCtrl.GoBackToLogin();
            Dispatcher.Invoke(DispatcherPriority.Normal,
               new Action(() =>
               {
                   Title = "Notification from Parent";
                   CrmLoginCtrl.IsEnabled = true;
               }
                ));

            // Notify Caller that we are done with success. 
            if (ConnectionToCrmCompleted != null)
                ConnectionToCrmCompleted(this, null);

            resetUiFlag = false;
        }

    }

    #region system.diagnostics settings for this control

    // Add or merge this section to your app to enable diagnostics on the use of the CRM login control and connection
    /*
  <system.diagnostics>
    <trace autoflush="true" />
    <sources>
      <source name="Microsoft.Xrm.Tooling.Connector.CrmServiceClient"
              switchName="Microsoft.Xrm.Tooling.Connector.CrmServiceClient"
              switchType="System.Diagnostics.SourceSwitch">
        <listeners>
          <add name="console" type="System.Diagnostics.DefaultTraceListener" />
          <remove name="Default"/>
          <add name ="fileListener"/>
        </listeners>
      </source>
      <source name="Microsoft.Xrm.Tooling.CrmConnectControl"
              switchName="Microsoft.Xrm.Tooling.CrmConnectControl"
              switchType="System.Diagnostics.SourceSwitch">
        <listeners>
          <add name="console" type="System.Diagnostics.DefaultTraceListener" />
          <remove name="Default"/>
          <add name ="fileListener"/>
        </listeners>
      </source>

      <source name="Microsoft.Xrm.Tooling.WebResourceUtility"
              switchName="Microsoft.Xrm.Tooling.WebResourceUtility"
              switchType="System.Diagnostics.SourceSwitch">
        <listeners>
          <add name="console" type="System.Diagnostics.DefaultTraceListener" />
          <remove name="Default"/>
          <add name ="fileListener"/>
        </listeners>
      </source>
      
    <!-- WCF DEBUG SOURCES -->
      <source name="System.IdentityModel" switchName="System.IdentityModel">
        <listeners>
          <add name="xml" />
        </listeners>
      </source>
      <!-- Log all messages in the 'Messages' tab of SvcTraceViewer. -->
      <source name="System.ServiceModel.MessageLogging" switchName="System.ServiceModel.MessageLogging" >
        <listeners>
          <add name="xml" />
        </listeners>
      </source>
      <!-- ActivityTracing and propogateActivity are used to flesh out the 'Activities' tab in
           SvcTraceViewer to aid debugging. -->
      <source name="System.ServiceModel" switchName="System.ServiceModel" propagateActivity="true">
        <listeners>
          <add name="xml" />
        </listeners>
      </source>
      <!-- END WCF DEBUG SOURCES -->
    </sources>
    <switches>
      <!-- 
            Possible values for switches: Off, Error, Warining, Info, Verbose
                Verbose:    includes Error, Warning, Info, Trace levels
                Info:       includes Error, Warning, Info levels
                Warning:    includes Error, Warning levels
                Error:      includes Error level
        -->
      <add name="Microsoft.Xrm.Tooling.Connector.CrmServiceClient" value="Verbose" />
      <add name="Microsoft.Xrm.Tooling.CrmConnectControl" value="Verbose"/>
      <add name="Microsoft.Xrm.Tooling.WebResourceUtility" value="Verbose" />
      <add name="System.IdentityModel" value="Verbose"/>
      <add name="System.ServiceModel.MessageLogging" value="Verbose"/>
      <add name="System.ServiceModel" value="Error, ActivityTracing"/>
      
    </switches>
    <sharedListeners>
      <add name="fileListener" type="System.Diagnostics.TextWriterTraceListener" initializeData="LoginControlTesterLog.txt"/>
      <!--<add name="eventLogListener" type="System.Diagnostics.EventLogTraceListener" initializeData="CRM UII"/>-->
      <add name="xml" type="System.Diagnostics.XmlWriterTraceListener" initializeData="CrmToolBox.svclog" />
    </sharedListeners>
  </system.diagnostics>
*/

    #endregion
}
