using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Tooling.Connector;
using SparkleXrm.Tasks.Config;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text.RegularExpressions;

namespace SparkleXrm.Tasks.Tests
{
    [TestClass]
    public class EarlyBoundTypes
    {
        [TestMethod]
        [TestCategory("Integration Tests")]
        public void TestEbgGenerateGlobalOptionsets()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            var tempFolder = Path.Combine(Path.GetTempPath(), id.ToString());
            Directory.CreateDirectory(tempFolder);
            try
            {
                var config = new ConfigFile
                {
                    earlyboundtypes = new List<EarlyBoundTypeConfig>{
                                new EarlyBoundTypeConfig{
                                    useEarlyBoundGeneratorApi = true,
                                  generateOptionsetEnums = true,
                                  entities ="socialprofile,socialactivity"
                                    }
                                },
                    filePath = tempFolder

                };
                Generate(tempFolder, config);

                // Check that there was only a single instance of the global optionsset 'socialprofile_community'
                // public enum socialprofile_community

                var matches = CountMatches("public enum SocialProfile_Community", tempFolder);
                Assert.AreEqual(1, matches, "Global optionset created once only");
            }
            finally
            {
                Directory.Delete(tempFolder, true);
            }
        }

        [TestMethod]
        [TestCategory("Integration Tests")]
        public void TestEbgGenerateGlobalOptionsets_OneTypePerFile()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            var tempFolder = Path.Combine(Path.GetTempPath(), id.ToString());
            Directory.CreateDirectory(tempFolder);
            try
            {
                var config = new ConfigFile
                {
                    earlyboundtypes = new List<EarlyBoundTypeConfig>{
                        new EarlyBoundTypeConfig{
                            generateOptionsetEnums = true,
                            useEarlyBoundGeneratorApi = true,
                            entities ="socialprofile,socialactivity",
                            oneTypePerFile = true
                        }
                    },
                    filePath = tempFolder

                };
                Generate(tempFolder, config);


                Assert.IsFalse(File.Exists(Path.Combine(tempFolder, "entities.cs")));

                EnsureClassIsCreatedCorrectly(Path.Combine($"{tempFolder}\\Entities", "SocialProfile.cs"), "SocialProfile");
                EnsureClassIsCreatedCorrectly(Path.Combine($"{tempFolder}\\Entities", "SocialActivity.cs"), "SocialActivity");

                EnsureOptionSetsIsCreatedCorrectly(Path.Combine($"{tempFolder}\\OptionSets", "socialprofile_community.cs"), "SocialProfile_Community");
                EnsureOptionSetsIsCreatedCorrectly(Path.Combine($"{tempFolder}\\OptionSets", "socialactivity_prioritycode.cs"), "SocialActivity_PriorityCode");
            }
            finally
            {
                Directory.Delete(tempFolder, true);
            }
        }

        [TestMethod]
        [TestCategory("Integration Tests")]
        public void TestGenerateGlobalOptionsets()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            var tempFolder = Path.Combine(Path.GetTempPath(), id.ToString());
            Directory.CreateDirectory(tempFolder);
            try
            {
                var config = new ConfigFile
                {
                    earlyboundtypes = new List<EarlyBoundTypeConfig>{
                                new EarlyBoundTypeConfig{
                                  generateOptionsetEnums = true,
                                  generateGlobalOptionsets = true,
                                  entities ="socialprofile,socialactivity",
                                  filename="entities.cs"
                                    }
                                },
                    filePath = tempFolder

                };
                Generate(tempFolder, config);

                // Check that there was only a single instance of the global optionsset 'socialprofile_community'
                // public enum socialprofile_community

                string code = File.ReadAllText(Path.Combine(tempFolder, "entities.cs"));
                var matches = Regex.Matches(code, "public enum socialprofile_community");
                Assert.AreEqual(1, matches.Count, "Global optionset created once only");
            }
            finally
            {
                Directory.Delete(tempFolder, true);
            }
        }

        [TestMethod]
        [TestCategory("Integration Tests")]
        public void TestGenerateGlobalOptionsets_OneTypePerFile()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            var tempFolder = Path.Combine(Path.GetTempPath(), id.ToString());
            Directory.CreateDirectory(tempFolder);
            try
            {
                var config = new ConfigFile
                {
                    earlyboundtypes = new List<EarlyBoundTypeConfig>{
                        new EarlyBoundTypeConfig{
                            generateOptionsetEnums = true,
                            generateGlobalOptionsets = true,
                            entities ="socialprofile,socialactivity",
                            filename="entities.cs",
                            oneTypePerFile = true
                        }
                    },
                    filePath = tempFolder

                };
                Generate(tempFolder, config);


                Assert.IsFalse(File.Exists(Path.Combine(tempFolder, "entities.cs")));

                EnsureClassIsCreatedCorrectly(Path.Combine($"{tempFolder}\\Entities", "SocialProfile.cs"), "SocialProfile");
                EnsureClassIsCreatedCorrectly(Path.Combine($"{tempFolder}\\Entities", "SocialActivity.cs"), "SocialActivity");

                EnsureOptionSetsIsCreatedCorrectly(Path.Combine($"{tempFolder}\\OptionSets", "socialprofile_community.cs"), "socialprofile_community");
                EnsureOptionSetsIsCreatedCorrectly(Path.Combine($"{tempFolder}\\OptionSets", "socialactivity_prioritycode.cs"), "socialactivity_prioritycode");
            }
            finally
            {
                Directory.Delete(tempFolder, true);
            }
        }

        [TestMethod]
        [TestCategory("Integration Tests")]
        public void TestNotGeneratingGlobalOptionsets()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            var tempFolder = Path.Combine(Path.GetTempPath(), id.ToString());
            Directory.CreateDirectory(tempFolder);
            try
            {
                var config = new ConfigFile
                {
                    earlyboundtypes = new List<EarlyBoundTypeConfig>{
                                new EarlyBoundTypeConfig{
                                  generateOptionsetEnums = true,
                                  generateGlobalOptionsets = false,
                                  entities ="socialprofile,socialactivity",
                                  filename="entities.cs"
                                    }
                                },
                    filePath = tempFolder

                };
                Generate(tempFolder, config);

                // Check that there was only a single instance of the global optionsset 'socialprofile_community'
                // public enum socialprofile_community

                string code = File.ReadAllText(Path.Combine(tempFolder, "entities.cs"));
                var matches = Regex.Matches(code, "public enum socialprofile_community");
                Assert.AreEqual(0, matches.Count, "Global optionset created once only");

                // There should still be non-global optionsets generated
                // public enum socialactivity_prioritycode
                matches = Regex.Matches(code, "public enum socialactivity_prioritycode");
                Assert.AreEqual(1, matches.Count, "Non-Global optionset created");
            }
            finally
            {
                Directory.Delete(tempFolder, true);
            }
        }

        [TestMethod]
        [TestCategory("Integration Tests")]
        public void TestNotGeneratingGlobalOptionsets_OneTypePerFile()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            var tempFolder = Path.Combine(Path.GetTempPath(), id.ToString());
            Directory.CreateDirectory(tempFolder);
            try
            {
                var config = new ConfigFile
                {
                    earlyboundtypes = new List<EarlyBoundTypeConfig>{
                        new EarlyBoundTypeConfig{
                            generateOptionsetEnums = true,
                            generateGlobalOptionsets = false,
                            entities ="socialprofile,socialactivity",
                            filename="entities.cs",
                            oneTypePerFile = true
                        }
                    },
                    filePath = tempFolder

                };
                Generate(tempFolder, config);


                Assert.IsFalse(File.Exists(Path.Combine(tempFolder, "entities.cs")));

                EnsureClassIsCreatedCorrectly(Path.Combine($"{tempFolder}\\Entities", "SocialProfile.cs"), "SocialProfile");
                EnsureClassIsCreatedCorrectly(Path.Combine($"{tempFolder}\\Entities", "SocialActivity.cs"), "SocialActivity");

                Assert.IsFalse(File.Exists(Path.Combine($"{tempFolder}\\OptionSets", "socialprofile_community.cs")));
                EnsureOptionSetsIsCreatedCorrectly(Path.Combine($"{tempFolder}\\OptionSets", "socialactivity_prioritycode.cs"), "socialactivity_prioritycode");

            }
            finally
            {
                Directory.Delete(tempFolder, true);
            }
        }

        [TestMethod]
        [TestCategory("Integration Tests")]
        public void TestNotGeneratingAnyOptionsets()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            var tempFolder = Path.Combine(Path.GetTempPath(), id.ToString());
            Directory.CreateDirectory(tempFolder);
            try
            {
                var config = new ConfigFile
                {
                    earlyboundtypes = new List<EarlyBoundTypeConfig>{
                                new EarlyBoundTypeConfig{
                                  generateOptionsetEnums = false,
                                  generateGlobalOptionsets = false,
                                  entities ="socialprofile,socialactivity",
                                  filename="entities.cs"
                                    }
                                },
                    filePath = tempFolder

                };
                Generate(tempFolder, config);

                // Check that there are non instances of the global optionsset 'socialprofile_community'
                // public enum socialprofile_community

                string code = File.ReadAllText(Path.Combine(tempFolder, "entities.cs"));
                var matches = Regex.Matches(code, "public enum socialprofile_community");
                Assert.AreEqual(0, matches.Count, "Global optionset created once only");

                // There should be no non-global optionsets generated
                // public enum socialactivity_prioritycode
                matches = Regex.Matches(code, "public enum socialactivity_prioritycode");
                Assert.AreEqual(0, matches.Count, "Non-Global optionset created");
            }
            finally
            {
                Directory.Delete(tempFolder, true);
            }
        }

        [TestMethod]
        [TestCategory("Integration Tests")]
        public void TestNotGeneratingAnyOptionsets_OneTypePerFile()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            var tempFolder = Path.Combine(Path.GetTempPath(), id.ToString());
            Directory.CreateDirectory(tempFolder);
            try
            {
                var config = new ConfigFile
                {
                    earlyboundtypes = new List<EarlyBoundTypeConfig>{
                        new EarlyBoundTypeConfig{
                            generateOptionsetEnums = false,
                            generateGlobalOptionsets = false,
                            entities ="socialprofile,socialactivity",
                            filename="entities.cs",
                            oneTypePerFile = true
                        }
                    },
                    filePath = tempFolder

                };
                Generate(tempFolder, config);

                Assert.IsFalse(File.Exists(Path.Combine(tempFolder, "entities.cs")));

                EnsureClassIsCreatedCorrectly(Path.Combine($"{tempFolder}\\Entities", "SocialProfile.cs"), "SocialProfile");

                EnsureClassIsCreatedCorrectly(Path.Combine($"{tempFolder}\\Entities", "SocialActivity.cs"), "SocialActivity");

                Assert.IsFalse(File.Exists(Path.Combine($"{tempFolder}\\OptionSets", "socialprofile_community.cs")));
                Assert.IsFalse(File.Exists(Path.Combine($"{tempFolder}\\OptionSets", "socialactivity_prioritycode.cs")));
            }
            finally
            {
                Directory.Delete(tempFolder, true);
            }
        }

        private static void Generate(string tempFolder, ConfigFile config)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["integration_testing"].ConnectionString;
            CrmServiceClient crmSvc = new CrmServiceClient(connectionString);
            var userId = crmSvc.GetMyCrmUserId();
            var trace = new TraceLogger();


            using (var ctx = new OrganizationServiceContext(crmSvc))
            {
                ctx.MergeOption = MergeOption.NoTracking;

                // Act
                var task = new EarlyBoundClassGeneratorTask(ctx, trace);
                task.ConectionString = connectionString;

                task.CreateEarlyBoundTypes(ctx, config);
            }
        }

        private static void EnsureClassIsCreatedCorrectly(string classPath, string className)
        {
            var code = File.ReadAllText(classPath);
            var matches = Regex.Matches(code, $"public partial class {className}");
            Assert.AreEqual(1, matches.Count, $"Class {className} created once only");
        }

        private static void EnsureOptionSetsIsCreatedCorrectly(string optionSetsPath, string optionSetsName)
        {
            var code = File.ReadAllText(optionSetsPath);
            var matches = Regex.Matches(code, $"public enum {optionSetsName}");
            Assert.AreEqual(1, matches.Count, $"Optionset {optionSetsName} created once only");
        }

        private static int CountMatches(string matchString, string tempFolder)
        {
            var path = Path.Combine(tempFolder, "entities.cs");
            string code = File.ReadAllText(path);
            var matches = Regex.Matches(code, matchString);
            var count = matches.Count;
            path = Path.Combine(tempFolder, "optionsets.cs");
            if (File.Exists(path))
            {
                code = File.ReadAllText(path);
                matches = Regex.Matches(code, matchString);
                count += matches.Count;
            }

            path = Path.Combine(tempFolder, "actions.cs");
            if (File.Exists(path))
            {
                code = File.ReadAllText(path);
                matches = Regex.Matches(code, matchString);
                count += matches.Count;
            }
            return count;
        }
    }
}
