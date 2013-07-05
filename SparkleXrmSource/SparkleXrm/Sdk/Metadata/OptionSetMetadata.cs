// OptionSetMetadata.cs
//

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml;

namespace Xrm.Sdk.Metadata
{
    public static partial class MetadataSerialiser
    {
        public static OptionSetMetadata DeSerialiseOptionSetMetadata(OptionSetMetadata item, XmlNode metaData)
        {
            XmlNode options = XmlHelper.SelectSingleNode(metaData, "Options");
            if (options != null)
            {
                item.Options = new List<OptionMetadata>();
                foreach (XmlNode option in options.ChildNodes)
                {
                    item.Options.Add(MetadataSerialiser.DeSerialiseOptionMetadata(new OptionMetadata(), option));
                }
            }
            return item;
        }
    }
    [Imported]
    [IgnoreNamespace]
    [ScriptName("Object")]
    public class OptionSetMetadata
    {
        public List<OptionMetadata> Options;

    }
}
