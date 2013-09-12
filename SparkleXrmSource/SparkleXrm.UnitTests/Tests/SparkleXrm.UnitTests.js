// SparkleXrm.UnitTests.js
(function(){
Type.registerNamespace('SparkleXrm.UnitTests');SparkleXrm.UnitTests.MetadataQueryTests=function(){}
SparkleXrm.UnitTests.MetadataQueryTests.prototype={queryAllMetaData:function(){var $0=new Xrm.Sdk.Messages.RetrieveMetadataChangesRequest();$0.query={};$0.query.criteria={};$0.query.criteria.filterOperator='Or';$0.query.criteria.conditions=[];var $1={};$1.conditionOperator='Equals';$1.propertyName='LogicalName';$1.value='account';$0.query.criteria.conditions.add($1);$0.query.properties={};$0.query.properties.propertyNames=['Attributes'];var $2={};$2.properties={};$2.properties.propertyNames=['OptionSet'];$0.query.attributeQuery=$2;var $3={};$2.criteria=$3;$3.filterOperator='And';$3.conditions=[];var $4=Xrm.Sdk.OrganizationServiceProxy.execute($0);return true;},queryAttributeDisplayNamesForTwoEntities:function(){var $0=new Xrm.Sdk.Metadata.Query.MetadataQueryBuilder();$0.addEntities(['account','contact'],['Attributes','DisplayName','DisplayCollectionName']);$0.addAttributes(['name','firstname','statecode','statuscode'],['DisplayName']);var $1=Xrm.Sdk.OrganizationServiceProxy.execute($0.request);return true;}}
SparkleXrm.UnitTests.MetadataQueryTests.registerClass('SparkleXrm.UnitTests.MetadataQueryTests');})();// This script was generated using Script# v0.7.4.0
