// FormContextTests.cs
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Xrm;
using Xrm.Sdk;

namespace SparkleXrm.UnitTests
{
    [IgnoreNamespace]
    public class FormContextTests
    {
        [ScriptName("contactFormOnLoad")]
        public static void ContactFormOnload()
        {
            Issue143_CreateFromDate();
        }

        public static void Issue143_CreateFromDate()
        {
            // Create a contact and assign it a date value
            // Run in context of contact form
            XrmEntity contact = ParentPage.Data.Entity;

            string attrName = "lastonholdtime";
            XrmAttribute xrmAttr = contact.Attributes.Get(attrName);
            DateTime dt = xrmAttr.GetValue<DateTime>();

            // Create anoter contact
            Entity contact1 = new Entity("contact");
            contact1.SetAttributeValue(attrName, new DateTime(dt.GetFullYear(),dt.GetMonth(),dt.GetDate(),dt.GetHours(),dt.GetMinutes(),dt.GetSeconds(),dt.GetMilliseconds()));
            contact1.SetAttributeValue("lastname", "TEST");

            OrganizationServiceProxy.BeginCreate(contact1, delegate (object state)
            {
                Guid contactId = OrganizationServiceProxy.EndCreate(state);
            });

        }
    }
}
