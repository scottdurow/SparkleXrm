Debugging:
1.Ensure that the CRM server is acccessible via localhost in the ServerUrl - ClientGlobalContext value
2.Add the fiddler script response for chrome debugging
3. Ensure UserID is set correctly
4.

Performance:
1. Why doesn't the grid automatically show column headers from metadata
a. This would involve parsing fetchxml multi-levels deep and downloading lots of metadata
b. The columns don't always want to be based on the meta data column display names


aims:
1. Enable code to be written in c# and compiled to javascript
2. Enable code to be shared between client and server
3. Provide a framework for Xrm UI's (this most painful bit!)

TODO:
1. Do we really need to create typed objects for entities -just for the static Logical Name member - or can we use partial classes to do this and the attributes be an imported class?
	-When saving - how do we hold null values v.s undefined
2. 

DONE: