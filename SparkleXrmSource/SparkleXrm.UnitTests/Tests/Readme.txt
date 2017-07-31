To run unit tests:
1. Install Fiddler
2. Enable AutoResponder
3. Add the autoresponder:
	regex:(?insx).+/WebResources/sparkle_/unittests/(?'fname'[^?]*).*
	<repo location>\SparkleXRM\SparkleXrmSource\SparkleXrm.UnitTests\Tests\${fname}