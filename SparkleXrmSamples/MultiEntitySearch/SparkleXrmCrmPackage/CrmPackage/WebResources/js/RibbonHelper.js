(function () {
    // Uses an unsuported method to navigate to a webresource since there is no way to do this in the SDK yet.
    Develop1_RibbonCommands_navigateToWebResource = function (webresourceName) {
        var parameters = {
            uri: "$webresource:" + webresourceName
        };
        var navBar = window.top && window.top.document.getElementById("navBar");
        if (navBar == null || navBar.control == null || navBar.control.raiseNavigateRequest == null) return;
        navBar.control.raiseNavigateRequest(parameters);
    }
})();