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

        public string Entity1IntersectAttribute;
       
        public string Entity1LogicalName;
      
        //public AssociatedMenuConfiguration Entity2AssociatedMenuConfiguration { get; set; }

        public string Entity2IntersectAttribute;

        public string Entity2LogicalName;

        public string IntersectEntityName;
    }
}
