//! ClientHooks.debug.js
//
var scriptLoader = scriptLoader || {
    delayedLoads: [],
    load: function (name, requires, script) {
        window._loadedScripts = window._loadedScripts || {};
        // Check for loaded scripts, if not all loaded then register delayed Load
        if (requires == null || requires.length == 0 || scriptLoader.areLoaded(requires)) {
            scriptLoader.runScript(name, script);
        }
        else {
            // Register an onload check
            scriptLoader.delayedLoads.push({ name: name, requires: requires, script: script });
        }
    },
    runScript: function (name, script) {      
        script.call(window);
        window._loadedScripts[name] = true;
        scriptLoader.onScriptLoaded(name);
    },
    onScriptLoaded: function (name) {
        // Check for any registered delayed Loads
        scriptLoader.delayedLoads.forEach(function (script) {
            if (script.loaded == null && scriptLoader.areLoaded(script.requires)) {
                script.loaded = true;
                scriptLoader.runScript(script.name, script.script);
            }
        });
    },
    areLoaded: function (requires) {
        var allLoaded = true;
        for (var i = 0; i < requires.length; i++) {
			var isLoaded = (window._loadedScripts[requires[i]] != null);
            allLoaded = allLoaded && isLoaded;
            if (!allLoaded)
                break;
        }
        return allLoaded;
    }
};
 
