// OptionMetadata.cs
//

using System.Runtime.CompilerServices;
using System.Xml;
using Xrm.Sdk;
using Xrm.Sdk.Metadata;


namespace SparkleXrm.Sdk.Metadata.Query
{
   
    public static partial class MetadataSerialiser
    {
        public static OptionMetadata DeSerialiseOptionMetadata(OptionMetadata item, XmlNode metaData)
        {
            item.Value = int.Parse(XmlHelper.SelectSingleNodeValue(metaData, "Value"));
            item.Label = MetadataSerialiser.DeSerialiseLabel(new Label(),XmlHelper.SelectSingleNode(metaData, "Label"));
            return item;
        }
    }
}
namespace Xrm.Sdk.Metadata
{
    [Imported]
    [IgnoreNamespace]
    [ScriptName("Object")]
    public class OptionMetadata
    {
        public int? Value;
        public Label Label;
        
    }
}
