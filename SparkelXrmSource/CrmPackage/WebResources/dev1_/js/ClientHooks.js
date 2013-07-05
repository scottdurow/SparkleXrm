//! ClientHooks.debug.js
//
waitForScripts("ribboncommands",["mscorlib","xrm"],
function () {



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
    ActivityPointer.initializeBase(this, [ 'activitypointer' ]);
}
ActivityPointer.prototype = {
    activityid: null,
    subject: null,
    activitytypecode: null
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
    /// <field name="dev1_sessionid" type="Xrm.Sdk.Guid">
    /// </field>
    /// <field name="dev1_starttime" type="Date">
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
    dev1_sessionid: null,
    dev1_starttime: null
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
Client.TimeSheet.Model.Queries.sessionsByWeekStartDate = "<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>" + "<entity name='dev1_session'>" + "<attribute name='dev1_sessionid' />" + "<attribute name='dev1_description' />" + "<attribute name='dev1_activityid' />" + "<attribute name='dev1_activitytypename' />" + "<attribute name='dev1_starttime' />" + "<attribute name='dev1_endtime' />" + "<attribute name='dev1_duration' />" + "<attribute name='dev1_taskid' />" + "<attribute name='dev1_letterid' />" + "<attribute name='dev1_emailid' />" + "<attribute name='dev1_phonecallid' />" + "<order attribute='dev1_description' descending='false' />" + "<filter type='and'>" + "<condition attribute='dev1_starttime' operator='on-or-after' value='{0}' />" + "<condition attribute='dev1_starttime' operator='on-or-before' value='{1}' />" + '</filter>' + '</entity>' + '</fetch>';
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
