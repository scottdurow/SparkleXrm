// LocalisationTests.cs
//


using QUnitApi;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Xrm;
using Xrm.Sdk;

namespace SparkleXrm.UnitTests
{
    public class LocalisationTests
    {
        static LocalisationTests()
        {
            QUnit.Module("LocalisationTests", null);
            QUnit.Test("NumberParse", LocalisationTests.NumberParse);
            QUnit.Test("LocalTimeZoneTests", LocalisationTests.LocalTimeZoneTests);
            QUnit.Test("UTCTimeZoneTests", LocalisationTests.UTCTimeZoneTests);
        }

        [PreserveCase]
        public static void NumberParse(Assert assert)
        {
            assert.Expect(2);
            // Use the Contact which 
            NumberFormatInfo format = new NumberFormatInfo();
            format.DecimalSymbol = ",";
            format.NumberSepartor = ".";
        
            Number value1 = NumberEx.Parse("22,10", format);
            assert.Equal(value1, 22.10,"numbers equal after format");

            Number value2 = NumberEx.Parse("1.022,10", format);
            assert.Equal(value2, 1022.10, "numbers equal after format");
        }

        [PreserveCase]
        public static void LocalTimeZoneTests(Assert assert)
        {
            assert.Expect(1);
            // Use the Contact which has a timezone aware date
           
            string dateAttribute = "lastonholdtime";

            DateTime localTime = new DateTime();

            // Create a contact with a local date time

            Entity contact = new Entity("contact");
            contact.SetAttributeValue(dateAttribute, localTime);
            contact.SetAttributeValue("lastname", "TEST");
            contact.Id = OrganizationServiceProxy.Create(contact).ToString();

            // Get the contact
            Entity contact2 = OrganizationServiceProxy.Retrieve("contact", contact.Id, new string[] { dateAttribute });

            DateTime serverTime = (DateTime)contact2.GetAttributeValue(dateAttribute);
            // Check they are the same in UTC
            assert.Equal(serverTime.ToUTCString(), localTime.ToUTCString(), String.Format("dates equal {0} {1}",serverTime.ToString(),localTime.ToString()) );

            OrganizationServiceProxy.Delete_("contact", new Guid(contact2.Id));

        
        }

        [PreserveCase]
        public static void UTCTimeZoneTests(Assert assert)
        {
            assert.Expect(1);
            // Use the Contact which has a timezone aware date

            string dateAttribute = "lastonholdtime";

            DateTime localTime = new DateTime();

            DateTime utcTime = new DateTime();
            utcTime.SetUTCFullYear(localTime.GetUTCFullYear());
            utcTime.SetUTCMonth(localTime.GetUTCMonth());
            utcTime.SetUTCDate(localTime.GetUTCDate());
            utcTime.SetUTCHours(localTime.GetUTCHours());
            utcTime.SetUTCMinutes(localTime.GetUTCMinutes());
            utcTime.SetUTCSeconds(localTime.GetUTCSeconds());
            utcTime.SetUTCMilliseconds(localTime.GetUTCMilliseconds());
            // Create a contact with a local date time

            Entity contact = new Entity("contact");
            contact.SetAttributeValue(dateAttribute, utcTime);
            contact.SetAttributeValue("lastname", "TEST");
            contact.Id = OrganizationServiceProxy.Create(contact).ToString();

            // Get the contact
            Entity contact2 = OrganizationServiceProxy.Retrieve("contact", contact.Id, new string[] { dateAttribute });

            DateTime serverTime = (DateTime)contact2.GetAttributeValue(dateAttribute);
            // Check they are the same in UTC
            assert.Equal(serverTime.ToUTCString(), utcTime.ToUTCString(), String.Format("dates equal {0} {1}", serverTime.ToString(), utcTime.ToString()));

            OrganizationServiceProxy.Delete_("contact", new Guid(contact2.Id));
            
        }
    }
}
