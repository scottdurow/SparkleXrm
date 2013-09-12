//! SparkleXrm.UnitTests.debug.js
//

(function() {

Type.registerNamespace('SparkleXrm.UnitTests');

////////////////////////////////////////////////////////////////////////////////
// SparkleXrm.UnitTests.MetadataQueryTests

SparkleXrm.UnitTests.MetadataQueryTests = function SparkleXrm_UnitTests_MetadataQueryTests() {
}
SparkleXrm.UnitTests.MetadataQueryTests.prototype = {
    
    queryAllMetaData: function SparkleXrm_UnitTests_MetadataQueryTests$queryAllMetaData() {
        var request = new Xrm.Sdk.Messages.RetrieveMetadataChangesRequest();
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
        var response = Xrm.Sdk.OrganizationServiceProxy.execute(request);
        return true;
    },
    
    queryAttributeDisplayNamesForTwoEntities: function SparkleXrm_UnitTests_MetadataQueryTests$queryAttributeDisplayNamesForTwoEntities() {
        var builder = new Xrm.Sdk.Metadata.Query.MetadataQueryBuilder();
        builder.addEntities(['account', 'contact'], ['Attributes', 'DisplayName', 'DisplayCollectionName']);
        builder.addAttributes(['name', 'firstname', 'statecode', 'statuscode'], ['DisplayName']);
        var response = Xrm.Sdk.OrganizationServiceProxy.execute(builder.request);
        return true;
    }
}


SparkleXrm.UnitTests.MetadataQueryTests.registerClass('SparkleXrm.UnitTests.MetadataQueryTests');
})();

//! This script was generated using Script# v0.7.4.0
