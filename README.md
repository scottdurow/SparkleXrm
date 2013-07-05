SparkleXrm
==========

An open-source library for building Dynamics CRM XRM solutions using Script#, jQuery &amp; Knockoutjs.

License: [MIT](http://www.opensource.org/licenses/mit-license.php)

[www.SparkleXrm.com](http://www.SparkleXrm.com)

SparkleXrm is currently in a Preview alpha release.

If you are interested in getting a feel for this project, you can install the [SparkleXrm_0_1_0_managed.zip](https://github.com/scottdurow/SparkleXrm/raw/master/SparkleXrmSamples/SparkleXrm/SparkleXrm_0_1_0_managed.zip) and [QuoteLineEditor_0_1_0_managed.zip](https://github.com/scottdurow/SparkleXrm/raw/master/SparkleXrmSamples/QuoteLineEditor_0_1_0_managed.zip) managed solution and check it out. Open a quote, and use the 'Quote Products' tab on the form to add/edit/remove quote products.Once you've finished, deleting the solutions afterwards will remove all trace! 

Building the Source
-------------------
To build the source you will need to install:
* VS2012
* [Developer Toolkit for VS2012](http://msdn.microsoft.com/en-us/library/hh372957.aspx)
* Script# v0.7.5 (Install through Tools->Extensions & Updates)

The DebugWeb project allows you to run and debug the samples without publishing to CRM. The [standalone samples](http://www.sparklexrm.com/Tutorials/SetUpNewProject.html) projects use fiddler to allow debugging, but I found having a dedicated test web easier for the full source.

Design Goals
------------
1. Make writing client extensions for Dynamics CRM as close as possible to writing Server extensions!
2. Provide User Interface controls for editable grids and form fields
3. Speed up the code/build/publish workflow and reduce JavaScript debug time.
4. Provide UI Validation controls that work for grids and forms
5. Other libraries do a good job of providing a JavaScript library for SDK calls, but SparkleXrm aims to provide an overall framework that Xrm applications can be built upon. I don't see SparkleXrm as a replacement for those libraries for when you simply need to write from JavaScript. SparkleXrm's intended use is where you want to speed up development of HTML web resources as part of your Xrm project. In these cases, it is natural to use the same library for form/ribbon JavaScript as well.

Read [Setting up a new SparkleXrm Project](http://www.sparklexrm.com/Tutorials/SetUpNewProject.html) and about the [Quote Line Editor](http://www.sparklexrm.com/Tutorials/QuoteEditor.html) before you dive into the source!

