// MetadataPropertiesExpression.cs
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Xrm.Sdk.Metadata.Query;

namespace SparkleXrm.Sdk.Metadata.Query
{
   
    public static partial class MetadataSerialiser
    {
        public static string SerialiseMetadataPropertiesExpression(MetadataPropertiesExpression item)
        {
            if (item != null)
            {
                string xml = @"
                <c:AllProperties>" + (item.AllProperties!=null ? item.AllProperties.ToString().ToLowerCase() : "false") + @"</c:AllProperties>
                <c:PropertyNames xmlns:d='http://schemas.microsoft.com/2003/10/Serialization/Arrays'>";
                if (item.PropertyNames != null)
                {
                    foreach (string value in item.PropertyNames)
                    {
                        xml = xml + @"<d:string>" + value + @"</d:string>";
                    }
                }
                xml = xml + @"
                </c:PropertyNames>
              ";

                return xml;
            }
            return "";
        }
    }
}
namespace Xrm.Sdk.Metadata.Query
{
    // Summary:
    //     Specifies the properties for which non-null values are returned from a query.
    //[DataContract(Name = "MetadataPropertiesExpression", Namespace = "http://schemas.microsoft.com/xrm/2011/Metadata/Query")]
    [Imported]
    [ScriptName("Object")]
    [IgnoreNamespace]
    public class MetadataPropertiesExpression 
    {
      
       

        // Summary:
        //     Gets or sets whether to retrieve all the properties of a metadata object.
        //
        // Returns:
        //     Type: Returns_Booleantrue to specify to retrieve all metadata object properties;
        //     false to retrieve only specified metadata object properties.

        public bool AllProperties;
        
        //
        // Summary:
        //     Gets or sets a collection of strings representing the metadata properties
        //     to retrieve.
        //
        // Returns:
        //     Type: Microsoft.Xrm.Sdk.DataCollection<T><Returns_String> The collection
        //     of strings representing the metadata properties to retrieve.

        public List<string> PropertyNames;
    }
}
