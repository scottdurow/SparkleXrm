// OrganizationSettings.cs
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Xrm.Sdk
{
    public class OrganizationSettings: Entity
    {
        public static string EntityLogicalName = "organization";

        public OrganizationSettings()
            : base(EntityLogicalName)
        {
        }

        [ScriptName("weekstartdaycode")]
        public OptionSetValue WeekStartDayCode;
    }
}
