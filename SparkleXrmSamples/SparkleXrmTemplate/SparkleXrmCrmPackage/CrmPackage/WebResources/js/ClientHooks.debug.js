//! ClientHooks.debug.js
//
waitForScripts("ribboncommands",["mscorlib","xrm"],
function () {



Type.registerNamespace('ClientHooks');

////////////////////////////////////////////////////////////////////////////////
// ClientHooks.Class1

ClientHooks.Class1 = function ClientHooks_Class1() {
}


ClientHooks.Class1.registerClass('ClientHooks.Class1');
});


function waitForScripts(name, scriptNames, callback) {
    var hasLoaded = false;
    window._loadedScripts = window._loadedScripts || [];
    function checkScripts() {
        var allLoaded = true;
        for (var i = 0; i < scriptNames.length; i++) {
            var hasLoaded = true;
            var script = scriptNames[i];
            switch (script) {
                case "mscorlib":
                    hasLoaded = typeof (window.ss) != "undefined";
                    break;
                case "jquery":
                    hasLoaded = typeof (window.jQuery) != "undefined";
                    break;
				 case "jquery-ui":
                    hasLoaded = typeof (window.xrmjQuery.ui) != "undefined";
                    break;
                default:
                    hasLoaded = window._loadedScripts[script];
                    break;
            }

            allLoaded = allLoaded && hasLoaded;
            if (!allLoaded) {
                setTimeout(checkScripts, 10);
                break;
            }
        }

        if (allLoaded) {
            callback();
            window._loadedScripts[name] = true;
        }
    }
   
	checkScripts();
	
}
