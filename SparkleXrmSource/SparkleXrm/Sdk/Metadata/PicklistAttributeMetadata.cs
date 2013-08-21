// PicklistAttributeMetadata.cs
//

using System.Runtime.CompilerServices;
using System.Xml;

namespace Xrm.Sdk.Metadata
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
    

    [Imported]
    [IgnoreNamespace]
    [ScriptName("Object")]
    public class PicklistAttributeMetadata : EnumAttributeMetadata
    {
        
       
    }
}
