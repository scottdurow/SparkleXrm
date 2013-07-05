// AttributeMetadata.cs
//

using System.Runtime.CompilerServices;
using System.Xml;

namespace Xrm.Sdk.Metadata
{
    public static partial class MetadataSerialiser
    {
        public static AttributeMetadata DeSerialiseAttributeMetadata(AttributeMetadata item, XmlNode entity)
        {
            return item;
        }

    }

    [Imported]
    [IgnoreNamespace]
    [ScriptName("Object")]
    public class AttributeMetadata
    {
    }
}
