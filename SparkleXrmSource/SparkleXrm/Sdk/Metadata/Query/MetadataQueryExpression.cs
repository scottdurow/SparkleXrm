

using System.Runtime.CompilerServices;
using Xrm.Sdk.Metadata.Query;
namespace SparkleXrm.Sdk.Metadata.Query
{
    public static partial class MetadataSerialiser
    {
        public static string SerialiseMetadataQueryExpression(MetadataQueryExpression item)
        {
            if (item != null)
            {
                string xml =
                    @"<c:Criteria>"
                    + MetadataSerialiser.SerialiseMetadataFilterExpression(item.Criteria) +
                         @"</c:Criteria>
                    <c:Properties>"
                         + MetadataSerialiser.SerialiseMetadataPropertiesExpression(item.Properties)
                         + @" </c:Properties>";
                return xml;
            }
            return "";
        }
    }
}
namespace Xrm.Sdk.Metadata.Query
{
    // Summary:
    //     Represents the abstract base class for constructing a metadata query.
    //[DataContract(Name = "MetadataQueryExpression", Namespace = "http://schemas.microsoft.com/xrm/2011/Metadata/Query")]
    //[KnownType(typeof(AttributeQueryExpression))]
    //[KnownType(typeof(EntityQueryExpression))]
    //[KnownType(typeof(LabelQueryExpression))]
    //[KnownType(typeof(RelationshipQueryExpression))]
    [Imported]
    [ScriptName("Object")]
    [IgnoreNamespace]
    public class MetadataQueryExpression 
    {


        // Summary:
        //     Gets or sets the filter criteria for the metadata query.
        //
        // Returns:
        //     Returns Microsoft.Xrm.Sdk.Metadata.Query.MetadataFilterExpressionThe filter
        //     criteria for the metadata query.
        [PreserveCase]
        public MetadataFilterExpression Criteria;
        //
        // Summary:
        //     Gets or sets the properties to be returned by the query.
        //
        // Returns:
        //     Returns Microsoft.Xrm.Sdk.Metadata.Query.MetadataPropertiesExpressionThe
        //     properties to be returned by the query.
        [PreserveCase]
        public MetadataPropertiesExpression Properties;
    }
}
