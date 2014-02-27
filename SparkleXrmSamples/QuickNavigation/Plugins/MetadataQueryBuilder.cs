using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Metadata.Query;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickNavigation.Plugins
{
    public class MetadataQueryBuilder
    {
        private RetrieveMetadataChangesRequest _request;
        private List<string> _entityLogicalNames;
        private List<string> _entityPropertiesToReturn;
        private List<string> _attributePropertiesToReturn;
        private List<string> _attributeLogicalNames;

        public MetadataQueryBuilder()
        {
            _request = new RetrieveMetadataChangesRequest();
            _request.Query = new EntityQueryExpression();

            _request.Query.Criteria = new MetadataFilterExpression();
            _request.Query.Criteria.FilterOperator = LogicalOperator.Or;
           
        }

        public void AddEntities(List<string> entityLogicalNames, List<string> propertiesToReturn)
        {
            _entityLogicalNames = entityLogicalNames;
            _entityPropertiesToReturn = propertiesToReturn;

            _request.Query.Criteria = new MetadataFilterExpression();
            _request.Query.Criteria.FilterOperator = LogicalOperator.Or;

            foreach (string entity in entityLogicalNames)
            {
                MetadataConditionExpression condition = new MetadataConditionExpression();
                condition.ConditionOperator = MetadataConditionOperator.Equals;
                condition.PropertyName = "LogicalName";
                condition.Value = entity;
                _request.Query.Criteria.Conditions.Add(condition);
            }

            if (propertiesToReturn != null)
            {
                _request.Query.Properties = new MetadataPropertiesExpression();
                _request.Query.Properties.PropertyNames.AddRange(propertiesToReturn);
            }
        }

        public void AddAttributes(List<string> attributeLogicalNames, List<string> propertiesToReturn)
        {
            this._attributeLogicalNames = attributeLogicalNames;
            this._attributePropertiesToReturn = propertiesToReturn;

            // Attribute Query Properties - Which Properties to return
            AttributeQueryExpression attributeQuery = new AttributeQueryExpression();
            attributeQuery.Properties = new MetadataPropertiesExpression();
            attributeQuery.Properties.PropertyNames.AddRange(propertiesToReturn);

            _request.Query.AttributeQuery = attributeQuery;

            // Attribute Query Criteria - Which Attributes to return
            MetadataFilterExpression critiera = new MetadataFilterExpression();
            attributeQuery.Criteria = critiera;
            critiera.FilterOperator = LogicalOperator.Or;


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
            _request.Query.LabelQuery = new LabelQueryExpression();
            _request.Query.LabelQuery.FilterLanguages.Add(lcid);
        }
        public EntityMetadataCollection Execute(IOrganizationService service)
        {
          
            return ((RetrieveMetadataChangesResponse)service.Execute(_request)).EntityMetadata;
          
        }

       
    }
}
