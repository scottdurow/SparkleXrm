// MetadataQueryTests.cs
//

using QUnitApi;
using SparkleXrm.Sdk.Metadata.Query;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Html;

using Xrm.Sdk;
using Xrm.Sdk.Messages;
using Xrm.Sdk.Metadata;
using Xrm.Sdk.Metadata.Query;

namespace SparkleXrm.UnitTests
{
   
    public class MetadataQueryTests 
    {
        static MetadataQueryTests()
        {
            QUnit.Module("MetadataQueryTests", null);
            QUnit.Test("EntityMetadataQuery_EntityOnly", MetadataQueryTests.EntityMetadataQuery_EntityOnly);
            QUnit.Test("EntityMetadataQuery_EntityAndAttributes", MetadataQueryTests.EntityMetadataQuery_EntityAndAttributes);
            QUnit.Test("AttributeMetadataQuery_Picklist", MetadataQueryTests.AttributeMetadataQuery_Picklist);
            
            //QUnit.Test("QueryAllMetaData", MetadataQueryTests.QueryAllMetaData);

            QUnit.Test("QueryAttributeDisplayNamesForTwoEntities", MetadataQueryTests.QueryAttributeDisplayNamesForTwoEntities);
            QUnit.Test("QueryNameAttributeForAccount", MetadataQueryTests.QueryNameAttributeForAccount);
            QUnit.Test("QueryManyToManyRelationship", MetadataQueryTests.QueryManyToManyRelationship);
            QUnit.Test("QueryOneToManyRelationship", MetadataQueryTests.QueryOneToManyRelationship);
        }

        public static void EntityMetadataQuery_EntityOnly(Assert assert)
        {
            assert.Expect(1);
            RetrieveEntityRequest request = new RetrieveEntityRequest();
            request.EntityFilters = EntityFilters.Entity;// | EntityFilters.Attributes;
            request.LogicalName = "account";
            request.MetadataId = Guid.Empty;
            RetrieveEntityResponse response = (RetrieveEntityResponse)OrganizationServiceProxy.Execute(request);
            assert.Equal(response.EntityMetadata.LogicalName, "account", "Metadata returned");

        }

        public static void EntityMetadataQuery_EntityAndAttributes(Assert assert)
        {
            assert.Expect(2);
            RetrieveEntityRequest request = new RetrieveEntityRequest();
            request.EntityFilters = EntityFilters.Entity | EntityFilters.Attributes;
            request.LogicalName = "account";
            request.MetadataId = Guid.Empty;
            RetrieveEntityResponse response = (RetrieveEntityResponse)OrganizationServiceProxy.Execute(request);
            assert.Equal(response.EntityMetadata.LogicalName, "account", "Metadata returned");
            assert.Ok(response.EntityMetadata.Attributes.Count > 10, "Attributes returned");
        }

