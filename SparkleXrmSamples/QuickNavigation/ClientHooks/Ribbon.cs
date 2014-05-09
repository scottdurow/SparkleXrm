
using jQueryApi;
using System;
using System.Collections.Generic;
using System.Html;
using Xrm;
using Xrm.Sdk;
using Xrm.Sdk.Ribbon;
using Xrm.XrmImport.Ribbon;



namespace QuickNavigation.ClientHooks.Ribbon
{

    public class Ribbon
    {
        public static string SELECT_TAB_COMMAND_PREFIX = "dev1.QuickNav.Tabs.";
        public static string SELECT_NAV_COMMAND_PREFIX = "dev1.QuickNav.Nav.";
        public static Dictionary<string, string> _resources;
        public static Dictionary<string, string> _userPriviledgeNames;
        public static SiteMap _siteMap;
        public static string uniquePrefix;
        public static void Populate(CommandProperties commandProperties)
        {
            
           

            _resources = (Dictionary<string, string>)Script.Literal("window.top._quickNav__resources");
            _userPriviledgeNames = (Dictionary<string, string>)Script.Literal("window.top._quickNav__userPriviledgeNames");
            _siteMap = (SiteMap)Script.Literal("window.top._quickNav__siteMap");

            uniquePrefix = "dev1_" + DateTime.Now.GetTime().ToString() + "|";
            bool isOnForm = (Page.Ui != null);

            LoadResources();

            RibbonMenu quickNav = new RibbonMenu("dev1.QuickNav");
            RibbonMenuSection topSection = new RibbonMenuSection("dev1.SiteMapMenuSection", "", 2, RibbonDisplayMode.Menu16);
            quickNav.AddSection(topSection);

            // Only show Sitemap in web client
            if (Page.Context.Client.GetClient() == ClientType.Web)
            {
                if (isOnForm)
                {
                    RibbonFlyoutAnchor siteMapMenuFlyout = new RibbonFlyoutAnchor(uniquePrefix + "SiteMapButton", 1, ReplaceResourceToken("$Site_Map"), "Mscrm.Enabled", "/_imgs/FormEditorRibbon/Subgrid_16.png", null);
                    topSection.AddButton((RibbonButton)(object)siteMapMenuFlyout);

                    siteMapMenuFlyout.Menu = new RibbonMenu("dev1.SiteMapButton.Menu");

                    RibbonMenuSection siteMapMenuFlyoutSection = new RibbonMenuSection(uniquePrefix + "SiteMapButton.Menu.Section", "", 1, RibbonDisplayMode.Menu16);
                    siteMapMenuFlyout.Menu.AddSection(siteMapMenuFlyoutSection);
                    GetSiteMap(siteMapMenuFlyoutSection);
                }
                else
                    GetSiteMap(topSection);
            }

            // Add Advanced Find
            RibbonButton advFind = new RibbonButton("dev1.OpenAdvancedFind.Button", 1, ReplaceResourceToken("$Advanced_Find"), "dev1.OpenAdvancedFind", "/_imgs/ribbon/AdvancedFind_16.png", null);
            topSection.AddButton(advFind);

            GetFormTabs(quickNav);

            GetFormNav(quickNav);

            commandProperties.PopulationXML = quickNav.SerialiseToRibbonXml();

            // Store for next time
            Script.Literal("window.top._quickNav__resources={0}",_resources);
            Script.Literal("window.top._quickNav__userPriviledgeNames={0}",_userPriviledgeNames);
            Script.Literal("window.top._quickNav__siteMap={0}",_siteMap);
        }

