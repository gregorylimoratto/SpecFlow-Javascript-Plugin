using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow.Generator;
using TechTalk.SpecFlow.Generator.Interfaces;

namespace Javascript.Generator.SpecflowPlugin.UnitTest
{
    [TestClass]
    public class GeneratorTester
    {
        // Permet de débugger le plugin specflow
        [TestMethod]
        public void TestGenerator()
        {
            VsConfigReader configurationReader = new VsConfigReader();

            var configurationHolder = configurationReader.ReadConfiguration();

            var projectSettings = new ProjectSettings
            {
                ProjectName = "Javascript.Generator.SpecflowPlugin.UnitTest",
                AssemblyName = "Javascript.Generator.SpecflowPlugin.UnitTest",
                ProjectFolder = "",
                DefaultNamespace = "Javascript.Generator.SpecflowPlugin.UnitTest",
                ProjectPlatformSettings = new ProjectPlatformSettings(),
                ConfigurationHolder = configurationHolder
            };
                var container = GeneratorContainerBuilder.CreateContainer(projectSettings.ConfigurationHolder, projectSettings);
                var test = container.Resolve<ITestGenerator>();

            var result = test.GenerateTestFile(new FeatureFileInput("SpecFlowFeature1.feature"), new GenerationSettings()
            {
                WriteResultToFile = false
            });

            // Résultat de l'exécution dans bin / debug / *.generatedspec
            Assert.IsTrue(result.Success);
        }
    }
}
