using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace Microsoft.Crm.Sdk.Fakes.Tests
{
    [TestClass]
    public class FakePipelineTest
    {
        [TestMethod]
        public void CreatePipeline()
        {
            var target = new Xrm.Sdk.Entity("account");
            target.Id = Guid.NewGuid();

            // Arrange
            using (var pipeline = new PluginPipeline(FakeMessageNames.Create, FakeStages.PreOperation, target))
            {
                pipeline.Depth = 10;

                pipeline.FakeService.ExpectRetrieve( (entityName, id, columnSet) => {

                    var returned = new Entity("account");
                    returned["name"] = "123";
                    return returned;
                  
                });

                var plugin = new AccountPlugin();
                // Act & Assert
                pipeline.Execute(plugin);
                // Assert
                Assert.AreEqual("123", target.GetAttributeValue<string>("name"));
                Assert.AreEqual(10, pipeline.PluginExecutionContext.Depth);
                pipeline.FakeService.AssertExpectedCalls();
            }
        }
    }

    public class AccountPlugin : Plugin
    {
        public AccountPlugin()
            : base(typeof(AccountPlugin))
        {
            base.RegisteredEvents.Add(new Tuple<int, string, string, Action<LocalPluginContext>>(20, "Create", "account", new Action<LocalPluginContext>(Create)));
         
        }
        protected void Create(LocalPluginContext localContext)
        {
            if (localContext == null)
            {
                throw new ArgumentNullException("localContext");
            }

            Assert.AreEqual("Create", localContext.PluginExecutionContext.MessageName);

            var target = localContext.Target;

            Assert.IsNotNull(target);
            Assert.AreEqual("account", target.LogicalName);

            var result = localContext.OrganizationService.Retrieve("account", localContext.PluginExecutionContext.PrimaryEntityId, new ColumnSet("name"));
            target["name"] = result["name"];
        }
      
    }
}
