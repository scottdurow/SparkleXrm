// Relationship.cs
//

using System;
using System.Collections.Generic;

namespace Xrm.Sdk
{
    public class Relationship
    {
        public Relationship(string schemaName)
        {
            this.SchemaName = schemaName;
        }

        public EntityRole PrimaryEntityRole;
        public string SchemaName;
    }

    public enum EntityRole
    {
        // Summary:
        //     Specifies that the entity is the referencing entity. Value = 0.
        Referencing = 0,
        //
        // Summary:
        //     Specifies that the entity is the referenced entity. Value = 1.
        Referenced = 1,
    }
}
