(function () {
    // Uses an unsuported method to navigate to a webresource since there is no way to do this in the SDK yet.
    Develop1_RibbonCommands_navigateToWebResource = function (webresourceName) {
        var parameters = {
            uri: "$webresource:" + webresourceName
        };
        var navBar = window.top && window.top.document.getElementById("navBar");
        if (navBar == null || navBar.control == null || navBar.control.raiseNavigateRequest == null) return;
        navBar.control.raiseNavigateRequest(parameters);
    };
    Develop1_NavBar_AddButton = function (id, title, onclickJavascript, cssClass) {

       
        //window.top.$('#GQC').before('<span class="navTabButton" id="' + id + '" title="' + title + '"><a class="navTabButtonLink" onkeypress="return true;" onclick="' + onclickJavascript + '" href="javascript:;" unselectable="on"><span class="navTabButtonImageContainer" unselectable="on"><img id="' + cssClass + '" class="' + cssClass + '" alt="" unselectable="on" src="/_imgs/NavBar/Invisible.gif"></span></span></a></span>').fadeIn(2000);
        var button = $('<span class="navTabButton" style="display:none" id="' + id + '" title="' + title + '"><a class="navTabButtonLink" onkeypress="return true;" onclick="' + onclickJavascript + '" href="javascript:;" unselectable="on"><span class="navTabButtonImageContainer" unselectable="on"><img id="' + cssClass + '" class="' + cssClass + '" alt="" unselectable="on" src="/_imgs/NavBar/Invisible.gif"></span></span></a></span>');
        window.top.$('#GQC').before(button);
        button.fadeIn(1000);
    };

    Develop1_NavBar_AddGlobalSearch = function () {

        if (window.top.__addingGlobalSearch)
            return false;

        window.top.__addingGlobalSearch = true;

        var cssWebResource = "dev1_/css/MultiEntitySearch.css";
        var cssUrl = Xrm.Page.context.getClientUrl() + "/" + WEB_RESOURCE_ORG_VERSION_NUMBER + "/WebResources/" + cssWebResource;

        if (typeof (window.top.__multientityCssAdded) == 'undefined') { 
            window.top.__multientityCssAdded = true;
            var css_link = window.top.$("<link>", {
                rel: "stylesheet",
                type: "text/css",
                href: cssUrl
            });
            css_link.appendTo('head');
        }


        
        if (window.top.$('#dev1QSC').length == 0) {
            // Add the button 

            Develop1_NavBar_AddButton('dev1QSC', 'Search', "if (typeof(window.frames[0].Develop1_NavBar_NavigateToSearch)!='undefined') window.frames[0].Develop1_NavBar_NavigateToSearch()", 'dev1GlobalSearchImage');
        }


        
        window.top.__addingGlobalSearch = false;
        // Return false so we hide Button that calls this enable rule
        return false;
    };

    Develop1_NavBar_NavigateToSearch = function () {
       
        var url = 'dev1_/html/MultiEntitySearch2013.htm';
        Develop1_RibbonCommands_navigateToWebResource(url);

    };
})();