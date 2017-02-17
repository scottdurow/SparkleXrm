// Relationship.cs
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Xrm.Sdk
{
    [ScriptNamespace("SparkleXrm.Sdk")]
    public class Relationship
    {
        public Relationship(string schemaName)
        {
            this.SchemaName = schemaName;
        }

        public EntityRole PrimaryEntityRole;
        public string SchemaName;
    }
    [ScriptNamespace("SparkleXrm.Sdk")]
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
