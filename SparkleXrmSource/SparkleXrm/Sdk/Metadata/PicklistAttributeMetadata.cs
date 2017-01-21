// PicklistAttributeMetadata.cs
//

using System.Runtime.CompilerServices;
using System.Xml;
using Xrm.Sdk;
using Xrm.Sdk.Metadata;

namespace SparkleXrm.Sdk.Metadata.Query
{
   
    public static partial class MetadataSerialiser
    {
        public static PicklistAttributeMetadata DeSerialisePicklistAttributeMetadata(PicklistAttributeMetadata item, XmlNode metaData)
        {
            XmlNode options = XmlHelper.SelectSingleNode(metaData, "OptionSet");
            if (options != null)
                item.OptionSet = MetadataSerialiser.DeSerialiseOptionSetMetadata(new OptionSetMetadata(), options);
            return item;
        }
    }
}
namespace Xrm.Sdk.Metadata
{ 

    [Imported]
    [IgnoreNamespace]
    [ScriptName("Object")]
    public class PicklistAttributeMetadata : EnumAttributeMetadata
    {
        
       
    }
}
