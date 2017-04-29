// Class1.cs
//

using ClientUI.ViewModel;
using QUnitApi;
using Slick;
using SparkleXrm;
using System;
using System.Collections.Generic;
using System.Html;
using Xrm.Sdk;

namespace ClientUI.UnitTests
{
  
    public class UnitTest2
    {
      
        public static void Run()
        {
            ModuleInfo module = new ModuleInfo();
            module.BeforeEach = SetUp;
            module.AfterEach = Teardown;
            QUnit.Module("Unit Tests", module);
            QUnit.Test("Test1", Test1);            
        }

        public static void SetUp()
        {
           // Set up
        }

        public static void Teardown()
        {
            // Teardown
        }
        public static void Test1(Assert assert)
        {
            assert.Expect(1);
            assert.Equal(true, false, "Message");
        }    
    }
}
