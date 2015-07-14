// Entities.cs
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Xrm.Sdk;

namespace ClientUI.Model
{
    public class Connection : Entity
    {
        public static new string LogicalName = "connection";
        public Connection() : base("connection")
        {

        }
        [ScriptName("connectionid")]
        public Guid ConnectionID;

        [ScriptName("record1id")]
        public EntityReference Record1Id;

        [ScriptName("record2id")]
        public EntityReference Record2Id;

        [ScriptName("record1roleid")]
        public EntityReference Record1RoleId;

        [ScriptName("record2roleid")]
        public EntityReference Record2RoleId;

        [ScriptName("description")]
        public string Description;

        [ScriptName("effectivestart")]
        public DateTime EffectiveStart;

        [ScriptName("effectiveend")]
        public DateTime EffectiveEnd;

    }
}
