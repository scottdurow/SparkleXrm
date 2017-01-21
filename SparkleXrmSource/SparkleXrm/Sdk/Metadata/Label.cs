// Label.cs
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
        public static Label DeSerialiseLabel(Label item, XmlNode metaData)
        {
            item.LocalizedLabels = new List<LocalizedLabel>();
            XmlNode labels =  XmlHelper.SelectSingleNode(metaData, "LocalizedLabels");
            if (labels!=null && labels.ChildNodes != null)
            {
                foreach (XmlNode label in labels.ChildNodes)
                {
                    item.LocalizedLabels.Add(MetadataSerialiser.DeSerialiseLocalizedLabel(new LocalizedLabel(), label));
                }
                item.UserLocalizedLabel = MetadataSerialiser.DeSerialiseLocalizedLabel(new LocalizedLabel(), XmlHelper.SelectSingleNode(metaData, "UserLocalizedLabel"));
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
    public class Label
    {
        public List<LocalizedLabel> LocalizedLabels;
        public LocalizedLabel UserLocalizedLabel;

    }
}
