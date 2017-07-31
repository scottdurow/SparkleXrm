// ManyToManyRelationshipMetadata.cs
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Xrm.Sdk.Metadata
{
    [Imported]
    [IgnoreNamespace]
    [ScriptName("Object")]
    public sealed class ManyToManyRelationshipMetadata : RelationshipMetadataBase
    {
        public ManyToManyRelationshipMetadata()
        {
        }

        //public AssociatedMenuConfiguration Entity1AssociatedMenuConfiguration { get; set; }
        [PreserveCase]
        public string Entity1IntersectAttribute;
        [PreserveCase]
        public string Entity1LogicalName;

        //public AssociatedMenuConfiguration Entity2AssociatedMenuConfiguration { get; set; }
        [PreserveCase]
        public string Entity2IntersectAttribute;
        [PreserveCase]
        public string Entity2LogicalName;
        [PreserveCase]
        public string IntersectEntityName;
    }
}
