// Label.cs
//

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml;

namespace Xrm.Sdk.Metadata
{
    public static partial class MetadataSerialiser
    {
        public static Label DeSerialiseLabel(Label item, XmlNode metaData)
        {
            item.LocalizedLabels = new List<LocalizedLabel>();
            foreach (XmlNode label in XmlHelper.SelectSingleNode(metaData, "LocalizedLabels").ChildNodes)
            {
                item.LocalizedLabels.Add(MetadataSerialiser.DeSerialiseLocalizedLabel(new LocalizedLabel(), label));
            }
            item.UserLocalizedLabel = MetadataSerialiser.DeSerialiseLocalizedLabel(new LocalizedLabel(), XmlHelper.SelectSingleNode(metaData, "UserLocalizedLabel"));
            return item;
        }
    }

    [Imported]
    [IgnoreNamespace]
    [ScriptName("Object")]
    public class Label
    {
        public List<LocalizedLabel> LocalizedLabels;
        public LocalizedLabel UserLocalizedLabel;

    }
}
