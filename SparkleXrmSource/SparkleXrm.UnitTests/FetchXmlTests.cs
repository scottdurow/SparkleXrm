// OrganizationServiceProxyTests.cs
//

using QUnitApi;
using SparkleXrm.UnitTests;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Xrm.Sdk;


namespace SparkleXrm.UnitTests
{
  
    public class FetchXmlTests
    {
        static FetchXmlTests()
        {
            QUnit.Module("FetchXmlTests", null);
            QUnit.Test("FetchXmlTests.RetreiveMultiple_01_Simple", FetchXmlTests.RetreiveMultiple_01_Simple);
            QUnit.Test("FetchXmlTests.RetreiveMultiple_02_InvalidXml", FetchXmlTests.RetreiveMultiple_02_InvalidXml);
            QUnit.Test("FetchXmlTests.RetreiveMultiple_03_UnkownLogicalName",FetchXmlTests.RetreiveMultiple_03_UnkownLogicalName);
            QUnit.Test("FetchXmlTests.RetreiveMultiple_04_VeryLongFetch", FetchXmlTests.RetreiveMultiple_04_VeryLongFetch);
            QUnit.Test("FetchXmlTests.RetreiveMultiple_05_TotalRecordCount", FetchXmlTests.RetreiveMultiple_05_TotalRecordCount);
        }
        [PreserveCase]
        public static void RetreiveMultiple_01_Simple(Assert assert)
        {
            assert.Expect(1);
            Action done = assert.Async();
            OrganizationServiceProxy.BeginRetrieveMultiple(@"<fetch version='1.0' output-format='xml-platform' mapping='logical' count='2' distinct='false' returntotalrecordcount='true'>
                          <entity name='account'>
                            <attribute name='name' />
                            <attribute name='primarycontactid' />
                            <attribute name='telephone1' />
                            <attribute name='accountid' />
                            <order attribute='name' descending='false' />
                          </entity>
                        </fetch>", delegate (object state)
            {
                
                EntityCollection items = OrganizationServiceProxy.EndRetrieveMultiple(state, typeof(Entity));
                assert.Ok(items.Entities.Count > 0, "Non zero return count");
                done();
            });
        }



        [PreserveCase]
        public static void RetreiveMultiple_02_InvalidXml(Assert assert)
        {
            assert.Expect(1);
            Action done = assert.Async();
            try
            {
                OrganizationServiceProxy.BeginRetrieveMultiple(@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <", delegate (object state)
                {


                    try
                    {

                        EntityCollection items = OrganizationServiceProxy.EndRetrieveMultiple(state, typeof(Entity));
                    }
                    catch (Exception ex)
                    {
                        assert.Ok(ex.Message.IndexOf("Invalid XML") > -1, ex.Message);
                        done();
                    }
                

                });
            }
            catch (Exception ex)
            {
         
                assert.Ok(ex.Message.IndexOf("Invalid FetchXml") > -1, ex.Message);
                done();
            }
        }


        [PreserveCase]
        public static void RetreiveMultiple_03_UnkownLogicalName(Assert assert)
        {
            Action done = assert.Async();
            assert.Expect(1);

            OrganizationServiceProxy.BeginRetrieveMultiple(@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                          <entity name='unknown_entity'>
                            <attribute name='name' />
                            <attribute name='primarycontactid' />
                            <attribute name='telephone1' />
                            <attribute name='accountid' />
                            <order attribute='name' descending='false' />
                          </entity>
                        </fetch>", delegate (object state)
            {
                try
                {
                    EntityCollection items = OrganizationServiceProxy.EndRetrieveMultiple(state, typeof(Entity));
                }
                catch (Exception ex)
                {
                    assert.Ok(ex.Message.IndexOf("unknown_entity") >-1, ex.Message);
                }

                done();
            });
        }

        [PreserveCase]
        public static void RetreiveMultiple_04_VeryLongFetch(Assert assert)
        {
            assert.Expect(1);

            string query = "<value>{00000000-0000-0000-0000-000000000000}</value>";

            string fetchxml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='contact'>
                                <attribute name='fullname' />
                                <attribute name='telephone1' />
                                <attribute name='contactid' />
                                <order attribute='fullname' descending='false' />
                                <filter type='and'>
                                  <condition attribute='contactid' operator='in'>
                                   {0}
                                  </condition>
                                </filter>
                              </entity>
                            </fetch>";

            string longCondition = "";
            for (int j = 0; j < 400; j++)
            {
                longCondition += query;
            }
            string fetchQuery = String.Format(fetchxml, longCondition);

            EntityCollection results = OrganizationServiceProxy.RetrieveMultiple(fetchQuery);


            assert.Ok(results.Entities.Count == 0, "No results - but not an error due to the larget fetch size!");
        }

        [PreserveCase]
        public static void RetreiveMultiple_05_TotalRecordCount(Assert assert)
        {
            assert.Expect(2);
            Action done = assert.Async();
            OrganizationServiceProxy.BeginRetrieveMultiple(@"<fetch version='1.0' output-format='xml-platform' mapping='logical' count='1' distinct='false' returntotalrecordcount='true'>
                          <entity name='account'>
                            <attribute name='name' />
                            <order attribute='name' descending='false' />
                          </entity>
                        </fetch>", delegate (object state)
            {

                EntityCollection items = OrganizationServiceProxy.EndRetrieveMultiple(state, typeof(Entity));
                assert.Ok(items.Entities.Count > 0, "Non zero return count");
                assert.Ok(items.TotalRecordCount > 0, "Total Record count returned");
                done();
            });
        }
    }
}
