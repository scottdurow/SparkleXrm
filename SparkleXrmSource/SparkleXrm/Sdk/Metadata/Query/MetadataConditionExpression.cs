// MetadataConditionExpression.cs
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Xrm.Sdk.Metadata.Query
{

    public static partial class MetadataSerialiser
    {
        public static string SerialiseMetadataConditionExpression(MetadataConditionExpression item)
        {
            return @"<c:MetadataConditionExpression>
                            <c:ConditionOperator>" + item.ConditionOperator.ToString() + @"</c:ConditionOperator>
                            <c:PropertyName>" + item.PropertyName + @"</c:PropertyName>
                            <c:Value i:type='d:string' xmlns:d='http://www.w3.org/2001/XMLSchema'>" + item.Value + @"</c:Value>
                          </c:MetadataConditionExpression>";
        }
    }
    // Summary:
    //     Contains a condition expression used to filter the results of the metadata
    //     query.
    //[DataContract(Name = "MetadataConditionExpression", Namespace = "http://schemas.microsoft.com/xrm/2011/Metadata/Query")]
    //[KnownType("GetKnownConditionValueTypes")]
    [Imported]
    [ScriptName("Object")]
    [IgnoreNamespace]
    public class MetadataConditionExpression 
    {
      

        // Summary:
        //     Gets or sets the condition operator.
        //
        // Returns:
        //     Type: Microsoft.Xrm.Sdk.Metadata.Query.MetadataConditionOperator. The condition
        //     operator.
        public MetadataConditionOperator ConditionOperator;
        
        //
        // Summary:
        //     Gets or sets the name of the metadata property in the condition expression.
        //
        // Returns:
        //     Type: Returns_String The name of the metadata property in the condition expression.
        public string PropertyName;
        //
        // Summary:
        //     Gets or sets the value for the metadata property.
        //
        // Returns:
        //     Type: Returns_Object The value for the metadata property.
        public object Value;
    }

    // Summary:
    //     Describes the type of comparison for two values in a metadata condition expression.
    //[DataContract(Name = "MetadataConditionOperator", Namespace = "http://schemas.microsoft.com/xrm/2011/Metadata/Query")]
      [NamedValues]
    
    public enum MetadataConditionOperator
    {
        // Summary:
        //     The values are compared for equality. Value = 0.
        [ScriptName("Equals")]
        Equals = 0,
        //
        // Summary:
        //     The two values are not equal. Value = 1.
       [ScriptName("NotEquals")]
        NotEquals = 1,
        //
        // Summary:
        //     The value exists in a list of values. Value = 2.
        [ScriptName("In")]
        In_ = 2,
        //
        // Summary:
        //     The given value is not matched to a value in a list. Value = 3.
        [ScriptName("NotIn")]
        NotIn = 3,
        //
        // Summary:
        //     The value is greater than the compared value. Value = 4.
        [ScriptName("GreaterThan")]
        GreaterThan = 4,
        //
        // Summary:
        //     The value is less than the compared value. Value = 5.
        [ScriptName("LessThan")]
        LessThan = 5,
    }
}