        public static void AttributeMetadataQuery_Picklist(Assert assert)
        {
            assert.Expect(1);
            RetrieveAttributeRequest request = new RetrieveAttributeRequest();
            request.EntityLogicalName = "account";
            request.LogicalName = "address1_shippingmethodcode";
           
            request.RetrieveAsIfPublished = true;


            RetrieveAttributeResponse response = (RetrieveAttributeResponse)OrganizationServiceProxy.Execute(request);
     
            assert.Ok(((PicklistAttributeMetadata)response.AttributeMetadata).OptionSet.Options.Count > 0, "Optionsets returned");

        }
        public static void QueryAllMetaData(Assert assert)
        {


            RetrieveMetadataChangesRequest request = new RetrieveMetadataChangesRequest();
            request.Query = new EntityQueryExpression();

            request.Query.Criteria = new MetadataFilterExpression();
            request.Query.Criteria.FilterOperator = LogicalOperator.Or;
            request.Query.Criteria.Conditions = new List<MetadataConditionExpression>();

            // Which entitiy to return
            MetadataConditionExpression condition = new MetadataConditionExpression();
            condition.ConditionOperator = MetadataConditionOperator.Equals;
            condition.PropertyName = "LogicalName";
            condition.Value = "account";
            request.Query.Criteria.Conditions.Add(condition);

            request.Query.Properties = new MetadataPropertiesExpression();
            request.Query.Properties.PropertyNames = new List<string>("Attributes");

            // Attribute Query Properties - Which Properties to return
            AttributeQueryExpression attributeQuery = new AttributeQueryExpression();
            attributeQuery.Properties = new MetadataPropertiesExpression();
            attributeQuery.Properties.PropertyNames = new List<string>("OptionSet");
            //attributeQuery.Properties.AllProperties = true;

            request.Query.AttributeQuery = attributeQuery;

           
            

            // Attribute Query Criteria - Which Attributes to return
            MetadataFilterExpression critiera = new MetadataFilterExpression();
            attributeQuery.Criteria = critiera;
            critiera.FilterOperator = LogicalOperator.And;
            critiera.Conditions = new List<MetadataConditionExpression>();
           

            RetrieveMetadataChangesResponse response = (RetrieveMetadataChangesResponse)OrganizationServiceProxy.Execute(request);
          
        }

        public static void QueryNameAttributeForAccount(Assert assert)
        {
            MetadataQueryBuilder builder = new MetadataQueryBuilder();
            builder.AddEntities(new List<string>("account"), new List<string>("PrimaryNameAttribute"));
           
            RetrieveMetadataChangesResponse response = (RetrieveMetadataChangesResponse)OrganizationServiceProxy.Execute(builder.Request);
            assert.Equal(response.EntityMetadata[0].PrimaryNameAttribute, "name","Name equal");
            

        }
        public static void QueryAttributeDisplayNamesForTwoEntities(Assert assert)
        {
            assert.Expect(1);
            MetadataQueryBuilder builder = new MetadataQueryBuilder();
            builder.AddEntities(new List<string>("account", "contact"), new List<string>("Attributes","DisplayName","DisplayCollectionName"));
            builder.AddAttributes(new List<string>("name", "firstname", "statecode", "statuscode"),new List<string>("DisplayName"));

            RetrieveMetadataChangesResponse response = (RetrieveMetadataChangesResponse)OrganizationServiceProxy.Execute(builder.Request);
            assert.Equal(response.EntityMetadata.Count, 2, "Metadata Count");

        }
        public static void QueryOneToManyRelationship(Assert assert)
        {
            RetrieveRelationshipRequest request = new RetrieveRelationshipRequest();
            request.Name = "contact_customer_accounts";
            request.RetrieveAsIfPublished = true;

            RetrieveRelationshipResponse response = (RetrieveRelationshipResponse) OrganizationServiceProxy.Execute(request);
            OneToManyRelationshipMetadata relationship = (OneToManyRelationshipMetadata)response.RelationshipMetadata;
            assert.Equal(relationship.IsCustomRelationship, false,"IsCustomRelationship");
            assert.Equal(relationship.SchemaName,"contact_customer_accounts","Schemaname");
            assert.Equal(relationship.ReferencedAttribute, "accountid","ReferencedAttribute");
           


        }

        public static void QueryManyToManyRelationship(Assert assert)
        {
            RetrieveRelationshipRequest request = new RetrieveRelationshipRequest();
            request.Name = "accountleads_association";
            request.RetrieveAsIfPublished = true;

            RetrieveRelationshipResponse response = (RetrieveRelationshipResponse)OrganizationServiceProxy.Execute(request);
            ManyToManyRelationshipMetadata relationship = (ManyToManyRelationshipMetadata)response.RelationshipMetadata;
            assert.Equal(relationship.IsCustomRelationship, false, "IsCustomRelationship");
            assert.Equal(relationship.SchemaName, "accountleads_association","Schemaname");
            assert.Equal(relationship.IntersectEntityName, "accountleads","InteresectEntityName");
            
          


        }

        
    }
}
