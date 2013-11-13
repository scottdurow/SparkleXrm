// OneToManyRelationshipMetadata.cs
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Xrm.Sdk.Metadata
{
    [Imported]
    [IgnoreNamespace]
    [ScriptName("Object")]
    public sealed class OneToManyRelationshipMetadata : RelationshipMetadataBase
    {
        //public AssociatedMenuConfiguration AssociatedMenuConfiguration { get; set; }
        //public CascadeConfiguration CascadeConfiguration { get; set; }
      
        public string ReferencedAttribute;
        public string ReferencedEntity;   
        public string ReferencingAttribute;      
        public string ReferencingEntity;
    }
}
