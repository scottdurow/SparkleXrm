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
        [PreserveCase]
        public string ReferencedAttribute;
        [PreserveCase]
        public string ReferencedEntity;
        [PreserveCase]
        public string ReferencingAttribute;
        [PreserveCase]
        public string ReferencingEntity;
    }
}