scriptLoader.load("clienthooks", ["mscorlib","xrm"], function () {


////////////////////////////////////////////////////////////////////////////////
// dev1_session_StatusCode

window.dev1_session_StatusCode = function() { 
    /// <field name="draft" type="Number" integer="true" static="true">
    /// </field>
};
dev1_session_StatusCode.prototype = {
    draft: 1
}
dev1_session_StatusCode.registerEnum('dev1_session_StatusCode', false);


////////////////////////////////////////////////////////////////////////////////
// ActivityPointer

window.ActivityPointer = function ActivityPointer() {
    /// <field name="entityLogicalName" type="String" static="true">
    /// </field>
    /// <field name="entityTypeCode" type="Number" integer="true" static="true">
    /// </field>
    /// <field name="activityid" type="String">
    /// </field>
    /// <field name="subject" type="String">
    /// </field>
    /// <field name="activitytypecode" type="String">
    /// </field>
    /// <field name="regardingobjectid" type="Xrm.Sdk.EntityReference">
    /// </field>
    /// <field name="displaySubject" type="String">
    /// </field>
    ActivityPointer.initializeBase(this, [ 'activitypointer' ]);
}
ActivityPointer.prototype = {
    activityid: null,
    subject: null,
    activitytypecode: null,
    regardingobjectid: null,
    displaySubject: null,
    
    _updateCalculatedFields: function ActivityPointer$_updateCalculatedFields() {
        this.logicalName = this.activitytypecode;
    }
}


////////////////////////////////////////////////////////////////////////////////
// dev1_session

window.dev1_session = function dev1_session() {
    /// <field name="entityLogicalName" type="String" static="true">
    /// </field>
    /// <field name="entityTypeCode" type="Number" integer="true" static="true">
    /// </field>
    /// <field name="createdon" type="Date">
    /// </field>
    /// <field name="dev1_description" type="String">
    /// </field>
    /// <field name="dev1_duration" type="Nullable`1">
    /// </field>
    /// <field name="dev1_activityid" type="String">
    /// </field>
    /// <field name="dev1_activitytypename" type="String">
    /// </field>
    /// <field name="dev1_emailid" type="Xrm.Sdk.EntityReference">
    /// </field>
    /// <field name="dev1_phonecallid" type="Xrm.Sdk.EntityReference">
    /// </field>
    /// <field name="dev1_letterid" type="Xrm.Sdk.EntityReference">
    /// </field>
    /// <field name="dev1_taskid" type="Xrm.Sdk.EntityReference">
    /// </field>
    /// <field name="dev1_endtime" type="Date">
    /// </field>
    /// <field name="dev1_row" type="Nullable`1">
    /// </field>
    /// <field name="statuscode" type="Xrm.Sdk.OptionSetValue">
    /// </field>
    /// <field name="dev1_sessionid" type="Xrm.Sdk.Guid">
    /// </field>
    /// <field name="dev1_starttime" type="Date">
    /// </field>
    /// <field name="contract_customerid" type="Xrm.Sdk.EntityReference">
    /// </field>
    /// <field name="incident_customerid" type="Xrm.Sdk.EntityReference">
    /// </field>
    /// <field name="opportunity_customerid" type="Xrm.Sdk.EntityReference">
    /// </field>
    /// <field name="activitypointer_regardingobjectid" type="Xrm.Sdk.EntityReference">
    /// </field>
    /// <field name="activitypointer_subject" type="String">
    /// </field>
    /// <field name="account" type="Xrm.Sdk.EntityReference">
    /// </field>
    dev1_session.initializeBase(this, [ 'dev1_session' ]);
    this._metaData['dev1_duration'] = Xrm.Sdk.AttributeTypes.int_;
}
dev1_session.prototype = {
    createdon: null,
    dev1_description: null,
    dev1_duration: null,
    dev1_activityid: null,
    dev1_activitytypename: null,
    dev1_emailid: null,
    dev1_phonecallid: null,
    dev1_letterid: null,
    dev1_taskid: null,
    dev1_endtime: null,
    dev1_row: null,
    statuscode: null,
    dev1_sessionid: null,
    dev1_starttime: null,
    contract_customerid: null,
    incident_customerid: null,
    opportunity_customerid: null,
    activitypointer_regardingobjectid: null,
    activitypointer_subject: null,
    account: null
}


Type.registerNamespace('Client.TimeSheet.Model');

////////////////////////////////////////////////////////////////////////////////
// Client.TimeSheet.Model.Queries

Client.TimeSheet.Model.Queries = function Client_TimeSheet_Model_Queries() {
    /// <field name="currentRunningActivities" type="String" static="true">
    /// </field>
    /// <field name="currentOpenActivitesWithSessions" type="String" static="true">
    /// Get each open activity for the current user
    /// where there is at least one session
    /// Also return if the session is running
    /// </field>
    /// <field name="sessionsByWeekStartDate" type="String" static="true">
    /// </field>
}


Type.registerNamespace('Client.TimeSheet.RibbonCommands');

////////////////////////////////////////////////////////////////////////////////
// Client.TimeSheet.RibbonCommands.Global

Client.TimeSheet.RibbonCommands.Global = function Client_TimeSheet_RibbonCommands_Global() {
}
Client.TimeSheet.RibbonCommands.Global.newAccount = function Client_TimeSheet_RibbonCommands_Global$newAccount() {
    Xrm.Utility.openEntityForm('account', null, null);
}
Client.TimeSheet.RibbonCommands.Global.accountOnSave = function Client_TimeSheet_RibbonCommands_Global$accountOnSave() {
    if (Xrm.Page.ui.getFormType() === 10*.1) {
        var parentOpener = window.top.opener;
        if (typeof(parentOpener.Client) !== 'undefined') {
            parentOpener.Client.TimeSheet.RibbonCommands.Global.newAccountCallBack();
        }
    }
}
Client.TimeSheet.RibbonCommands.Global.newAccountCallBack = function Client_TimeSheet_RibbonCommands_Global$newAccountCallBack() {
    alert('Refresh!');
}
Client.TimeSheet.RibbonCommands.Global.getRunningActivities = function Client_TimeSheet_RibbonCommands_Global$getRunningActivities(commandProperties) {
    /// <param name="commandProperties" type="Object">
    /// </param>
    var runningActivities = Xrm.Sdk.OrganizationServiceProxy.retrieveMultiple(Client.TimeSheet.Model.Queries.currentOpenActivitesWithSessions);
    var section = new Xrm.Sdk.Ribbon.RibbonMenuSection('dev1.Activities.Section', 'Activities', 1, 'Menu16');
    var i = 0;
    var $enum1 = ss.IEnumerator.getEnumerator(runningActivities.get_entities());
    while ($enum1.moveNext()) {
        var activity = $enum1.current;
        var image = 'WebResources/dev1_/images/start.gif';
        var isRunning = activity['isRunning'] != null && (activity['isRunning'].toString() === '1');
        if (isRunning) {
            image = 'WebResources/dev1_/images/stop.gif';
        }
        section.addButton(new Xrm.Sdk.Ribbon.RibbonButton('dev1.Activity.' + activity['a.activityid'].toString(), i, activity['a.subject'].toString(), 'dev1.ApplicationRibbon.StartStopActivity.Command', image, image));
        i++;
    }
    var activities = new Xrm.Sdk.Ribbon.RibbonMenu('dev1.Activities').addSection(section);
    commandProperties.PopulationXML = activities.serialiseToRibbonXml();
}
Client.TimeSheet.RibbonCommands.Global.startStopActivity = function Client_TimeSheet_RibbonCommands_Global$startStopActivity(commandProperties) {
    /// <param name="commandProperties" type="Object">
    /// </param>
    alert(commandProperties.SourceControlId);
    var values = 'activityid=' + commandProperties.SourceControlId.replaceAll('dev1.Activity.', '');
    var parameters = encodeURIComponent(values);
    Xrm.Utility.openWebResource('dev1_/scripts/StartStopSession.htm', parameters, 400, 300);
}


ActivityPointer.registerClass('ActivityPointer', Xrm.Sdk.Entity);
dev1_session.registerClass('dev1_session', Xrm.Sdk.Entity);
Client.TimeSheet.Model.Queries.registerClass('Client.TimeSheet.Model.Queries');
Client.TimeSheet.RibbonCommands.Global.registerClass('Client.TimeSheet.RibbonCommands.Global');
ActivityPointer.entityLogicalName = 'activitypointer';
ActivityPointer.entityTypeCode = 4200;
dev1_session.entityLogicalName = 'dev1_session';
dev1_session.entityTypeCode = 10000;
Client.TimeSheet.Model.Queries.currentRunningActivities = "<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>" + "<entity name='activitypointer'>" + "<attribute name='activitytypecode' />" + "<attribute name='subject' />" + "<attribute name='activityid' />" + "<attribute name='instancetypecode' />" + "<order attribute='modifiedon' descending='false' />" + "<filter type='and'>" + "<condition attribute='ownerid' operator='eq-userid' />" + '</filter>' + '</entity>' + '</fetch>';
Client.TimeSheet.Model.Queries.currentOpenActivitesWithSessions = "<fetch version='1.0' output-format='xml-platform' mapping='logical' aggregate='true'>" + "<entity name='activitypointer'>" + "<attribute name='subject' groupby='true' alias='a.subject'/>" + "<attribute name='activityid' groupby='true' alias='a.activityid'/>" + "<filter type='and'>" + "<condition attribute='ownerid' operator='eq-userid'  />" + "<condition attribute='statecode' operator='not-in'>" + '<value>1</value>' + '<value>2</value>' + '</condition>' + '</filter>' + "<link-entity name='dev1_session' from='dev1_activityid' to='activityid' alias='s'>" + "<attribute name='dev1_runningflag' aggregate='max' distinct='true' alias='isRunning'/>" + '</link-entity>' + '</entity>' + '</fetch>';
Client.TimeSheet.Model.Queries.sessionsByWeekStartDate = "\r\n                    <fetch>\r\n                        <entity name='dev1_session' >\r\n                            <attribute name='dev1_sessionid' />\r\n                            <attribute name='dev1_description' />\r\n                            <attribute name='dev1_activityid' />\r\n                            <attribute name='dev1_activitytypename' />\r\n                            <attribute name='dev1_starttime' />\r\n                            <attribute name='dev1_endtime' />\r\n                            <attribute name='dev1_duration' />\r\n                            <attribute name='dev1_taskid' />\r\n                            <attribute name='dev1_letterid' />\r\n                            <attribute name='dev1_emailid' />\r\n                            <attribute name='dev1_phonecallid' />\r\n                            <attribute name='statuscode' />\r\n                            <attribute name='dev1_row' />\r\n                            <order attribute='dev1_row' descending='false' />\r\n                            <filter type='and'>\r\n                                <condition attribute='dev1_starttime' operator='on-or-after' value='{0}' />\r\n                                <condition attribute='dev1_starttime' operator='on-or-before' value='{1}' />\r\n                            </filter>\r\n                            <link-entity name='activitypointer' from='activityid' to='dev1_activityid' alias='aa' >\r\n                                <attribute name='regardingobjectid' alias='activitypointer_regardingobjectid' />\r\n                                <attribute name='subject' alias='activitypointer_subject' />\r\n                                <link-entity name='contract' from='contractid' to='regardingobjectid' visible='false' link-type='outer' alias='contract' >\r\n                                    <attribute name='customerid' alias='contract_customerid'/>\r\n                                </link-entity>\r\n                                <link-entity name='opportunity' from='opportunityid' to='regardingobjectid' visible='false' link-type='outer' alias='opportunity' >\r\n                                    <attribute name='customerid' alias='opportunity_customerid'/>\r\n                                </link-entity>\r\n                                <link-entity name='incident' from='incidentid' to='regardingobjectid' visible='false' link-type='outer' alias='incident' >\r\n                                    <attribute name='customerid' alias='incident_customerid'/>\r\n                                </link-entity>\r\n                            </link-entity>\r\n                        </entity>\r\n                    </fetch>";
});
