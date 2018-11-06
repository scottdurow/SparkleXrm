// PromiseTests.cs
//

using QUnitApi;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Xrm.Sdk;

namespace SparkleXrm.UnitTests
{
    public class PromiseTests
    {
        static PromiseTests()
        {
            QUnit.Module("PromiseTests",null);
            QUnit.Test("PromiseTests.Test_Create", Test_Create);

        }
        [PreserveCase]
        public static void Test_Create(Assert assert)
        {
            assert.Expect(1);
            Action done = assert.Async();
            Entity contact = new Entity("contact");
            contact.SetAttributeValue("lastname", "Test " + Date.Now.ToISOString());
            ;
            XrmService.Create(contact)
            .Then(delegate (Guid id)
            {
                contact.Id = id.Value;
                assert.Ok(true, contact.Id);
                done(); 

            })
            .Catch(delegate (Exception ex)
            {
                done();
            });

        }
    }
}
