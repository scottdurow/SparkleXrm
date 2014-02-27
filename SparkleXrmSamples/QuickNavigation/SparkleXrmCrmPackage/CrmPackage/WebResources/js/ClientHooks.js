//! ClientHooks.debug.js
//
waitForScripts("ribboncommands",["mscorlib","xrm"],
function () {



////////////////////////////////////////////////////////////////////////////////
// SiteMap

window.SiteMap = function SiteMap() {
    this.areas = [];
}
SiteMap.prototype = {
    url: null,
    privileges: null
}


////////////////////////////////////////////////////////////////////////////////
// SiteMapElement

window.SiteMapElement = function SiteMapElement() {
}
SiteMapElement.prototype = {
    title: null
}


////////////////////////////////////////////////////////////////////////////////
// Group

window.Group = function Group() {
    Group.initializeBase(this);
}
Group.prototype = {
    id: null,
    icon: null,
    subareas: null
}


////////////////////////////////////////////////////////////////////////////////
// Area

window.Area = function Area() {
    this.groups = [];
    Area.initializeBase(this);
}
Area.prototype = {
    id: null,
    icon: null
}


////////////////////////////////////////////////////////////////////////////////
// SubArea

window.SubArea = function SubArea() {
    SubArea.initializeBase(this);
}
SubArea.prototype = {
    id: null,
    icon: null,
    entity: null,
    objecttypecode: 0,
    url: null,
    privileges: null
}


Type.registerNamespace('QuickNavigation.ClientHooks');

////////////////////////////////////////////////////////////////////////////////
// QuickNavigation.ClientHooks.RetrieveUserPrivilegesRequest

QuickNavigation.ClientHooks.RetrieveUserPrivilegesRequest = function QuickNavigation_ClientHooks_RetrieveUserPrivilegesRequest() {
}
QuickNavigation.ClientHooks.RetrieveUserPrivilegesRequest.prototype = {
    userId: null,
    
    serialise: function QuickNavigation_ClientHooks_RetrieveUserPrivilegesRequest$serialise() {
        return '<request i:type="b:RetrieveUserPrivilegesRequest" xmlns:a="http://schemas.microsoft.com/xrm/2011/Contracts" xmlns:b="http://schemas.microsoft.com/crm/2011/Contracts">' + '        <a:Parameters xmlns:c="http://schemas.datacontract.org/2004/07/System.Collections.Generic">' + '          <a:KeyValuePairOfstringanyType>' + '            <c:key>UserId</c:key>' + '            <c:value i:type="d:guid" xmlns:d="http://schemas.microsoft.com/2003/10/Serialization/" >' + Xrm.Sdk.XmlHelper.encode(this.userId.value) + '</c:value>' + '          </a:KeyValuePairOfstringanyType>' + '        </a:Parameters>' + '        <a:RequestId i:nil="true" />' + '        <a:RequestName>RetrieveUserPrivileges</a:RequestName>' + '      </request>';
    }
}


////////////////////////////////////////////////////////////////////////////////
// QuickNavigation.ClientHooks.RetrieveUserPrivilegesResponse

QuickNavigation.ClientHooks.RetrieveUserPrivilegesResponse = function QuickNavigation_ClientHooks_RetrieveUserPrivilegesResponse(response) {
    var results = Xrm.Sdk.XmlHelper.selectSingleNode(response, 'Results');
    var $enum1 = ss.IEnumerator.getEnumerator(results.childNodes);
    while ($enum1.moveNext()) {
        var nameValuePair = $enum1.current;
        var key = Xrm.Sdk.XmlHelper.selectSingleNode(nameValuePair, 'key');
        if (Xrm.Sdk.XmlHelper.getNodeTextValue(key) === 'RolePrivileges') {
            var value = Xrm.Sdk.XmlHelper.selectSingleNode(nameValuePair, 'value');
            this.rolePrivileges = [];
            var $enum2 = ss.IEnumerator.getEnumerator(value.childNodes);
            while ($enum2.moveNext()) {
                var privNode = $enum2.current;
                var priv = new QuickNavigation.ClientHooks.RolePrivilege();
                priv.privilegeId = new Xrm.Sdk.Guid(Xrm.Sdk.XmlHelper.selectSingleNodeValue(privNode, 'PrivilegeId'));
                Xrm.ArrayEx.add(this.rolePrivileges, priv);
            }
        }
    }
}
QuickNavigation.ClientHooks.RetrieveUserPrivilegesResponse.prototype = {
    rolePrivileges: null
}


////////////////////////////////////////////////////////////////////////////////
// QuickNavigation.ClientHooks.RolePrivilege

QuickNavigation.ClientHooks.RolePrivilege = function QuickNavigation_ClientHooks_RolePrivilege() {
}
QuickNavigation.ClientHooks.RolePrivilege.prototype = {
    privilegeId: null
}


Type.registerNamespace('QuickNavigation.ClientHooks.Ribbon');

////////////////////////////////////////////////////////////////////////////////
// QuickNavigation.ClientHooks.Ribbon.Ribbon

QuickNavigation.ClientHooks.Ribbon.Ribbon = function QuickNavigation_ClientHooks_Ribbon_Ribbon() {
}
QuickNavigation.ClientHooks.Ribbon.Ribbon.populate = function QuickNavigation_ClientHooks_Ribbon_Ribbon$populate(commandProperties) {
    QuickNavigation.ClientHooks.Ribbon.Ribbon._resources = window.top._quickNav__resources;
    QuickNavigation.ClientHooks.Ribbon.Ribbon._userPriviledgeNames = window.top._quickNav__userPriviledgeNames;
    QuickNavigation.ClientHooks.Ribbon.Ribbon._siteMap = window.top._quickNav__siteMap;
    QuickNavigation.ClientHooks.Ribbon.Ribbon.uniquePrefix = 'dev1_' + Date.get_now().getTime().toString() + '|';
    var isOnForm = (Xrm.Page.ui != null);
    QuickNavigation.ClientHooks.Ribbon.Ribbon._loadResources();
    var quickNav = new Xrm.Sdk.Ribbon.RibbonMenu('dev1.QuickNav');
    var topSection = new Xrm.Sdk.Ribbon.RibbonMenuSection('dev1.SiteMapMenuSection', '', 2, 'Menu16');
    quickNav.addSection(topSection);
    if (Xrm.Page.context.client.getClient() === 'Web') {
        if (isOnForm) {
            var siteMapMenuFlyout = new Xrm.Sdk.Ribbon.RibbonFlyoutAnchor(QuickNavigation.ClientHooks.Ribbon.Ribbon.uniquePrefix + 'SiteMapButton', 1, QuickNavigation.ClientHooks.Ribbon.Ribbon._replaceResourceToken('$Site_Map'), 'Mscrm.Enabled', '/_imgs/FormEditorRibbon/Subgrid_16.png', null);
            topSection.addButton(siteMapMenuFlyout);
            siteMapMenuFlyout.menu = new Xrm.Sdk.Ribbon.RibbonMenu('dev1.SiteMapButton.Menu');
            var siteMapMenuFlyoutSection = new Xrm.Sdk.Ribbon.RibbonMenuSection(QuickNavigation.ClientHooks.Ribbon.Ribbon.uniquePrefix + 'SiteMapButton.Menu.Section', '', 1, 'Menu16');
            siteMapMenuFlyout.menu.addSection(siteMapMenuFlyoutSection);
            QuickNavigation.ClientHooks.Ribbon.Ribbon.getSiteMap(siteMapMenuFlyoutSection);
        }
        else {
            QuickNavigation.ClientHooks.Ribbon.Ribbon.getSiteMap(topSection);
        }
    }
    var advFind = new Xrm.Sdk.Ribbon.RibbonButton('dev1.OpenAdvancedFind.Button', 1, QuickNavigation.ClientHooks.Ribbon.Ribbon._replaceResourceToken('$Advanced_Find'), 'dev1.OpenAdvancedFind', '/_imgs/ribbon/AdvancedFind_16.png', null);
    topSection.addButton(advFind);
    QuickNavigation.ClientHooks.Ribbon.Ribbon._getFormTabs(quickNav);
    QuickNavigation.ClientHooks.Ribbon.Ribbon._getFormNav(quickNav);
    commandProperties.PopulationXML = quickNav.serialiseToRibbonXml();
    window.top._quickNav__resources=QuickNavigation.ClientHooks.Ribbon.Ribbon._resources;
    window.top._quickNav__userPriviledgeNames=QuickNavigation.ClientHooks.Ribbon.Ribbon._userPriviledgeNames;
    window.top._quickNav__siteMap=QuickNavigation.ClientHooks.Ribbon.Ribbon._siteMap;
}
QuickNavigation.ClientHooks.Ribbon.Ribbon._getFormNav = function QuickNavigation_ClientHooks_Ribbon_Ribbon$_getFormNav(quickNav) {
    QuickNavigation.ClientHooks.Ribbon.Ribbon.selecT_NAV_COMMAND_PREFIX = QuickNavigation.ClientHooks.Ribbon.Ribbon.uniquePrefix + 'QuickNav.Nav';
    if (Xrm.Page.ui != null && Xrm.Page.ui.navigation != null) {
        var navSection = new Xrm.Sdk.Ribbon.RibbonMenuSection(QuickNavigation.ClientHooks.Ribbon.Ribbon.uniquePrefix + 'QuickNav.Nav', 'Related', 1, 'Menu16');
        quickNav.addSection(navSection);
        var i = 0;
        Xrm.Page.ui.navigation.items.forEach(function(nav, index) {
            var button = new Xrm.Sdk.Ribbon.RibbonButton(QuickNavigation.ClientHooks.Ribbon.Ribbon.selecT_NAV_COMMAND_PREFIX + nav.getId(), i, nav.getLabel(), 'dev1.QuickNav.SelectNav', '/_imgs/FormEditorRibbon/Subgrid_16.png', '');
            navSection.addButton(button);
            i++;
            return true;
        });
    }
}
QuickNavigation.ClientHooks.Ribbon.Ribbon._getFormTabs = function QuickNavigation_ClientHooks_Ribbon_Ribbon$_getFormTabs(quickNav) {
    QuickNavigation.ClientHooks.Ribbon.Ribbon.selecT_TAB_COMMAND_PREFIX = QuickNavigation.ClientHooks.Ribbon.Ribbon.uniquePrefix + 'QuickNav.Tabs.';
    if (Xrm.Page.ui != null && Xrm.Page.ui.formSelector != null) {
        var form = Xrm.Page.ui.formSelector.getCurrentItem();
        var formSectionName = 'Form';
        if (form != null) {
            formSectionName = form.getLabel();
        }
        var tabsSection = new Xrm.Sdk.Ribbon.RibbonMenuSection(QuickNavigation.ClientHooks.Ribbon.Ribbon.uniquePrefix + 'QuickNav.Tabs', formSectionName, 1, 'Menu16');
        quickNav.addSection(tabsSection);
        var i = 0;
        Xrm.Page.ui.tabs.forEach(function(tab, index) {
            var button = new Xrm.Sdk.Ribbon.RibbonButton(QuickNavigation.ClientHooks.Ribbon.Ribbon.selecT_TAB_COMMAND_PREFIX + tab.getName(), i, tab.getLabel(), 'dev1.QuickNav.SelectTab', '/_imgs/FormEditorRibbon/Subgrid_16.png', '');
            tabsSection.addButton(button);
            i++;
            return true;
        });
    }
}
QuickNavigation.ClientHooks.Ribbon.Ribbon.getSiteMap = function QuickNavigation_ClientHooks_Ribbon_Ribbon$getSiteMap(siteMapSection) {
    QuickNavigation.ClientHooks.Ribbon.Ribbon._getUserPrivs();
    var sequence = 1;
    var $enum1 = ss.IEnumerator.getEnumerator(QuickNavigation.ClientHooks.Ribbon.Ribbon._siteMap.areas);
    while ($enum1.moveNext()) {
        var area = $enum1.current;
        var areaIconUrl = '/_imgs/FormEditorRibbon/Subgrid_16.png';
        if (!String.isNullOrEmpty(area.icon)) {
            areaIconUrl = QuickNavigation.ClientHooks.Ribbon.Ribbon._decodeImageUrl(area.icon);
        }
        var areaFlyout = new Xrm.Sdk.Ribbon.RibbonFlyoutAnchor(QuickNavigation.ClientHooks.Ribbon.Ribbon.uniquePrefix + area.id + '.Flyout', sequence++, QuickNavigation.ClientHooks.Ribbon.Ribbon._replaceResourceToken(area.title), 'Mscrm.Enabled', areaIconUrl, null);
        areaFlyout.menu = new Xrm.Sdk.Ribbon.RibbonMenu(area.id + '.Flyout.Menu');
        siteMapSection.addButton(areaFlyout);
        var areaSection = null;
        var $enum2 = ss.IEnumerator.getEnumerator(area.groups);
        while ($enum2.moveNext()) {
            var group = $enum2.current;
            var subAreaMenuSection = null;
            if (group.title != null) {
                areaSection = new Xrm.Sdk.Ribbon.RibbonMenuSection(QuickNavigation.ClientHooks.Ribbon.Ribbon.uniquePrefix + area.id + '|' + group.id + '.Section', QuickNavigation.ClientHooks.Ribbon.Ribbon._replaceResourceToken(group.title), sequence++, 'Menu16');
                areaFlyout.menu.addSection(areaSection);
                subAreaMenuSection = areaSection;
            }
            else {
                if (areaSection == null) {
                    areaSection = new Xrm.Sdk.Ribbon.RibbonMenuSection(QuickNavigation.ClientHooks.Ribbon.Ribbon.uniquePrefix + area.id + '.Section', QuickNavigation.ClientHooks.Ribbon.Ribbon._replaceResourceToken(area.title), sequence++, 'Menu16');
                    areaFlyout.menu.addSection(areaSection);
                }
                subAreaMenuSection = areaSection;
            }
            var subAreaSequence = 1;
            var $enum3 = ss.IEnumerator.getEnumerator(group.subareas);
            while ($enum3.moveNext()) {
                var subArea = $enum3.current;
                var subAreaIconUrl = '/_imgs/FormEditorRibbon/Subgrid_16.png';
                if (!String.isNullOrEmpty(subArea.icon)) {
                    subAreaIconUrl = QuickNavigation.ClientHooks.Ribbon.Ribbon._decodeImageUrl(subArea.icon);
                }
                else if (!String.isNullOrEmpty(subArea.entity) && subArea.objecttypecode !== 4703 && subArea.objecttypecode !== 9603) {
                    subAreaIconUrl = '/_imgs/ico_16_' + subArea.objecttypecode.toString() + '.gif';
                }
                var hasAccess = true;
                if (subArea.privileges != null && subArea.privileges.length > 0) {
                    var $enum4 = ss.IEnumerator.getEnumerator(subArea.privileges);
                    while ($enum4.moveNext()) {
                        var priv = $enum4.current;
                        hasAccess = hasAccess && Object.keyExists(QuickNavigation.ClientHooks.Ribbon.Ribbon._userPriviledgeNames, priv.toLowerCase());
                        if (!hasAccess) {
                            break;
                        }
                    }
                }
                if (hasAccess) {
                    var button = new Xrm.Sdk.Ribbon.RibbonButton(QuickNavigation.ClientHooks.Ribbon.Ribbon.uniquePrefix + area.id + '|' + group.id + '|' + subArea.id, subAreaSequence++, QuickNavigation.ClientHooks.Ribbon.Ribbon._replaceResourceToken(subArea.title), 'dev1.QuickNav.SelectSiteMapNav', subAreaIconUrl, '');
                    subAreaMenuSection.addButton(button);
                }
            }
        }
    }
}
QuickNavigation.ClientHooks.Ribbon.Ribbon._loadResources = function QuickNavigation_ClientHooks_Ribbon_Ribbon$_loadResources() {
    var options = {};
    options.async = false;
    var lcid = Xrm.Page.context.getUserLcid();
    if (QuickNavigation.ClientHooks.Ribbon.Ribbon._siteMap == null) {
        options.url = Xrm.Page.context.getClientUrl() + '/' + Xrm.PageEx.getCacheKey() + '/WebResources/dev1_/js/SiteMap' + lcid.toString() + '.js';
        QuickNavigation.ClientHooks.Ribbon.Ribbon._siteMap = $.parseJSON($.ajax(options).responseText);
    }
    if (QuickNavigation.ClientHooks.Ribbon.Ribbon._resources == null) {
        var loadedResources = null;
        options.url = QuickNavigation.ClientHooks.Ribbon.Ribbon._getResourceStringWebResourceUrl(lcid);
        try {
            loadedResources = $.parseJSON($.ajax(options).responseText);
        }
        catch ($e1) {
        }
        if (loadedResources == null) {
            options.url = QuickNavigation.ClientHooks.Ribbon.Ribbon._getResourceStringWebResourceUrl(1033);
            loadedResources = $.parseJSON($.ajax(options).responseText);
        }
        QuickNavigation.ClientHooks.Ribbon.Ribbon._resources = loadedResources;
    }
}
QuickNavigation.ClientHooks.Ribbon.Ribbon._getResourceStringWebResourceUrl = function QuickNavigation_ClientHooks_Ribbon_Ribbon$_getResourceStringWebResourceUrl(lcid) {
    return Xrm.Page.context.getClientUrl() + '/' + Xrm.PageEx.getCacheKey() + '/WebResources/dev1_/js/QuickNavigationResources_' + lcid.toString() + '.js';
}
QuickNavigation.ClientHooks.Ribbon.Ribbon._replaceResourceToken = function QuickNavigation_ClientHooks_Ribbon_Ribbon$_replaceResourceToken(title) {
    if (title == null) {
        title = '';
    }
    else if (title.startsWith('$')) {
        title = title.replaceAll('$', '');
        if (Object.keyExists(QuickNavigation.ClientHooks.Ribbon.Ribbon._resources, title)) {
            title = QuickNavigation.ClientHooks.Ribbon.Ribbon._resources[title];
        }
    }
    return title;
}
QuickNavigation.ClientHooks.Ribbon.Ribbon._decodeImageUrl = function QuickNavigation_ClientHooks_Ribbon_Ribbon$_decodeImageUrl(url) {
    if (url.toLowerCase().startsWith('$webresource:')) {
        url = Xrm.PageEx.getCacheKey() + '/WebResources/' + url.substr(13);
    }
    return url;
}
QuickNavigation.ClientHooks.Ribbon.Ribbon._getUserPrivs = function QuickNavigation_ClientHooks_Ribbon_Ribbon$_getUserPrivs() {
    if (QuickNavigation.ClientHooks.Ribbon.Ribbon._userPriviledgeNames == null) {
        Xrm.Sdk.OrganizationServiceProxy.registerExecuteMessageResponseType('RetrieveUserPrivileges', QuickNavigation.ClientHooks.RetrieveUserPrivilegesResponse);
        var request = new QuickNavigation.ClientHooks.RetrieveUserPrivilegesRequest();
        request.userId = new Xrm.Sdk.Guid(Xrm.Page.context.getUserId());
        var response = Xrm.Sdk.OrganizationServiceProxy.execute(request);
        var priviledgeFetchXml = "<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>\r\n                                            <entity name='privilege'>\r\n                                            <attribute name='name'/>\r\n                                            <filter type='and'>\r\n                                            <condition attribute='privilegeid' operator='in'>\r\n                                            {0}\r\n                                            </condition>\r\n                                            <condition attribute='name' operator='in'>\r\n                                            {1}\r\n                                            </condition>\r\n                                            </filter>\r\n                                            </entity>\r\n                                            </fetch>";
        var priviledgeIds = '';
        var $enum1 = ss.IEnumerator.getEnumerator(response.rolePrivileges);
        while ($enum1.moveNext()) {
            var p = $enum1.current;
            priviledgeIds += '<value>' + p.privilegeId.value + '</value>';
        }
        var priviledgeNames = '';
        var $enum2 = ss.IEnumerator.getEnumerator(QuickNavigation.ClientHooks.Ribbon.Ribbon._siteMap.privileges);
        while ($enum2.moveNext()) {
            var priv = $enum2.current;
            priviledgeNames += '<value>' + priv + '</value>';
        }
        var userPrivNameResults = Xrm.Sdk.OrganizationServiceProxy.retrieveMultiple(String.format(priviledgeFetchXml, priviledgeIds, priviledgeNames));
        QuickNavigation.ClientHooks.Ribbon.Ribbon._userPriviledgeNames = {};
        var $enum3 = ss.IEnumerator.getEnumerator(userPrivNameResults.get_entities());
        while ($enum3.moveNext()) {
            var priv = $enum3.current;
            QuickNavigation.ClientHooks.Ribbon.Ribbon._userPriviledgeNames[priv.getAttributeValueString('name').toLowerCase()] = '1';
        }
    }
}
QuickNavigation.ClientHooks.Ribbon.Ribbon.selectTab = function QuickNavigation_ClientHooks_Ribbon_Ribbon$selectTab(commandProperties) {
    var tabId = commandProperties.SourceControlId.replaceAll(QuickNavigation.ClientHooks.Ribbon.Ribbon.selecT_TAB_COMMAND_PREFIX, '');
    var tab = window.parent.parent.document.getElementById('TabNode_tab0Tab-main');
    if (tab != null) {
        tab.click();
    }
    var tabControl = Xrm.Page.ui.tabs.get(tabId);
    if (tabControl.getDisplayState() === 'collapsed') {
        tabControl.setDisplayState('expanded');
    }
    tabControl.setFocus();
}
QuickNavigation.ClientHooks.Ribbon.Ribbon.selectNav = function QuickNavigation_ClientHooks_Ribbon_Ribbon$selectNav(commandProperties) {
    var navId = commandProperties.SourceControlId.replaceAll(QuickNavigation.ClientHooks.Ribbon.Ribbon.selecT_NAV_COMMAND_PREFIX, '');
    var navItem = Xrm.Page.ui.navigation.items.get(navId);
    navItem.setFocus();
}
QuickNavigation.ClientHooks.Ribbon.Ribbon.selectSiteMapNav = function QuickNavigation_ClientHooks_Ribbon_Ribbon$selectSiteMapNav(commandProperties) {
    var navId = commandProperties.SourceControlId.split('|');
    var areaId = navId[1];
    var groupId = navId[2];
    var subAreaId = navId[3];
    var $enum1 = ss.IEnumerator.getEnumerator(QuickNavigation.ClientHooks.Ribbon.Ribbon._siteMap.areas);
    while ($enum1.moveNext()) {
        var area = $enum1.current;
        if (area.id === areaId) {
            var $enum2 = ss.IEnumerator.getEnumerator(area.groups);
            while ($enum2.moveNext()) {
                var group = $enum2.current;
                if (group.id === groupId) {
                    var $enum3 = ss.IEnumerator.getEnumerator(group.subareas);
                    while ($enum3.moveNext()) {
                        var subArea = $enum3.current;
                        if (subArea.id === subAreaId) {
                            var url = subArea.url;
                            var siteMapPath = 'sitemappath=' + encodeURIComponent(areaId + '|' + groupId + '|' + subAreaId);
                            if (url == null) {
                                var objectypecode = subArea.objecttypecode;
                                url = '/_root/homepage.aspx?etc=' + objectypecode + '&' + siteMapPath;
                            }
                            else if (url.indexOf('?') > -1) {
                                url = url + '&' + siteMapPath;
                            }
                            else {
                                url = url + '?' + siteMapPath;
                            }
                            var navBar = window.top.document.getElementById('navBar');
                            if (navBar != null) {
                                if (navBar.control.raiseNavigateRequest != null) {
                                    var urlParameter = {};
                                    urlParameter['uri'] = url;
                                    navBar.control.raiseNavigateRequest(urlParameter);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}


SiteMap.registerClass('SiteMap');
SiteMapElement.registerClass('SiteMapElement');
Group.registerClass('Group', SiteMapElement);
Area.registerClass('Area', SiteMapElement);
SubArea.registerClass('SubArea', SiteMapElement);
QuickNavigation.ClientHooks.RetrieveUserPrivilegesRequest.registerClass('QuickNavigation.ClientHooks.RetrieveUserPrivilegesRequest', null, Object);
QuickNavigation.ClientHooks.RetrieveUserPrivilegesResponse.registerClass('QuickNavigation.ClientHooks.RetrieveUserPrivilegesResponse', null, Object);
QuickNavigation.ClientHooks.RolePrivilege.registerClass('QuickNavigation.ClientHooks.RolePrivilege');
QuickNavigation.ClientHooks.Ribbon.Ribbon.registerClass('QuickNavigation.ClientHooks.Ribbon.Ribbon');
QuickNavigation.ClientHooks.Ribbon.Ribbon.selecT_TAB_COMMAND_PREFIX = 'dev1.QuickNav.Tabs.';
QuickNavigation.ClientHooks.Ribbon.Ribbon.selecT_NAV_COMMAND_PREFIX = 'dev1.QuickNav.Nav.';
QuickNavigation.ClientHooks.Ribbon.Ribbon._resources = null;
QuickNavigation.ClientHooks.Ribbon.Ribbon._userPriviledgeNames = null;
QuickNavigation.ClientHooks.Ribbon.Ribbon._siteMap = null;
QuickNavigation.ClientHooks.Ribbon.Ribbon.uniquePrefix = null;
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
