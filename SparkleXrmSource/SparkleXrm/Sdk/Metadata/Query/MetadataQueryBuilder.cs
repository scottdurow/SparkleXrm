// MetadataQueryBuilder.cs
//

using System;
using System.Collections.Generic;
using Xrm.Sdk.Messages;

namespace Xrm.Sdk.Metadata.Query
{
    public class MetadataQueryBuilder
    {
        public RetrieveMetadataChangesRequest Request;
        public MetadataQueryBuilder()
        {
            Request = new RetrieveMetadataChangesRequest();
            Request.Query = new EntityQueryExpression();

            Request.Query.Criteria = new MetadataFilterExpression();
            Request.Query.Criteria.FilterOperator = LogicalOperator.Or;
            Request.Query.Criteria.Conditions = new List<MetadataConditionExpression>();
        }

        public void AddEntities(List<string> entityLogicalNames,List<string> propertiesToReturn)
        {
            Request.Query.Criteria = new MetadataFilterExpression();
            Request.Query.Criteria.FilterOperator = LogicalOperator.Or;
            Request.Query.Criteria.Conditions = new List<MetadataConditionExpression>();

            foreach (string entity in entityLogicalNames)
            {
                MetadataConditionExpression condition = new MetadataConditionExpression();
                condition.ConditionOperator = MetadataConditionOperator.Equals;
                condition.PropertyName = "LogicalName";
                condition.Value = entity;
                Request.Query.Criteria.Conditions.Add(condition);
            }

            Request.Query.Properties = new MetadataPropertiesExpression();
            Request.Query.Properties.PropertyNames = propertiesToReturn;
        }

        public void AddAttributes(List<string> attributeLogicalNames, List<string> propertiesToReturn)
        {

            // Attribute Query Properties - Which Properties to return
            AttributeQueryExpression attributeQuery = new AttributeQueryExpression();
            attributeQuery.Properties = new MetadataPropertiesExpression();
            attributeQuery.Properties.PropertyNames = propertiesToReturn;
            
            Request.Query.AttributeQuery = attributeQuery;

            // Attribute Query Criteria - Which Attributes to return
            MetadataFilterExpression critiera = new MetadataFilterExpression();
            attributeQuery.Criteria = critiera;
            critiera.FilterOperator = LogicalOperator.Or;
            critiera.Conditions = new List<MetadataConditionExpression>();

            foreach (string attribute in attributeLogicalNames)
            {
                MetadataConditionExpression condition = new MetadataConditionExpression();
                condition.PropertyName = "LogicalName";
                condition.ConditionOperator = MetadataConditionOperator.Equals;
                condition.Value = attribute;
                critiera.Conditions.Add(condition);
            }

        }
        public void SetLanguage(int lcid)
        {
            Request.Query.LabelQuery = new LabelQueryExpression();
            Request.Query.LabelQuery.FilterLanguages = new List<int>();
            Request.Query.LabelQuery.FilterLanguages.Add(lcid);
        }
    }
}
