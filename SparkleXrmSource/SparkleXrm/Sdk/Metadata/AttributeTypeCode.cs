// AttributeTypeCode.cs
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Xrm.Sdk.Metadata
{
    // Summary:
    //     Describes the type of an attribute.
    //[DataContract(Name = "AttributeTypeCode", Namespace = "http://schemas.microsoft.com/xrm/2011/Metadata")]
    [NamedValues]
    [ScriptNamespace("SparkleXrm.Sdk.Metadata")]
    public enum AttributeTypeCode
    {
        // Summary:
        //     A Boolean attribute. Value = 0.
         [ScriptName("Boolean")]
        Boolean_ = 0,
        //
        // Summary:
        //     An attribute that represents a customer. Value = 1.
        [ScriptName("Customer")]
        Customer = 1,
        //
        // Summary:
        //     A date/time attribute. Value = 2.
        [ScriptName("DateTime")]
        DateTime = 2,
        //
        // Summary:
        //     A decimal attribute. Value = 3.
       [ScriptName("Decimal")]
        Decimal = 3,
        //
        // Summary:
        //     A double attribute. Value = 4.
       [ScriptName("Double")]
        Double_ = 4,
        //
        // Summary:
        //     An integer attribute. Value = 5.
       [ScriptName("Integer")]
        Integer = 5,
        //
        // Summary:
        //     A lookup attribute. Value = 6.
      [ScriptName("Lookup")]
        Lookup = 6,
        //
        // Summary:
        //     A memo attribute. Value = 7.
       [ScriptName("Memo")]
        Memo = 7,
        //
        // Summary:
        //     A money attribute. Value = 8.
      [ScriptName("None")]
        Money = 8,
        //
        // Summary:
        //     An owner attribute. Value = 9.
        [ScriptName("Owner")]
        Owner = 9,
        //
        // Summary:
        //     A partylist attribute. Value = 10.
        [ScriptName("PartyList")]
        PartyList = 10,
        //
        // Summary:
        //     A picklist attribute. Value = 11.
       [ScriptName("Picklist")]
        Picklist = 11,
        //
        // Summary:
        //     A state attribute. Value = 12.
        [ScriptName("State")]
        State = 12,
        //
        // Summary:
        //     A status attribute. Value = 13.
        [ScriptName("Status")]
        Status = 13,
        //
        // Summary:
        //     A string attribute. Value = 14.
        [ScriptName("String")]
        String = 14,
        //
        // Summary:
        //     An attribute that is an ID. Value = 15.
       [ScriptName("Uniqueidentifier")]
        Uniqueidentifier = 15,
        //
        // Summary:
        //     An attribute that contains calendar rules. Value = 0x10.
        [ScriptName("CalendarRules")]
        CalendarRules = 16,
        //
        // Summary:
        //     An attribute that is created by the system at run time. Value = 0x11.
        [ScriptName("Virtual")]
        Virtual = 17,
        //
        // Summary:
        //     A big integer attribute. Value = 0x12.
       [ScriptName("BigInt")]
        BigInt = 18,
        //
        // Summary:
        //     A managed property attribute. Value = 0x13.
        [ScriptName("ManagedProperty")]
        ManagedProperty = 19,
        //
        // Summary:
        //     An entity name attribute. Value = 20.
         [ScriptName("EntityName")]
        EntityName = 20,

      
    }
}
