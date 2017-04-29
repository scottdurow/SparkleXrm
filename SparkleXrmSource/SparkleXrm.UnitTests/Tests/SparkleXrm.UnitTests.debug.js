//! SparkleXrm.UnitTests.debug.js
//

(function() {

Type.registerNamespace('SparkleXrm.UnitTests');

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
        contact1.setAttributeValue('lastname', 'Test Contact1 ' + timeStamp);
        contact1.id = SparkleXrm.Sdk.OrganizationServiceProxy.create(contact1).toString();
        var contact2 = new SparkleXrm.Sdk.Entity('contact');
        contact2.setAttributeValue('lastname', 'Test Contact2 ' + timeStamp);
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
        ss.Debug.assert(to.get_entities().get_count() === 1);
        var recipient2 = new SparkleXrm.Sdk.Entity('activityparty');
        recipient2.setAttributeValue('partyid', contact2.toEntityReference());
        var toRecipients = to.get_entities().items();
        SparkleXrm.ArrayEx.add(toRecipients, recipient2);
        SparkleXrm.Sdk.OrganizationServiceProxy.update(email2);
        SparkleXrm.Sdk.OrganizationServiceProxy.delete_(email.logicalName, new SparkleXrm.Sdk.Guid(email.id));
        SparkleXrm.Sdk.OrganizationServiceProxy.delete_(contact1.logicalName, new SparkleXrm.Sdk.Guid(contact1.id));
        SparkleXrm.Sdk.OrganizationServiceProxy.delete_(contact2.logicalName, new SparkleXrm.Sdk.Guid(contact2.id));
        return true;
    }
}


SparkleXrm.UnitTests.LocalisationTests.registerClass('SparkleXrm.UnitTests.LocalisationTests');
SparkleXrm.UnitTests.MetadataQueryTests.registerClass('SparkleXrm.UnitTests.MetadataQueryTests');
SparkleXrm.UnitTests.OrganizationServiceProxyTests.registerClass('SparkleXrm.UnitTests.OrganizationServiceProxyTests');
})();

//! This script was generated using Script# v0.7.4.0
