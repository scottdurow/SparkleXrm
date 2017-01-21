// EntityReference.cs
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Xrm.Sdk
{
    [ScriptNamespace("SparkleXrm.Sdk")]
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

        /// <summary>
        /// Convert entity reference into a valid SOAP xml entity. 
        /// </summary>
        /// <param name="NameSpace"></param>
        /// <returns></returns>
        public string ToSoap(string NameSpace)
        {
            if (NameSpace == null || NameSpace == "")
                NameSpace = "a";

            return String.Format("<{0}:EntityReference><{0}:Id>{1}</{0}:Id><{0}:LogicalName>{2}</{0}:LogicalName><{0}:Name i:nil=\"true\" /></{0}:EntityReference>", NameSpace, this.Id.Value, this.LogicalName);
        }

        

    }
}
