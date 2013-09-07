// MetadataFilterExpression.cs
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Xrm.Sdk.Metadata.Query
{
    public static partial class MetadataSerialiser
    {
        public static string SerialiseMetadataFilterExpression(MetadataFilterExpression item)
        {
            if (item != null)
            {
                string xml = @"<c:Conditions>";


                foreach (MetadataConditionExpression ex in item.Conditions)
                {
                    xml += MetadataSerialiser.SerialiseMetadataConditionExpression(ex);
                }

                xml = xml + @"</c:Conditions>
                        <c:FilterOperator>" + item.FilterOperator.ToString() + @"</c:FilterOperator>
                        <c:Filters />";
                return xml;
            }
            return "";
        }
    }

    // Summary:
    //     Specifies complex condition and logical filter expressions used for filtering
    //     the results of a metadata query.
    //[DataContract(Name = "MetadataFilterExpression", Namespace = "http://schemas.microsoft.com/xrm/2011/Metadata/Query")]
    [Imported]
    [ScriptName("Object")]
    [IgnoreNamespace]
    public class MetadataFilterExpression 
    {
       
       

        // Summary:
        //     Gets condition expressions that include metadata properties, condition operators
        //     and values.
        //
        // Returns:
        //     Type: Microsoft.Xrm.Sdk.DataCollection<T><Microsoft.Xrm.Sdk.Metadata.Query.MetadataConditionExpression>The
        //     collection of metadata condition expressions.

        public List<MetadataConditionExpression> Conditions;
        
        //
        // Summary:
        //     Gets or sets the logical AND/OR filter operator.
        //
        // Returns:
        //     Type: Microsoft.Xrm.Sdk.Query.LogicalOperatorThe filter operator.
     
        public LogicalOperator FilterOperator;
        //
        // Summary:
        //     Gets a collection of logical filter expressions that filter the results of
        //     the metadata query.
        //
        // Returns:
        //     Type: Microsoft.Xrm.Sdk.DataCollection<T><Microsoft.Xrm.Sdk.Metadata.Query.MetadataFilterExpression>.
       
        public List<MetadataFilterExpression> Filters;
    }


    // Summary:
    //     Contains the possible values for an operator in a Microsoft.Xrm.Sdk.Query.FilterExpression.
    //[DataContract(Name = "LogicalOperator", Namespace = "http://schemas.microsoft.com/xrm/2011/Contracts")]
    [NamedValues]
    public enum LogicalOperator
    {
        // Summary:
        //     A logical AND operation is performed. Value = 0.
        [ScriptName("And")]
        And = 0,
        //
        // Summary:
        //     A logical OR operation is performed. Value = 1.
        [ScriptName("Or")]
        Or = 1,
    }
}
