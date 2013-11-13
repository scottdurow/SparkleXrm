// MetadataQueryTests.cs
//

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Html;
using System.Testing;
using Xrm.Sdk;
using Xrm.Sdk.Messages;
using Xrm.Sdk.Metadata;
using Xrm.Sdk.Metadata.Query;

namespace SparkleXrm.UnitTests
{
   
    public class MetadataQueryTests 
    {
        public bool QueryAllMetaData()
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
            return true;
        }
        public bool QueryNameAttributeForAccount()
        {
            MetadataQueryBuilder builder = new MetadataQueryBuilder();
            builder.AddEntities(new List<string>("account"), new List<string>("PrimaryNameAttribute"));
           
            RetrieveMetadataChangesResponse response = (RetrieveMetadataChangesResponse)OrganizationServiceProxy.Execute(builder.Request);
            Assert.AreEqual(response.EntityMetadata[0].PrimaryNameAttribute, "name");
            return true;

        }
        public bool QueryAttributeDisplayNamesForTwoEntities()
        {
            MetadataQueryBuilder builder = new MetadataQueryBuilder();
            builder.AddEntities(new List<string>("account", "contact"), new List<string>("Attributes","DisplayName","DisplayCollectionName"));
            builder.AddAttributes(new List<string>("name", "firstname", "statecode", "statuscode"),new List<string>("DisplayName"));

            RetrieveMetadataChangesResponse response = (RetrieveMetadataChangesResponse)OrganizationServiceProxy.Execute(builder.Request);
            return true;

        }
        public bool QueryOneToManyRelationship()
        {
            RetrieveRelationshipRequest request = new RetrieveRelationshipRequest();
            request.Name = "contact_customer_accounts";
            request.RetrieveAsIfPublished = true;

            RetrieveRelationshipResponse response = (RetrieveRelationshipResponse) OrganizationServiceProxy.Execute(request);
            OneToManyRelationshipMetadata relationship = (OneToManyRelationshipMetadata)response.RelationshipMetadata;
            Assert.AreEqual(relationship.IsCustomRelationship, false);
            Assert.AreEqual(relationship.SchemaName,"contact_customer_accounts");
            Assert.AreEqual(relationship.ReferencedAttribute, "accountid");
            return true;


        }

        public bool QueryManyToManyRelationship()
        {
            RetrieveRelationshipRequest request = new RetrieveRelationshipRequest();
            request.Name = "accountleads_association";
            request.RetrieveAsIfPublished = true;

            RetrieveRelationshipResponse response = (RetrieveRelationshipResponse)OrganizationServiceProxy.Execute(request);
            ManyToManyRelationshipMetadata relationship = (ManyToManyRelationshipMetadata)response.RelationshipMetadata;
            Assert.AreEqual(relationship.IsCustomRelationship, false);
            Assert.AreEqual(relationship.SchemaName, "accountleads_association");
            Assert.AreEqual(relationship.IntersectEntityName, "accountleads");
            
            return true;


        }

        
    }
}
