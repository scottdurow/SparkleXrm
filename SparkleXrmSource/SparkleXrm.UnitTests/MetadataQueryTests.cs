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
            QUnit.Test("QueryAttributeDisplayNamesForTwoEntities", MetadataQueryTests.QueryAttributeDisplayNamesForTwoEntities);
            QUnit.Test("QueryNameAttributeForAccount", MetadataQueryTests.QueryNameAttributeForAccount);
            QUnit.Test("QueryManyToManyRelationship", MetadataQueryTests.QueryManyToManyRelationship);
            QUnit.Test("QueryOneToManyRelationship", MetadataQueryTests.QueryOneToManyRelationship);
        }

        public void QueryAllMetaData(Assert assert)
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