        private static void GetFormNav(RibbonMenu quickNav)
        {
            SELECT_NAV_COMMAND_PREFIX = uniquePrefix + "QuickNav.Nav";
            if (Page.Ui != null && Page.Ui.Navigation != null)
            {
                RibbonMenuSection navSection = new RibbonMenuSection(uniquePrefix + "QuickNav.Nav", "Related", 1, RibbonDisplayMode.Menu16);
                quickNav.AddSection(navSection);
                int i = 0;
                Page.Ui.Navigation.Items.ForEach(delegate(NavigationItem nav, int index)
                {
                    RibbonButton button = new RibbonButton(SELECT_NAV_COMMAND_PREFIX + nav.GetId(), i, nav.GetLabel(), "dev1.QuickNav.SelectNav", "/_imgs/FormEditorRibbon/Subgrid_16.png", "");
                    navSection.AddButton(button);
                    
                    i++;
                    return true;
                });
            }
        }

        private static void GetFormTabs(RibbonMenu quickNav)
        {
            SELECT_TAB_COMMAND_PREFIX = uniquePrefix + "QuickNav.Tabs.";
            if (Page.Ui!=null && Page.Ui.FormSelector != null)
            {

                FormSelectorItem form = Page.Ui.FormSelector.GetCurrentItem();
                string formSectionName = "Form";
                if (form != null)
                {
                    formSectionName = form.GetLabel();
                }

                RibbonMenuSection tabsSection = new RibbonMenuSection(uniquePrefix + "QuickNav.Tabs", formSectionName, 1, RibbonDisplayMode.Menu16);
                quickNav.AddSection(tabsSection);
                int i = 0;
                // Get the tabs and sections on the form
                Page.Ui.Tabs.ForEach(delegate(TabItem tab, int index)
                {
                    if (tab.GetVisible())
                    {
                        RibbonButton button = new RibbonButton(SELECT_TAB_COMMAND_PREFIX + tab.GetName(), i, tab.GetLabel(), "dev1.QuickNav.SelectTab", "/_imgs/FormEditorRibbon/Subgrid_16.png", "");
                        tabsSection.AddButton(button);
                        i++;
                    }
                    return true;
                });
            }
        }

        public static void GetSiteMap(RibbonMenuSection siteMapSection)
        {
            
            GetUserPrivs();

            //jQuery.FromHtml("<style>.commandBar-NavButton{background-color:red}</style>").AppendTo("head");



            int sequence = 1;
            foreach (Area area in _siteMap.areas)
            {
                string areaIconUrl = "/_imgs/FormEditorRibbon/Subgrid_16.png";
                if (!String.IsNullOrEmpty(area.icon))
                {
                    areaIconUrl = DecodeImageUrl(area.icon);
                }
                RibbonFlyoutAnchor areaFlyout = new RibbonFlyoutAnchor(uniquePrefix + area.id + ".Flyout", sequence++, ReplaceResourceToken(area.title), "Mscrm.Enabled", areaIconUrl, null);
                areaFlyout.Menu = new RibbonMenu(area.id + ".Flyout.Menu");
                //areaFlyout.Image16by16Class = @"commandBar-NavButton"; 
                
                //menu.AddSection(areaSection);
                siteMapSection.AddButton((RibbonButton)(object)areaFlyout);
                RibbonMenuSection areaSection=null;

                foreach (Group group in area.groups)
                {
                    RibbonMenuSection subAreaMenuSection = null;
                    RibbonMenu subAreaParentMenu = null;
                    


                    if (group.title != null)
                    {
                    
                        areaSection = new RibbonMenuSection(uniquePrefix + area.id +"|" + group.id + ".Section", ReplaceResourceToken(group.title), sequence++, RibbonDisplayMode.Menu16);
                        
                        subAreaParentMenu = areaFlyout.Menu;
                        subAreaMenuSection = areaSection;
                    }
                    else
                    {
                        if (areaSection == null)
                        {
                            // Create default areaSection because we don't have group titles
                            areaSection = new RibbonMenuSection(uniquePrefix + area.id + ".Section", ReplaceResourceToken(area.title), sequence++, RibbonDisplayMode.Menu16);
                           
                            subAreaParentMenu = areaFlyout.Menu;
                        }
                        subAreaMenuSection = areaSection;
                    }
    
                    int subAreaSequence = 1;

                    foreach (SubArea subArea in group.subareas)
                    {
                        string subAreaIconUrl = "/_imgs/FormEditorRibbon/Subgrid_16.png";
                        if (!String.IsNullOrEmpty(subArea.icon))
                        {
                            subAreaIconUrl = DecodeImageUrl(subArea.icon);
                        }
                        else if (!string.IsNullOrEmpty(subArea.entity) && subArea.objecttypecode != 4703 && subArea.objecttypecode != 9603)
                        {
                            subAreaIconUrl = "/_imgs/ico_16_" + subArea.objecttypecode.ToString() + ".gif";

                        }


                        bool hasAccess = true;
                        // Check Privs
                        if (subArea.privileges!=null && subArea.privileges.Count > 0)
                        {
                            foreach (string priv in subArea.privileges)
                            {
                                hasAccess = hasAccess && _userPriviledgeNames.ContainsKey(priv.ToLowerCase());
                                if (!hasAccess)
                                    break;
                            }

                        }
                        if (hasAccess)
                        {
                            RibbonButton button = new RibbonButton(uniquePrefix + area.id + "|" + group.id + "|" + subArea.id, subAreaSequence++, ReplaceResourceToken(subArea.title), "dev1.QuickNav.SelectSiteMapNav", subAreaIconUrl, "");
                            subAreaMenuSection.AddButton(button);
                        }
                    }
                    // Add submenu to menu if it has at least one item
                    if (subAreaMenuSection.Buttons.Count > 0)
                    {
                        subAreaParentMenu.AddSection(subAreaMenuSection);
                    }
                    
                }

            }
        }

