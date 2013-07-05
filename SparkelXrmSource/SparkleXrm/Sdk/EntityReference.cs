// EntityReference.cs
//

using System;
using System.Collections.Generic;

namespace Xrm.Sdk
{
    public class EntityReference
    {
        public string Name;
        public Guid Id;
        public string LogicalName;
        public EntityReference(Guid Id, string LogicalName, string Name)
        {
            this.Id = Id;
            this.LogicalName = LogicalName;
            this.Name = Name;
        }
        public override string ToString()
        {

            return String.Format("[EntityReference: {0},{1},{2}]", this.Name, this.Id, this.LogicalName);

        }
    }
}
