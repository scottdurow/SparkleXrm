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

        public string SchemaName;
    }
}