        private static void LoadResources()
        {
            jQueryAjaxOptions options = new jQueryAjaxOptions();
            options.Async = false;
            int lcid = Page.Context.GetUserLcid();
           
            if (_siteMap == null)
            {
                options.Url = Page.Context.GetClientUrl() + "/" + PageEx.GetCacheKey() + "/WebResources/dev1_/js/SiteMap" + lcid.ToString() + ".js";
                _siteMap = jQuery.ParseJsonData<SiteMap>(jQuery.Ajax(options).ResponseText);
            }

            if (_resources == null)
            {
                Dictionary<string, string> loadedResources = null;
                options.Url =  GetResourceStringWebResourceUrl(lcid);
                try
                {
                    loadedResources = jQuery.ParseJsonData<Dictionary<string, string>>(jQuery.Ajax(options).ResponseText);
                }
                catch { }

                if (loadedResources == null)
                {
                    // We fall back to the 1033 because this is included in the solution
                    options.Url = GetResourceStringWebResourceUrl(1033);
                    loadedResources = jQuery.ParseJsonData<Dictionary<string, string>>(jQuery.Ajax(options).ResponseText);
                }
                _resources = loadedResources;
            }
        }
        private static string GetResourceStringWebResourceUrl(int lcid)
        {
            return Page.Context.GetClientUrl() + "/" + PageEx.GetCacheKey() + "/WebResources/dev1_/js/QuickNavigationResources_" + lcid.ToString() + ".js";
        }
        private static string ReplaceResourceToken(string title)
        {
            if (title == null)
                title = "";
            else if (title.StartsWith("$"))
            {
                // Get the resource string if it's there
                title = title.Replace("$", "");
                if (_resources.ContainsKey(title))
                    title = _resources[title];
            }
            return title;
        }
        private static string DecodeImageUrl(string url)
        {
            if (url.ToLowerCase().StartsWith("$webresource:"))
            {
                url = Page.Context.GetClientUrl() + "/" + PageEx.GetCacheKey() + "WebResources/" +  url.Substr(13);
            }
            return url;
        }
        private static void GetUserPrivs()
        {
            if (_userPriviledgeNames == null)
            {
                // Get the Users' Privileges
                OrganizationServiceProxy.RegisterExecuteMessageResponseType("RetrieveUserPrivileges", typeof(RetrieveUserPrivilegesResponse));
                RetrieveUserPrivilegesRequest request = new RetrieveUserPrivilegesRequest();
                request.UserId = new Guid(Page.Context.GetUserId());

                RetrieveUserPrivilegesResponse response = (RetrieveUserPrivilegesResponse)OrganizationServiceProxy.Execute(request);

                // Translate into names
                string priviledgeFetchXml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                            <entity name='privilege'>
                                            <attribute name='name'/>
                                            <filter type='and'>
                                            <condition attribute='privilegeid' operator='in'>
                                            {0}
                                            </condition>
                                            <condition attribute='name' operator='in'>
                                            {1}
                                            </condition>
                                            </filter>
                                            </entity>
                                            </fetch>";

                string priviledgeIds = "";
                // Load the names of the privs where the user has them in their roles
                foreach (RolePrivilege p in response.RolePrivileges)
                {
                    priviledgeIds += @"<value>" + p.PrivilegeId.Value + "</value>";
                }
                // Load only the names/ids where we need to compare in the sitemap
                string priviledgeNames = "";
                foreach (string priv in _siteMap.privileges)
                {
                    priviledgeNames += @"<value>" + priv + "</value>";
                }

                EntityCollection userPrivNameResults = OrganizationServiceProxy.RetrieveMultiple(string.Format(priviledgeFetchXml, priviledgeIds, priviledgeNames));
                _userPriviledgeNames = new Dictionary<string, string>();
                foreach (Entity priv in userPrivNameResults.Entities)
                {
                    _userPriviledgeNames[priv.GetAttributeValueString("name").ToLowerCase()] = "1";

                }
            }
        }
        public static void SelectTab(CommandProperties commandProperties)
        {
            string tabId = commandProperties.SourceControlId.Replace(SELECT_TAB_COMMAND_PREFIX, "");
            Element tab = Window.Parent.Parent.Document.GetElementById("TabNode_tab0Tab-main");
            // Move back to the main form
            if (tab != null)
                tab.Click();

            TabItem tabControl = Page.Ui.Tabs.Get(tabId);
            if (tabControl.GetDisplayState() == DisplayState.Collapsed)
            {
                tabControl.SetDisplayState(DisplayState.Expanded);
            }
            tabControl.SetFocus();



        }

