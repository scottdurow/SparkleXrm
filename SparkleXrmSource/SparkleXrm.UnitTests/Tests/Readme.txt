To run unit tests:
1. Install Fiddler
2. Enable AutoResponder
3. Add the autoresponder:
	regex:(?insx).+/WebResources/sparkle_/unittests/(?'fname'[^?]*).*
	<repo location>\SparkleXRM\SparkleXrmSource\SparkleXrm.UnitTests\Tests\${fname}

4. You can then browse to https://<orgname>.crm<x>.dynamics.com/Webresources/sparkle_/unittests/Runinbrowser.htm

NOTE: When using fiddler SSL decryption you'll need to add exceptions for:

https://code.jquery.com/qunit/qunit-git.js
*.dynamics.com