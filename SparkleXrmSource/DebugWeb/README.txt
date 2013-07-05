The DebugWeb is a webapplication project that is used to mimic the CRM environment so that html can be edited and tested without publishing webresources and using Fiddler.
This is especially useful when prototyping webresources and new features.

Your CRM Server details should be set in the DebugWeb/Webresources/CLientGlobalContext.js.aspx file so that the scripts can connect to the correct webservice endpoint.

To get client debugging working in Chrome, you will need to use Fiddler and add the following to the OnBeforeResponse Script:
oSession.oResponse.headers.Add("Access-Control-Allow-Origin","http://localhost:43684");
See http://www.develop1.net/public/post/Debugging-HTML-Webresources-on-localhost.aspx

Let me know if you need any more info - @ScottDurow