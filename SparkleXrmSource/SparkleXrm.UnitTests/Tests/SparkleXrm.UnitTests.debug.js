//! SparkleXrm.UnitTests.debug.js
//

(function() {

Type.registerNamespace('SparkleXrm.UnitTests');

////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.UnitTests.ActionTests

SparkleXrm.UnitTests.ActionTests = function SparkleXrm_UnitTests_ActionTests() {
}
SparkleXrm.UnitTests.ActionTests.WinOpportunity = function SparkleXrm_UnitTests_ActionTests$WinOpportunity(assert) {
    assert.expect(1);
    var name = 'Unit Test' + Date.get_now().toISOString();
    var account = new SparkleXrm.Sdk.Entity('account');
    account.setAttributeValue('name', name);
    account.id = SparkleXrm.Sdk.OrganizationServiceProxy.create(account).value;
    var opportunity = new SparkleXrm.Sdk.Entity('opportunity');
    opportunity.setAttributeValue('customerid', account.toEntityReference());
    opportunity.setAttributeValue('name', name);
    SparkleXrm.Sdk.WebApiOrganizationServiceProxy.addNavigationPropertyMetadata('opportunity', 'customerid', 'account,contact');
    opportunity.id = SparkleXrm.Sdk.OrganizationServiceProxy.create(opportunity).value;
    var request = new SparkleXrm.Sdk.Messages.WinOpportunityRequest();
    var oppClose = new SparkleXrm.Sdk.Entity('opportunityclose');
    oppClose.setAttributeValue('subject', 'Win!!');
    oppClose.setAttributeValue('opportunityid', opportunity.toEntityReference());
    request.OpportunityClose = oppClose;
    request.Status = new SparkleXrm.Sdk.OptionSetValue(3);
    try {
        SparkleXrm.Sdk.OrganizationServiceProxy.registerExecuteMessageResponseType('WinOpportunity', SparkleXrm.Sdk.Messages.WinOpportunityResponse);
        var winResponse = SparkleXrm.Sdk.OrganizationServiceProxy.execute(request);
        var closedOpp = SparkleXrm.Sdk.OrganizationServiceProxy.retrieve(opportunity.logicalName, opportunity.id, [ 'statuscode' ]);
        assert.equal(closedOpp.getAttributeValueOptionSet('statuscode').value, 3, 'Opportunity closed');
    }
    finally {
        SparkleXrm.Sdk.OrganizationServiceProxy.delete_(opportunity.logicalName, new SparkleXrm.Sdk.Guid(opportunity.id));
        SparkleXrm.Sdk.OrganizationServiceProxy.delete_(account.logicalName, new SparkleXrm.Sdk.Guid(account.id));
    }
}
SparkleXrm.UnitTests.ActionTests.AddToQueue = function SparkleXrm_UnitTests_ActionTests$AddToQueue(assert) {
    assert.expect(1);
    var letter = new SparkleXrm.Sdk.Entity('letter');
    letter.setAttributeValue('subject', 'Test');
    letter.id = SparkleXrm.Sdk.OrganizationServiceProxy.create(letter).value;
    var queue = new SparkleXrm.Sdk.Entity('queue');
    queue.setAttributeValue('name', 'Test');
    queue.id = SparkleXrm.Sdk.OrganizationServiceProxy.create(queue).value;
    try {
        var request = new SparkleXrm.Sdk.Messages.AddToQueueRequest();
        request.DestinationQueueId = new SparkleXrm.Sdk.Guid(queue.id);
        request.Target = letter.toEntityReference();
        SparkleXrm.Sdk.OrganizationServiceProxy.registerExecuteMessageResponseType('AddToQueue', SparkleXrm.Sdk.Messages.AddToQueueResponse);
        var response = SparkleXrm.Sdk.OrganizationServiceProxy.execute(request);
        assert.ok(response.queueItemId != null, 'Queue item returned');
    }
    finally {
        SparkleXrm.Sdk.OrganizationServiceProxy.delete_(letter.logicalName, new SparkleXrm.Sdk.Guid(letter.id));
        SparkleXrm.Sdk.OrganizationServiceProxy.delete_(queue.logicalName, new SparkleXrm.Sdk.Guid(queue.id));
    }
}


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.UnitTests.AttributeTypeTests

SparkleXrm.UnitTests.AttributeTypeTests = function SparkleXrm_UnitTests_AttributeTypeTests() {
}
SparkleXrm.UnitTests.AttributeTypeTests.EntityReference_01 = function SparkleXrm_UnitTests_AttributeTypeTests$EntityReference_01(assert) {
    assert.expect(4);
    var name = 'Unit Test' + Date.get_now().toISOString();
    var account = new SparkleXrm.Sdk.Entity('account');
    account.setAttributeValue('name', name);
    account.id = SparkleXrm.Sdk.OrganizationServiceProxy.create(account).value;
    var contact = new SparkleXrm.Sdk.Entity('contact');
    SparkleXrm.Sdk.WebApiOrganizationServiceProxy.addNavigationPropertyMetadata('contact', 'parentcustomerid', 'account,contact');
    contact.setAttributeValue('lastname', name);
    contact.setAttributeValue('parentcustomerid', account.toEntityReference());
    contact.id = SparkleXrm.Sdk.OrganizationServiceProxy.create(contact).toString();
    var contact2 = SparkleXrm.Sdk.OrganizationServiceProxy.retrieve(contact.logicalName, contact.id, [ 'parentcustomerid' ]);
    assert.equal(contact.getAttributeValueEntityReference('parentcustomerid').id.value, contact2.getAttributeValueEntityReference('parentcustomerid').id.value, 'Account Contact related: ID correct');
    assert.equal(contact.getAttributeValueEntityReference('parentcustomerid').logicalName, contact2.getAttributeValueEntityReference('parentcustomerid').logicalName, 'Account Contact related: Logical Name correct');
    contact.setAttributeValue('parentcustomerid', null);
    contact.setAttributeValue('contactid', new SparkleXrm.Sdk.Guid(contact.id));
    SparkleXrm.Sdk.OrganizationServiceProxy.update(contact);
    var contact3 = SparkleXrm.Sdk.OrganizationServiceProxy.retrieve(contact.logicalName, contact.id, [ 'parentcustomerid' ]);
    assert.ok(contact3.getAttributeValueEntityReference('parentcustomerid') == null, 'Nulled lookup on update');
    var contact4 = new SparkleXrm.Sdk.Entity('contact');
    contact4.setAttributeValue('lastname', name);
    contact4.setAttributeValue('parentcustomerid', null);
    contact4.id = SparkleXrm.Sdk.OrganizationServiceProxy.create(contact4).value;
    var contact5 = SparkleXrm.Sdk.OrganizationServiceProxy.retrieve(contact4.logicalName, contact4.id, [ 'parentcustomerid' ]);
    assert.ok(contact5.getAttributeValueEntityReference('parentcustomerid') == null, 'Nulled lookup on create');
    SparkleXrm.Sdk.OrganizationServiceProxy.delete_(contact.logicalName, new SparkleXrm.Sdk.Guid(contact.id));
    SparkleXrm.Sdk.OrganizationServiceProxy.delete_(account.logicalName, new SparkleXrm.Sdk.Guid(account.id));
    SparkleXrm.Sdk.OrganizationServiceProxy.delete_(contact4.logicalName, new SparkleXrm.Sdk.Guid(contact4.id));
}
SparkleXrm.UnitTests.AttributeTypeTests.EntityReference_02_SetPrimarContactToNull = function SparkleXrm_UnitTests_AttributeTypeTests$EntityReference_02_SetPrimarContactToNull(assert) {
    assert.expect(2);
    var name = 'Unit Test' + Date.get_now().toISOString();
    var account = new SparkleXrm.Sdk.Entity('account');
    account.setAttributeValue('name', name);
    account.id = SparkleXrm.Sdk.OrganizationServiceProxy.create(account).value;
    var contact = new SparkleXrm.Sdk.Entity('contact');
    SparkleXrm.Sdk.WebApiOrganizationServiceProxy.addNavigationPropertyMetadata('contact', 'parentcustomerid', 'account,contact');
    SparkleXrm.Sdk.WebApiOrganizationServiceProxy.addNavigationPropertyMetadata('account', 'primarycontactid', 'contact');
    contact.setAttributeValue('lastname', name);
    contact.setAttributeValue('parentcustomerid', account.toEntityReference());
    contact.id = SparkleXrm.Sdk.OrganizationServiceProxy.create(contact).toString();
    account.setAttributeValue('primarycontactid', contact.toEntityReference());
    account.setAttributeValue('accountid', new SparkleXrm.Sdk.Guid(account.id));
    SparkleXrm.Sdk.OrganizationServiceProxy.update(account);
    var account2 = SparkleXrm.Sdk.OrganizationServiceProxy.retrieve(account.logicalName, account.id, [ 'primarycontactid' ]);
    assert.equal(account2.getAttributeValueEntityReference('primarycontactid').id.value, contact.id, 'Primary Contact Set');
    account.setAttributeValue('primarycontactid', null);
    SparkleXrm.Sdk.OrganizationServiceProxy.update(account);
    var account3 = SparkleXrm.Sdk.OrganizationServiceProxy.retrieve(account.logicalName, account.id, [ 'primarycontactid' ]);
    assert.equal(account3.getAttributeValueEntityReference('primarycontactid'), null, 'Primary Contact Set to null');
    SparkleXrm.Sdk.OrganizationServiceProxy.delete_(contact.logicalName, new SparkleXrm.Sdk.Guid(contact.id));
    SparkleXrm.Sdk.OrganizationServiceProxy.delete_(account.logicalName, new SparkleXrm.Sdk.Guid(account.id));
}
SparkleXrm.UnitTests.AttributeTypeTests.EntityReference_03_CustomerId = function SparkleXrm_UnitTests_AttributeTypeTests$EntityReference_03_CustomerId(assert) {
    assert.expect(2);
    var name = 'Unit Test' + Date.get_now().toISOString();
    var account = new SparkleXrm.Sdk.Entity('account');
    account.setAttributeValue('name', name);
    account.id = SparkleXrm.Sdk.OrganizationServiceProxy.create(account).value;
    var opportunity = new SparkleXrm.Sdk.Entity('opportunity');
    opportunity.setAttributeValue('customerid', account.toEntityReference());
    opportunity.setAttributeValue('name', name);
    try {
        SparkleXrm.Sdk.WebApiOrganizationServiceProxy.addNavigationPropertyMetadata('opportunity', 'customerid', 'account,contact');
        opportunity.id = SparkleXrm.Sdk.OrganizationServiceProxy.create(opportunity).value;
        var opportunity2 = SparkleXrm.Sdk.OrganizationServiceProxy.retrieve(opportunity.logicalName, opportunity.id, [ 'customerid' ]);
        assert.equal(opportunity2.getAttributeValueEntityReference('customerid').logicalName, account.logicalName, 'Logical Name correct');
        assert.equal(opportunity2.getAttributeValueEntityReference('customerid').id.value, account.id, 'Id correct : ' + account.id);
    }
    finally {
        SparkleXrm.Sdk.OrganizationServiceProxy.delete_(opportunity.logicalName, new SparkleXrm.Sdk.Guid(opportunity.id));
        SparkleXrm.Sdk.OrganizationServiceProxy.delete_(account.logicalName, new SparkleXrm.Sdk.Guid(account.id));
    }
}
SparkleXrm.UnitTests.AttributeTypeTests.Money_01 = function SparkleXrm_UnitTests_AttributeTypeTests$Money_01(assert) {
    assert.expect(2);
    var name = 'Unit Test' + Date.get_now().toISOString();
    var account = new SparkleXrm.Sdk.Entity('account');
    account.setAttributeValue('name', name);
    var creditlimit = new SparkleXrm.Sdk.Money(123456);
    account.setAttributeValue('creditlimit', creditlimit);
    account.id = SparkleXrm.Sdk.OrganizationServiceProxy.create(account).value;
    var account2 = SparkleXrm.Sdk.OrganizationServiceProxy.retrieve(account.logicalName, account.id, [ 'creditlimit', 'transactioncurrencyid' ]);
    assert.equal((account2.getAttributeValue('creditlimit')).value, creditlimit.value, 'Money value correct');
    var currency = account2.getAttributeValueEntityReference('transactioncurrencyid');
    assert.ok(currency != null && currency.logicalName === 'transactioncurrency', 'Transaction Currency = ' + currency.name);
    SparkleXrm.Sdk.OrganizationServiceProxy.delete_(account.logicalName, new SparkleXrm.Sdk.Guid(account.id));
}


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.UnitTests.CRUDTests

SparkleXrm.UnitTests.CRUDTests = function SparkleXrm_UnitTests_CRUDTests() {
}
SparkleXrm.UnitTests.CRUDTests.Create_01 = function SparkleXrm_UnitTests_CRUDTests$Create_01(assert) {
    var done = assert.async();
    var contact = new SparkleXrm.Sdk.Entity('contact');
    contact.setAttributeValue('lastname', 'Test ' + Date.get_now().toISOString());
    SparkleXrm.Sdk.OrganizationServiceProxy.beginCreate(contact, function(state) {
        contact.id = SparkleXrm.Sdk.OrganizationServiceProxy.endCreate(state).toString();
        assert.ok(contact.id, 'New ID = ' + contact.id);
        done();
        SparkleXrm.Sdk.OrganizationServiceProxy.delete_(contact.logicalName, new SparkleXrm.Sdk.Guid(contact.id));
    });
}
SparkleXrm.UnitTests.CRUDTests.Create_01_Sync = function SparkleXrm_UnitTests_CRUDTests$Create_01_Sync(assert) {
    var contact = new SparkleXrm.Sdk.Entity('contact');
    contact.setAttributeValue('lastname', 'Test ' + Date.get_now().toISOString());
    contact.id = SparkleXrm.Sdk.OrganizationServiceProxy.create(contact).toString();
    assert.ok(contact.id, 'New ID = ' + contact.id);
    SparkleXrm.Sdk.OrganizationServiceProxy.delete_(contact.logicalName, new SparkleXrm.Sdk.Guid(contact.id));
}
SparkleXrm.UnitTests.CRUDTests.Update_01 = function SparkleXrm_UnitTests_CRUDTests$Update_01(assert) {
    assert.expect(2);
    var contact = new SparkleXrm.Sdk.Entity('contact');
    contact.setAttributeValue('lastname', 'Test ' + Date.get_now().toISOString());
    contact.id = SparkleXrm.Sdk.OrganizationServiceProxy.create(contact).toString();
    contact.setAttributeValue('contactid', new SparkleXrm.Sdk.Guid(contact.id));
    assert.ok(contact.id, 'New ID = ' + contact.id);
    contact.setAttributeValue('lastname', 'Update');
    SparkleXrm.Sdk.OrganizationServiceProxy.update(contact);
    var contact2 = SparkleXrm.Sdk.OrganizationServiceProxy.retrieve(contact.logicalName, contact.id, [ 'lastname' ]);
    assert.equal(contact2.getAttributeValue('lastname'), contact.getAttributeValue('lastname'), 'Contact update');
    SparkleXrm.Sdk.OrganizationServiceProxy.delete_(contact.logicalName, new SparkleXrm.Sdk.Guid(contact.id));
}
SparkleXrm.UnitTests.CRUDTests.Update_02_UnknownEntity = function SparkleXrm_UnitTests_CRUDTests$Update_02_UnknownEntity(assert) {
}
SparkleXrm.UnitTests.CRUDTests.Update_03_UnknownAttribute = function SparkleXrm_UnitTests_CRUDTests$Update_03_UnknownAttribute(assert) {
}
SparkleXrm.UnitTests.CRUDTests.Retrieve_01 = function SparkleXrm_UnitTests_CRUDTests$Retrieve_01(assert) {
}
SparkleXrm.UnitTests.CRUDTests.Retrieve_02_UnknownLogicalName = function SparkleXrm_UnitTests_CRUDTests$Retrieve_02_UnknownLogicalName(assert) {
    assert.expect(1);
    try {
        var contact2 = SparkleXrm.Sdk.OrganizationServiceProxy.retrieve('unknown_logicalname', '00000000-0000-0000-0000-000000000001', [ 'lastname' ]);
    }
    catch (ex) {
        assert.ok(ex.message.indexOf('unknown_logicalname') > -1, 'Exception thrown:' + ex.message);
    }
}
SparkleXrm.UnitTests.CRUDTests.Retrieve_02_UnknownAttribute = function SparkleXrm_UnitTests_CRUDTests$Retrieve_02_UnknownAttribute(assert) {
    assert.expect(1);
    var contact = new SparkleXrm.Sdk.Entity('contact');
    contact.setAttributeValue('lastname', 'Test ' + Date.get_now().toISOString());
    contact.id = SparkleXrm.Sdk.OrganizationServiceProxy.create(contact).toString();
    try {
        var contact2 = SparkleXrm.Sdk.OrganizationServiceProxy.retrieve('contact', contact.id, [ 'unknown_attribute' ]);
    }
    catch (ex) {
        assert.ok(ex.message.indexOf('unknown_attribute') > -1, 'Exception thrown:' + ex.message);
    }
    finally {
    }
}
SparkleXrm.UnitTests.CRUDTests.Delete_01_Sync = function SparkleXrm_UnitTests_CRUDTests$Delete_01_Sync(assert) {
    var contact = new SparkleXrm.Sdk.Entity('contact');
    contact.setAttributeValue('lastname', 'Test ' + Date.get_now().toISOString());
    contact.id = SparkleXrm.Sdk.OrganizationServiceProxy.create(contact).toString();
    assert.expect(1);
    SparkleXrm.Sdk.OrganizationServiceProxy.delete_(contact.logicalName, new SparkleXrm.Sdk.Guid(contact.id));
    try {
        var deletedContact = SparkleXrm.Sdk.OrganizationServiceProxy.retrieve('contact', contact.id, [ 'fullname' ]);
        assert.notOk(true, 'Contact not detailed');
    }
    catch (ex) {
        assert.ok(ex.message.indexOf(contact.id) > -1, ex.message);
    }
}
SparkleXrm.UnitTests.CRUDTests.Delete_02_Async = function SparkleXrm_UnitTests_CRUDTests$Delete_02_Async(assert) {
    var contact = new SparkleXrm.Sdk.Entity('contact');
    contact.setAttributeValue('lastname', 'Test ' + Date.get_now().toISOString());
    contact.id = SparkleXrm.Sdk.OrganizationServiceProxy.create(contact).toString();
    var done = assert.async();
    assert.expect(1);
    SparkleXrm.Sdk.OrganizationServiceProxy.beginDelete(contact.logicalName, new SparkleXrm.Sdk.Guid(contact.id), function(state) {
        SparkleXrm.Sdk.OrganizationServiceProxy.endDelete(state);
        try {
            var deletedContact = SparkleXrm.Sdk.OrganizationServiceProxy.retrieve('contact', contact.id, [ 'fullname' ]);
        }
        catch (ex) {
            assert.ok(ex.message.indexOf(contact.id) > -1, ex.message);
        }
        done();
    });
}
SparkleXrm.UnitTests.CRUDTests.Asscoiate_01_Sync = function SparkleXrm_UnitTests_CRUDTests$Asscoiate_01_Sync(assert) {
    assert.expect(3);
    var acc1 = new SparkleXrm.Sdk.Entity('account');
    acc1.setAttributeValue('name', 'Test ' + Date.get_now().toISOString());
    var lead1 = new SparkleXrm.Sdk.Entity('lead');
    acc1.id = SparkleXrm.Sdk.OrganizationServiceProxy.create(acc1).value;
    lead1.id = SparkleXrm.Sdk.OrganizationServiceProxy.create(lead1).value;
    try {
        SparkleXrm.Sdk.OrganizationServiceProxy.associate(acc1.logicalName, new SparkleXrm.Sdk.Guid(acc1.id), new SparkleXrm.Sdk.Relationship('accountleads_association'), [lead1.toEntityReference()]);
        var fetchxml = "<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='true'>\r\n  <entity name='lead'>  \r\n    <attribute name='leadid' />\r\n    <order attribute='fullname' descending='false' />\r\n    <link-entity name='accountleads' from='leadid' to='leadid' visible='false' intersect='true'>\r\n      <link-entity name='account' from='accountid' to='accountid' alias='ag'>\r\n        <filter type='and'>\r\n          <condition attribute='accountid' operator='eq' value='{" + acc1.id + "}' />\r\n        </filter>\r\n      </link-entity>\r\n    </link-entity>\r\n  </entity>\r\n</fetch>";
        var results = SparkleXrm.Sdk.OrganizationServiceProxy.retrieveMultiple(fetchxml);
        assert.equal(results.entities.get_count(), 1, '1 lead returned');
        assert.equal(results.entities.get_item(0).getAttributeValueGuid('leadid'), lead1.id, 'Lead ID correct');
        SparkleXrm.Sdk.OrganizationServiceProxy.disassociate(acc1.logicalName, new SparkleXrm.Sdk.Guid(acc1.id), new SparkleXrm.Sdk.Relationship('accountleads_association'), [lead1.toEntityReference()]);
        var results2 = SparkleXrm.Sdk.OrganizationServiceProxy.retrieveMultiple(fetchxml);
        assert.equal(results2.entities.get_count(), 0, '0 leads returned');
    }
    finally {
        SparkleXrm.Sdk.OrganizationServiceProxy.delete_(lead1.logicalName, new SparkleXrm.Sdk.Guid(lead1.id));
        SparkleXrm.Sdk.OrganizationServiceProxy.delete_(acc1.logicalName, new SparkleXrm.Sdk.Guid(acc1.id));
    }
}


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.UnitTests.FetchXmlTests

SparkleXrm.UnitTests.FetchXmlTests = function SparkleXrm_UnitTests_FetchXmlTests() {
}
SparkleXrm.UnitTests.FetchXmlTests.RetreiveMultiple_01_Simple = function SparkleXrm_UnitTests_FetchXmlTests$RetreiveMultiple_01_Simple(assert) {
    assert.expect(1);
    var done = assert.async();
    SparkleXrm.Sdk.OrganizationServiceProxy.beginRetrieveMultiple("<fetch version='1.0' output-format='xml-platform' mapping='logical' count='2' distinct='false' returntotalrecordcount='true'>\r\n                          <entity name='account'>\r\n                            <attribute name='name' />\r\n                            <attribute name='primarycontactid' />\r\n                            <attribute name='telephone1' />\r\n                            <attribute name='accountid' />\r\n                            <order attribute='name' descending='false' />\r\n                          </entity>\r\n                        </fetch>", function(state) {
        var items = SparkleXrm.Sdk.OrganizationServiceProxy.endRetrieveMultiple(state, SparkleXrm.Sdk.Entity);
        assert.ok(items.entities.get_count() > 0, 'Non zero return count');
        done();
    });
}
SparkleXrm.UnitTests.FetchXmlTests.RetreiveMultiple_02_InvalidXml = function SparkleXrm_UnitTests_FetchXmlTests$RetreiveMultiple_02_InvalidXml(assert) {
    assert.expect(1);
    var done = assert.async();
    try {
        SparkleXrm.Sdk.OrganizationServiceProxy.beginRetrieveMultiple("<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>\r\n                              <", function(state) {
            try {
                var items = SparkleXrm.Sdk.OrganizationServiceProxy.endRetrieveMultiple(state, SparkleXrm.Sdk.Entity);
            }
            catch (ex) {
                assert.ok(ex.message.indexOf('Invalid XML') > -1, ex.message);
                done();
            }
        });
    }
    catch (ex) {
        assert.ok(ex.message.indexOf('Invalid FetchXml') > -1, ex.message);
        done();
    }
}
SparkleXrm.UnitTests.FetchXmlTests.RetreiveMultiple_03_UnkownLogicalName = function SparkleXrm_UnitTests_FetchXmlTests$RetreiveMultiple_03_UnkownLogicalName(assert) {
    var done = assert.async();
    assert.expect(1);
    SparkleXrm.Sdk.OrganizationServiceProxy.beginRetrieveMultiple("<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>\r\n                          <entity name='unknown_entity'>\r\n                            <attribute name='name' />\r\n                            <attribute name='primarycontactid' />\r\n                            <attribute name='telephone1' />\r\n                            <attribute name='accountid' />\r\n                            <order attribute='name' descending='false' />\r\n                          </entity>\r\n                        </fetch>", function(state) {
        try {
            var items = SparkleXrm.Sdk.OrganizationServiceProxy.endRetrieveMultiple(state, SparkleXrm.Sdk.Entity);
        }
        catch (ex) {
            assert.ok(ex.message.indexOf('unknown_entity') > -1, ex.message);
        }
        done();
    });
}
SparkleXrm.UnitTests.FetchXmlTests.RetreiveMultiple_04_VeryLongFetch = function SparkleXrm_UnitTests_FetchXmlTests$RetreiveMultiple_04_VeryLongFetch(assert) {
    assert.expect(1);
    var query = '<value>{00000000-0000-0000-0000-000000000000}</value>';
    var fetchxml = "<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>\r\n                              <entity name='contact'>\r\n                                <attribute name='fullname' />\r\n                                <attribute name='telephone1' />\r\n                                <attribute name='contactid' />\r\n                                <order attribute='fullname' descending='false' />\r\n                                <filter type='and'>\r\n                                  <condition attribute='contactid' operator='in'>\r\n                                   {0}\r\n                                  </condition>\r\n                                </filter>\r\n                              </entity>\r\n                            </fetch>";
    var longCondition = '';
    for (var j = 0; j < 400; j++) {
        longCondition += query;
    }
    var fetchQuery = String.format(fetchxml, longCondition);
    var results = SparkleXrm.Sdk.OrganizationServiceProxy.retrieveMultiple(fetchQuery);
    assert.ok(!results.entities.get_count(), 'No results - but not an error due to the larget fetch size!');
}
SparkleXrm.UnitTests.FetchXmlTests.RetreiveMultiple_05_TotalRecordCount = function SparkleXrm_UnitTests_FetchXmlTests$RetreiveMultiple_05_TotalRecordCount(assert) {
    assert.expect(2);
    var done = assert.async();
    SparkleXrm.Sdk.OrganizationServiceProxy.beginRetrieveMultiple("<fetch version='1.0' output-format='xml-platform' mapping='logical' count='1' distinct='false' returntotalrecordcount='true'>\r\n                          <entity name='account'>\r\n                            <attribute name='name' />\r\n                            <order attribute='name' descending='false' />\r\n                          </entity>\r\n                        </fetch>", function(state) {
        var items = SparkleXrm.Sdk.OrganizationServiceProxy.endRetrieveMultiple(state, SparkleXrm.Sdk.Entity);
        assert.ok(items.entities.get_count() > 0, 'Non zero return count');
        assert.ok(items.totalRecordCount > 0, 'Total Record count returned');
        done();
    });
}


////////////////////////////////////////////////////////////////////////////////
// FormContextTests

FormContextTests = function FormContextTests() {
}
FormContextTests.contactFormOnLoad = function FormContextTests$contactFormOnLoad() {
    FormContextTests.issue143_CreateFromDate();
}
FormContextTests.issue143_CreateFromDate = function FormContextTests$issue143_CreateFromDate() {
    var contact = window.parent.Xrm.Page.data.entity;
    var attrName = 'lastonholdtime';
    var xrmAttr = contact.attributes.get(attrName);
    var dt = xrmAttr.getValue();
    var contact1 = new SparkleXrm.Sdk.Entity('contact');
    contact1.setAttributeValue(attrName, new Date(dt.getFullYear(), dt.getMonth(), dt.getDate(), dt.getHours(), dt.getMinutes(), dt.getSeconds(), dt.getMilliseconds()));
    contact1.setAttributeValue('lastname', 'TEST');
    SparkleXrm.Sdk.OrganizationServiceProxy.beginCreate(contact1, function(state) {
        var contactId = SparkleXrm.Sdk.OrganizationServiceProxy.endCreate(state);
    });
}


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.UnitTests.FunctionTests

SparkleXrm.UnitTests.FunctionTests = function SparkleXrm_UnitTests_FunctionTests() {
}
SparkleXrm.UnitTests.FunctionTests.WhoAmI = function SparkleXrm_UnitTests_FunctionTests$WhoAmI(assert) {
    assert.expect(1);
    var request = new SparkleXrm.Sdk.Messages.WhoAmIRequest();
    SparkleXrm.Sdk.OrganizationServiceProxy.registerExecuteMessageResponseType('WhoAmI', SparkleXrm.Sdk.Messages.WhoAmIResponse);
    var response = SparkleXrm.Sdk.OrganizationServiceProxy.execute(request);
    assert.ok(response.userId != null, 'Userid=' + response.userId.value);
}
SparkleXrm.UnitTests.FunctionTests.RetrieveDuplicates_01_NoExistingContact = function SparkleXrm_UnitTests_FunctionTests$RetrieveDuplicates_01_NoExistingContact(assert) {
    assert.expect(2);
    var contact1 = new SparkleXrm.Sdk.Entity('contact');
    contact1.setAttributeValue('firstname', 'Foo');
    contact1.setAttributeValue('lastname', 'Bar');
    contact1.setAttributeValue('emailaddress1', 'foo@bar.com');
    contact1.id = SparkleXrm.Sdk.OrganizationServiceProxy.create(contact1).value;
    try {
        var request = new Xrm.Sdk.Messages.RetrieveDuplicatesRequest();
        request.matchingEntityName = 'contact';
        request.pagingInfo = new SparkleXrm.Sdk.Messages.PagingInfo();
        request.pagingInfo.PageNumber = 1;
        request.pagingInfo.Count = 10;
        request.pagingInfo.ReturnTotalRecordCount = true;
        var contact = new SparkleXrm.Sdk.Entity('contact');
        contact.setAttributeValue('firstname', 'Foo');
        contact.setAttributeValue('lastname', 'Bar');
        contact.setAttributeValue('emailaddress1', 'foo@bar.com');
        request.businessEntity = contact;
        SparkleXrm.Sdk.OrganizationServiceProxy.registerExecuteMessageResponseType('RetrieveDuplicates', Xrm.Sdk.Messages.RetrieveDuplicatesResponse);
        var response = SparkleXrm.Sdk.OrganizationServiceProxy.execute(request);
        assert.ok(response.duplicateCollection != null && response.duplicateCollection.entities != null, 'Duplicates returned');
        assert.equal(response.duplicateCollection.entities.get_count(), 1, 'Duplicate detected');
    }
    finally {
        SparkleXrm.Sdk.OrganizationServiceProxy.delete_('contact', new SparkleXrm.Sdk.Guid(contact1.id));
    }
}
SparkleXrm.UnitTests.FunctionTests.RetrieveDuplicates_02_ExistingContact = function SparkleXrm_UnitTests_FunctionTests$RetrieveDuplicates_02_ExistingContact(assert) {
    assert.expect(2);
    var id1 = null;
    var id2 = null;
    try {
        var contact = new SparkleXrm.Sdk.Entity('contact');
        contact.setAttributeValue('firstname', 'Foo');
        contact.setAttributeValue('lastname', 'Bar');
        contact.setAttributeValue('emailaddress1', 'foo@bar.com');
        id1 = SparkleXrm.Sdk.OrganizationServiceProxy.create(contact);
        id2 = SparkleXrm.Sdk.OrganizationServiceProxy.create(contact);
        contact.id = id1.value;
        var request = new Xrm.Sdk.Messages.RetrieveDuplicatesRequest();
        request.matchingEntityName = 'contact';
        request.pagingInfo = new SparkleXrm.Sdk.Messages.PagingInfo();
        request.pagingInfo.PageNumber = 1;
        request.pagingInfo.Count = 10;
        request.pagingInfo.ReturnTotalRecordCount = true;
        request.businessEntity = contact;
        SparkleXrm.Sdk.OrganizationServiceProxy.registerExecuteMessageResponseType('RetrieveDuplicates', Xrm.Sdk.Messages.RetrieveDuplicatesResponse);
        var response = SparkleXrm.Sdk.OrganizationServiceProxy.execute(request);
        assert.ok(response.duplicateCollection.entities.get_count() > 0, 'Expected >0 record returned');
        var firstId = response.duplicateCollection.entities.get_item(0).id;
        var secondId = (response.duplicateCollection.entities.get_count() > 1) ? response.duplicateCollection.entities.get_item(1).id : null;
        assert.ok(firstId === id2.value || secondId === id2.value, 'ID of second contact returned');
    }
    finally {
        SparkleXrm.Sdk.OrganizationServiceProxy.delete_('contact', id1);
        SparkleXrm.Sdk.OrganizationServiceProxy.delete_('contact', id2);
    }
}
SparkleXrm.UnitTests.FunctionTests.RetrieveUserPrivileges_01 = function SparkleXrm_UnitTests_FunctionTests$RetrieveUserPrivileges_01(assert) {
    assert.expect(3);
    var whoAmIRequest = new SparkleXrm.Sdk.Messages.WhoAmIRequest();
    SparkleXrm.Sdk.OrganizationServiceProxy.registerExecuteMessageResponseType('WhoAmI', SparkleXrm.Sdk.Messages.WhoAmIResponse);
    var whoAmIResponse = SparkleXrm.Sdk.OrganizationServiceProxy.execute(whoAmIRequest);
    var request = new SparkleXrm.Sdk.Messages.RetrieveUserPrivilegesRequest();
    request.userId = whoAmIResponse.userId;
    SparkleXrm.Sdk.OrganizationServiceProxy.registerExecuteMessageResponseType('RetrieveUserPrivileges', SparkleXrm.Sdk.Messages.RetrieveUserPrivilegesResponse);
    var response = SparkleXrm.Sdk.OrganizationServiceProxy.execute(request);
    assert.ok(response.rolePrivileges.length > 0, 'Privileges returned');
    var depth = response.rolePrivileges[0].Depth;
    assert.ok(depth === 'Basic' || depth === 'Deep' || depth === 'Global' || depth === 'Local', 'Privileges Depth returned');
    assert.ok(response.rolePrivileges[0].PrivilegeId != null && response.rolePrivileges[0].PrivilegeId.value != null, 'Privileges Id returned');
}


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.UnitTests.LocalisationTests

SparkleXrm.UnitTests.LocalisationTests = function SparkleXrm_UnitTests_LocalisationTests() {
}
SparkleXrm.UnitTests.LocalisationTests.prototype = {
    
    NumberParse: function SparkleXrm_UnitTests_LocalisationTests$NumberParse() {
        var format = {};
        format.decimalSymbol = ',';
        format.numberSepartor = '.';
        debugger;
        var value1 = SparkleXrm.NumberEx.parse('22,10', format);
        QUnit.equal(value1, 22.1);
        var value2 = SparkleXrm.NumberEx.parse('1.022,10', format);
        QUnit.equal(value2, 1022.1);
        return true;
    },
    
    LocalTimeZoneTests: function SparkleXrm_UnitTests_LocalisationTests$LocalTimeZoneTests(assert) {
        var dateAttribute = 'lastonholdtime';
        var localTime = new Date();
        var contact = new SparkleXrm.Sdk.Entity('contact');
        contact.setAttributeValue(dateAttribute, localTime);
        contact.setAttributeValue('lastname', 'TEST');
        contact.id = SparkleXrm.Sdk.OrganizationServiceProxy.create(contact).toString();
        var contact2 = SparkleXrm.Sdk.OrganizationServiceProxy.retrieve('contact', contact.id, [ dateAttribute ]);
        var serverTime = contact2.getAttributeValue(dateAttribute);
        assert.equal(serverTime.toUTCString(), localTime.toUTCString(), String.format('dates equal {0} {1}', serverTime.toString(), localTime.toString()));
        SparkleXrm.Sdk.OrganizationServiceProxy.delete_('contact', new SparkleXrm.Sdk.Guid(contact2.id));
        return true;
    },
    
    UTCTimeZoneTests: function SparkleXrm_UnitTests_LocalisationTests$UTCTimeZoneTests(assert) {
        var dateAttribute = 'lastonholdtime';
        var localTime = new Date();
        var utcTime = new Date();
        utcTime.setUTCFullYear(localTime.getUTCFullYear());
        utcTime.setUTCMonth(localTime.getUTCMonth());
        utcTime.setUTCDate(localTime.getUTCDate());
        utcTime.setUTCHours(localTime.getUTCHours());
        utcTime.setUTCMinutes(localTime.getUTCMinutes());
        utcTime.setUTCSeconds(localTime.getUTCSeconds());
        utcTime.setUTCMilliseconds(localTime.getUTCMilliseconds());
        var contact = new SparkleXrm.Sdk.Entity('contact');
        contact.setAttributeValue(dateAttribute, utcTime);
        contact.setAttributeValue('lastname', 'TEST');
        contact.id = SparkleXrm.Sdk.OrganizationServiceProxy.create(contact).toString();
        var contact2 = SparkleXrm.Sdk.OrganizationServiceProxy.retrieve('contact', contact.id, [ dateAttribute ]);
        var serverTime = contact2.getAttributeValue(dateAttribute);
        assert.equal(serverTime.toUTCString(), utcTime.toUTCString(), String.format('dates equal {0} {1}', serverTime.toString(), utcTime.toString()));
        SparkleXrm.Sdk.OrganizationServiceProxy.delete_('contact', new SparkleXrm.Sdk.Guid(contact2.id));
        return true;
    }
}


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.UnitTests.MetadataQueryTests

SparkleXrm.UnitTests.MetadataQueryTests = function SparkleXrm_UnitTests_MetadataQueryTests() {
}
SparkleXrm.UnitTests.MetadataQueryTests.prototype = {
    
    queryAllMetaData: function SparkleXrm_UnitTests_MetadataQueryTests$queryAllMetaData() {
        var request = new SparkleXrm.Sdk.Messages.RetrieveMetadataChangesRequest();
        request.query = {};
        request.query.criteria = {};
        request.query.criteria.filterOperator = 'Or';
        request.query.criteria.conditions = [];
        var condition = {};
        condition.conditionOperator = 'Equals';
        condition.propertyName = 'LogicalName';
        condition.value = 'account';
        request.query.criteria.conditions.add(condition);
        request.query.properties = {};
        request.query.properties.propertyNames = ['Attributes'];
        var attributeQuery = {};
        attributeQuery.properties = {};
        attributeQuery.properties.propertyNames = ['OptionSet'];
        request.query.attributeQuery = attributeQuery;
        var critiera = {};
        attributeQuery.criteria = critiera;
        critiera.filterOperator = 'And';
        critiera.conditions = [];
        var response = SparkleXrm.Sdk.OrganizationServiceProxy.execute(request);
        return true;
    },
    
    queryNameAttributeForAccount: function SparkleXrm_UnitTests_MetadataQueryTests$queryNameAttributeForAccount() {
        var builder = new SparkleXrm.Sdk.Metadata.Query.MetadataQueryBuilder();
        builder.addEntities(['account'], ['PrimaryNameAttribute']);
        var response = SparkleXrm.Sdk.OrganizationServiceProxy.execute(builder.request);
        QUnit.equal(response.entityMetadata[0].primaryNameAttribute, 'name');
        return true;
    },
    
    queryAttributeDisplayNamesForTwoEntities: function SparkleXrm_UnitTests_MetadataQueryTests$queryAttributeDisplayNamesForTwoEntities() {
        var builder = new SparkleXrm.Sdk.Metadata.Query.MetadataQueryBuilder();
        builder.addEntities(['account', 'contact'], ['Attributes', 'DisplayName', 'DisplayCollectionName']);
        builder.addAttributes(['name', 'firstname', 'statecode', 'statuscode'], ['DisplayName']);
        var response = SparkleXrm.Sdk.OrganizationServiceProxy.execute(builder.request);
        return true;
    },
    
    queryOneToManyRelationship: function SparkleXrm_UnitTests_MetadataQueryTests$queryOneToManyRelationship() {
        var request = new SparkleXrm.Sdk.RetrieveRelationshipRequest();
        request.name = 'contact_customer_accounts';
        request.retrieveAsIfPublished = true;
        var response = SparkleXrm.Sdk.OrganizationServiceProxy.execute(request);
        var relationship = response.relationshipMetadata;
        QUnit.equal(relationship.isCustomRelationship, false);
        QUnit.equal(relationship.schemaName, 'contact_customer_accounts');
        QUnit.equal(relationship.referencedAttribute, 'accountid');
        return true;
    },
    
    queryManyToManyRelationship: function SparkleXrm_UnitTests_MetadataQueryTests$queryManyToManyRelationship() {
        var request = new SparkleXrm.Sdk.RetrieveRelationshipRequest();
        request.name = 'accountleads_association';
        request.retrieveAsIfPublished = true;
        var response = SparkleXrm.Sdk.OrganizationServiceProxy.execute(request);
        var relationship = response.relationshipMetadata;
        QUnit.equal(relationship.isCustomRelationship, false);
        QUnit.equal(relationship.schemaName, 'accountleads_association');
        QUnit.equal(relationship.intersectEntityName, 'accountleads');
        return true;
    }
}


////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.UnitTests.OrganizationServiceProxyTests

SparkleXrm.UnitTests.OrganizationServiceProxyTests = function SparkleXrm_UnitTests_OrganizationServiceProxyTests() {
}
SparkleXrm.UnitTests.OrganizationServiceProxyTests.prototype = {
    
    crudTests: function SparkleXrm_UnitTests_OrganizationServiceProxyTests$crudTests() {
        var timeStamp = Date.get_now().toISOString() + Date.get_now().toTimeString();
        var contact1 = new SparkleXrm.Sdk.Entity('contact');
        contact1.setAttributeValue('lastname', 'Test Contact1 ');
        contact1.id = SparkleXrm.Sdk.OrganizationServiceProxy.create(contact1).toString();
        var contact2 = new SparkleXrm.Sdk.Entity('contact');
        contact2.setAttributeValue('lastname', 'Test Contact2 ');
        contact2.id = SparkleXrm.Sdk.OrganizationServiceProxy.create(contact2).toString();
        var recipient = new SparkleXrm.Sdk.Entity('activityparty');
        recipient.setAttributeValue('partyid', contact1.toEntityReference());
        var recipients = [];
        SparkleXrm.ArrayEx.add(recipients, recipient);
        var email = new SparkleXrm.Sdk.Entity('email');
        email.setAttributeValue('to', new SparkleXrm.Sdk.EntityCollection(recipients));
        email.setAttributeValue('subject', 'Unit Test email ' + timeStamp);
        email.setAttributeValue('id', SparkleXrm.Sdk.OrganizationServiceProxy.create(email));
        email.id = email.getAttributeValue('id').toString();
        var email2 = SparkleXrm.Sdk.OrganizationServiceProxy.retrieve('email', email.id, [ 'to', 'subject' ]);
        var to = email2.getAttributeValue('to');
        var recipient2 = new SparkleXrm.Sdk.Entity('activityparty');
        recipient2.setAttributeValue('partyid', contact2.toEntityReference());
        var toRecipients = to.entities.items();
        SparkleXrm.ArrayEx.add(toRecipients, recipient2);
        SparkleXrm.Sdk.OrganizationServiceProxy.update(email2);
        SparkleXrm.Sdk.OrganizationServiceProxy.delete_(email.logicalName, new SparkleXrm.Sdk.Guid(email.id));
        SparkleXrm.Sdk.OrganizationServiceProxy.delete_(contact1.logicalName, new SparkleXrm.Sdk.Guid(contact1.id));
        SparkleXrm.Sdk.OrganizationServiceProxy.delete_(contact2.logicalName, new SparkleXrm.Sdk.Guid(contact2.id));
        return true;
    },
    
    issue143_DateRetrieve: function SparkleXrm_UnitTests_OrganizationServiceProxyTests$issue143_DateRetrieve(assert) {
        var contact1 = new SparkleXrm.Sdk.Entity('contact');
        var contact3 = new SparkleXrm.Sdk.Entity('contact');
        try {
            contact1.setAttributeValue('lastname', 'Test Contact1 ');
            contact1.id = SparkleXrm.Sdk.OrganizationServiceProxy.create(contact1).toString();
            var contact2 = SparkleXrm.Sdk.OrganizationServiceProxy.retrieve('contact', contact1.id, [ 'createdon', 'modifiedon' ]);
            var created = contact2.getAttributeValue('createdon');
            QUnit.equal(Date.get_now().getFullYear(), created.getFullYear(), 'Year must be the same');
            contact3 = new SparkleXrm.Sdk.Entity('contact');
            contact3.setAttributeValue('birthdate', created);
            var done = assert.async();
            SparkleXrm.Sdk.OrganizationServiceProxy.beginCreate(contact3, function(state) {
                contact3.id = SparkleXrm.Sdk.OrganizationServiceProxy.endCreate(state).value;
                QUnit.ok(contact3.id != null, 'ID returned 2 ' + contact3.id);
                SparkleXrm.Sdk.OrganizationServiceProxy.delete_(contact3.logicalName, new SparkleXrm.Sdk.Guid(contact3.id));
                done();
            });
        }
        finally {
            SparkleXrm.Sdk.OrganizationServiceProxy.delete_(contact1.logicalName, new SparkleXrm.Sdk.Guid(contact1.id));
        }
        return true;
    }
}


SparkleXrm.UnitTests.ActionTests.registerClass('SparkleXrm.UnitTests.ActionTests');
SparkleXrm.UnitTests.AttributeTypeTests.registerClass('SparkleXrm.UnitTests.AttributeTypeTests');
SparkleXrm.UnitTests.CRUDTests.registerClass('SparkleXrm.UnitTests.CRUDTests');
SparkleXrm.UnitTests.FetchXmlTests.registerClass('SparkleXrm.UnitTests.FetchXmlTests');
FormContextTests.registerClass('FormContextTests');
SparkleXrm.UnitTests.FunctionTests.registerClass('SparkleXrm.UnitTests.FunctionTests');
SparkleXrm.UnitTests.LocalisationTests.registerClass('SparkleXrm.UnitTests.LocalisationTests');
SparkleXrm.UnitTests.MetadataQueryTests.registerClass('SparkleXrm.UnitTests.MetadataQueryTests');
SparkleXrm.UnitTests.OrganizationServiceProxyTests.registerClass('SparkleXrm.UnitTests.OrganizationServiceProxyTests');
})();

//! This script was generated using Script# v0.7.4.0
