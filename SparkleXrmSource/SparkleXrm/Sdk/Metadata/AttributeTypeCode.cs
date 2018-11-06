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
        [PreserveCase]
        Customer = 1,
        //
        // Summary:
        //     A date/time attribute. Value = 2.
        [PreserveCase]
        DateTime = 2,
        //
        // Summary:
        //     A decimal attribute. Value = 3.
        [PreserveCase]
        Decimal = 3,
        //
        // Summary:
        //     A double attribute. Value = 4.
       [ScriptName("Double")]
        Double_ = 4,
        //
        // Summary:
        //     An integer attribute. Value = 5.
        [PreserveCase]
        Integer = 5,
        //
        // Summary:
        //     A lookup attribute. Value = 6.
        [PreserveCase]
        Lookup = 6,
        //
        // Summary:
        //     A memo attribute. Value = 7.
        [PreserveCase]
        Memo = 7,
        //
        // Summary:
        //     A money attribute. Value = 8.
        [PreserveCase]
        Money = 8,
        //
        // Summary:
        //     An owner attribute. Value = 9.
        [PreserveCase]
        Owner = 9,
        //
        // Summary:
        //     A partylist attribute. Value = 10.
        [PreserveCase]
        PartyList = 10,
        //
        // Summary:
        //     A picklist attribute. Value = 11.
        [PreserveCase]
        Picklist = 11,
        //
        // Summary:
        //     A state attribute. Value = 12.
        [PreserveCase]
        State = 12,
        //
        // Summary:
        //     A status attribute. Value = 13.
        [PreserveCase]
        Status = 13,
        //
        // Summary:
        //     A string attribute. Value = 14.
        [PreserveCase]
        String = 14,
        //
        // Summary:
        //     An attribute that is an ID. Value = 15.
        [PreserveCase]
        Uniqueidentifier = 15,
        //
        // Summary:
        //     An attribute that contains calendar rules. Value = 0x10.
        [PreserveCase]
        CalendarRules = 16,
        //
        // Summary:
        //     An attribute that is created by the system at run time. Value = 0x11.
        [PreserveCase]
        Virtual = 17,
        //
        // Summary:
        //     A big integer attribute. Value = 0x12.
        [PreserveCase]
        BigInt = 18,
        //
        // Summary:
        //     A managed property attribute. Value = 0x13.
        [PreserveCase]
        ManagedProperty = 19,
        //
        // Summary:
        //     An entity name attribute. Value = 20.
        [PreserveCase]
        EntityName = 20,

      
    }
}
