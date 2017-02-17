// OptionSetMetadata.cs
//

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml;
using Xrm.Sdk;
using Xrm.Sdk.Metadata;

namespace SparkleXrm.Sdk.Metadata.Query
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
}
namespace Xrm.Sdk.Metadata
{
    [Imported]
    [IgnoreNamespace]
    [ScriptName("Object")]
    public class OptionSetMetadata
    {
        public List<OptionMetadata> Options;

    }
}
