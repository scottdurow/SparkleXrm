using System.Runtime.CompilerServices;
using System.Xml;

namespace Xrm.Sdk.Metadata
{
    public static partial class MetadataSerialiser
    {
        public static LocalizedLabel DeSerialiseLocalizedLabel(LocalizedLabel item, XmlNode metaData)
        {
            item.Label = XmlHelper.SelectSingleNodeValue(metaData, "Label");
            item.LanguageCode = int.Parse(XmlHelper.SelectSingleNodeValue(metaData, "LanguageCode"));
            return item;
        }
    }

    [Imported]
    [IgnoreNamespace]
    [ScriptName("Object")]
    public class LocalizedLabel
    {
        public string Label;
        public int LanguageCode;
        
    }
}
