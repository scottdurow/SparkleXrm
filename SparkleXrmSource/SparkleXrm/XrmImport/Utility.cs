using System.Runtime.CompilerServices;
using System;

namespace Xrm
{
    [Imported]
    public class Utility
    {
        /// <summary>
        /// Opens an entity form
        /// </summary>
        /// <param name="name">The logical name of an entity</param>
        /// <param name="id">The string representation of a unique identifier or the record to open in the form. If not set, a form to create a new record is opened</param>
        /// <param name="parameters">A dictionary object that passes extra query string parameters to the form. Invalid query string parameters will cause an error</param>
        /// <remarks>This function provides a better developer experience than the process of manipulating the URL passed to the window.open method described in Open forms, views, dialogs and reports with a URL. Using this function also helps ensure that users are not prompted to log in again under certain circumstances. </remarks>
        public static object OpenEntityForm(string name, string id, object parameters)
        {
            return null;
        }
        /// <summary>
        /// Opens an entity form
        /// </summary>
        /// <param name="name">The logical name of an entity</param>
        /// <param name="id">The string representation of a unique identifier or the record to open in the form. If not set, a form to create a new record is opened</param>
        /// <param name="parameters">A dictionary object that passes extra query string parameters to the form. Invalid query string parameters will cause an error</param>
        /// <param name="windowOptions">For Microsoft Dynamics CRM Online 2015 Update 1 or later use this optional parameter in the web application to control how the form opens. You can choose to open a form in a new window by passing a dictionary object with a Boolean openInNewWindow property set to true.
        /// This parameter is ignored in CRM for tablets and CRM for phones. </param>
        /// <remarks>This function provides a better developer experience than the process of manipulating the URL passed to the window.open method described in Open forms, views, dialogs and reports with a URL. Using this function also helps ensure that users are not prompted to log in again under certain circumstances. </remarks>
        [ScriptName("openEntityForm")]
        public static object OpenEntityForm2(string name, string id, object parameters, OpenEntityFormOptions windowOptions)
        {
            return null;
        }
        /// <summary>
        /// Opens an HTML web resource.  
        /// </summary>
        /// <param name="webResourceName">The name of the HTML web resource to open</param>
        /// <param name="webResourceData">Data to be passed into the data parameter</param>
        /// <param name="width">The width of the window to open in pixels</param>
        /// <param name="height">The height of the window to open in pixels</param>
        /// <returns>Window object</returns>
        /// <remarks>An HTML web resource can accept the parameter values described in Passing Parameters to HTML Web Resources. This function only provides for passing in the optional data parameter. To pass values for the other valid parameters, you must append them to the webResourceName parameter.  NOTE: This function will not work with Microsoft Dynamics CRM for tablets.</remarks>
        public static object OpenWebResource(string webResourceName, string webResourceData, int width, int height)
        {
            return null;
        }

        /// <summary>
        /// CRM2013 Only: Determine if an entity is an activity entity
        /// </summary>
        /// <param name="entityName">String. The logicalName of an entity</param>
        /// <returns>True if the entity is an activity entity, otherwise false</returns>
        public static bool IsActivityType(string entityName)
        { 
            return false; 
        }

        /// <summary>
        /// CRM2013 Only: Displays a dialog box containing an application-defined message
        /// </summary>
        /// <param name="message">The text of the message to display in the dialog</param>
        /// <param name="onCloseCallback">A function to execute when the OK button is clicked</param>
        /// <remarks>This method is only available for Updated Entities</remarks>
        public static void AlertDialog(string message, ParameterlessFunctionHandler onCloseCallback)
        { 
        }

        /// <summary>
        /// CRM2013 Only: Displays a confirmation dialog box that contains an optional message as well as OK and Cancel buttons
        /// </summary>
        /// <param name="message">The text of the message to display in the dialog</param>
        /// <param name="yesCloseCallback">A function to execute when the OK button is clicked</param>
        /// <param name="noCloseCallback">A function to execute when the Cancel button is clicked</param>
        /// <remarks>This method is only available for Updated Entities</remarks>
        public static void ConfirmDialog(string message, ParameterlessFunctionHandler yesCloseCallback, ParameterlessFunctionHandler noCloseCallback)
        { 
        }

    }

    [Imported]
    [IgnoreNamespace]
    [ScriptNamespace("")]
    public static class GlobalFunctions
    {
        
       
        [ScriptAlias("encodeURIComponent")]
        public static string encodeURIComponent(string values)
        {
            return null;
        }

    }

    [Imported]
    [IgnoreNamespace]
    [ScriptName("Object")]
    public class OpenEntityFormOptions
    {
         public bool OpenInNewWindow;
    }
}