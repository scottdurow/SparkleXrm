// LabelQueryExpression.cs
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Xrm.Sdk.Metadata.Query
{
    public static partial class MetadataSerialiser
    {
        public static string SerialiseLabelQueryExpression(LabelQueryExpression item)
        {
            if (item != null)
            {
                string xml = @"<c:FilterLanguages xmlns:d='http://schemas.microsoft.com/2003/10/Serialization/Arrays'>";
                foreach (int lcid in item.FilterLanguages)
                {
                    xml = xml + @"<d:int>" + lcid.ToString() + @"</d:int>";
                }
                xml = xml + @"</c:FilterLanguages>";
                return xml;
            }
            else
                return "";
        }
    }

    // Summary:
    //     Defines the languages for the labels to be retrieved for metadata items that
    //     have labels.
    //[DataContract(Name = "LabelQueryExpression", Namespace = "http://schemas.microsoft.com/xrm/2011/Metadata/Query")]
    [Imported]
    [ScriptName("Object")]
    [IgnoreNamespace]
    public class LabelQueryExpression 
    {
      

        // Summary:
        //     Gets the LCID values for localized labels to be retrieved for metadata items.
        //
        // Returns:
        //     Type Microsoft.Xrm.Sdk.DataCollection<T><Returns_Int32>The LCID values for
        //     localized labels to be retrieved for metadata items.

        public List<int> FilterLanguages;
    }
}