        public static void SelectNav(CommandProperties commandProperties)
        {
            string navId = commandProperties.SourceControlId.Replace(SELECT_NAV_COMMAND_PREFIX, "");
            NavigationItem navItem = Page.Ui.Navigation.Items.Get(navId);
            navItem.SetFocus();

        }

        public static void SelectSiteMapNav(CommandProperties commandProperties)
        {
            string[] navId = commandProperties.SourceControlId.Split("|");
            string areaId = navId[1];
            string groupId = navId[2];
            string subAreaId = navId[3];

            foreach (Area area in _siteMap.areas)
            {
                if (area.id == areaId)
                {
                    foreach (Group group in area.groups)
                    {
                        if (group.id == groupId)
                        {
                            foreach (SubArea subArea in group.subareas)
                            {
                                if (subArea.id == subAreaId)
                                {
                                    // Navigate to sub area url
                                    string url = subArea.url;
                                    string siteMapPath = "sitemappath=" + GlobalFunctions.encodeURIComponent(areaId + "|" + groupId + "|" + subAreaId);
                                    if (url == null)
                                    {
                                        // Entity url
                                        int objectypecode = subArea.objecttypecode;
                                        url = "/_root/homepage.aspx?etc=" + objectypecode + "&" + siteMapPath;
                                    }
                                    else if (url.IndexOf("?") > -1)
                                    {
                                        url = url + "&" + siteMapPath;
                                    }
                                    else
                                    {
                                        url = url + "?" + siteMapPath;
                                    }

                                    Element navBar = Window.Top.Document.GetElementById("navBar");
                                    if (navBar != null)
                                    {
                                        if (Script.Literal("{0}.control.raiseNavigateRequest",navBar) != null)
                                        {
                                            Dictionary<string, string> urlParameter = new Dictionary<string, string>();
                                            urlParameter["uri"] = url;
                                            Script.Literal("{0}.control.raiseNavigateRequest({1})", navBar, urlParameter);

                                        }

                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}

