using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow.Generator.Interfaces;
using Javascript.Generator.SpecflowPlugin;
using TechTalk.SpecFlow.Generator;
using TechTalk.SpecFlow.Generator.Configuration;
using System.Xml;

namespace UnitTestProject1
{
    /// <summary>
    /// Description résumée pour UnitTest1
    /// </summary>
    [TestClass]
    public class UnitTest1
    {
        public UnitTest1()
        {
        }


        [TestMethod]
        public void TestMethod1()
        {
            VsConfigReader configurationReader = new VsConfigReader();

            var configurationHolder = configurationReader.ReadConfiguration();

            var projectSettings =  new ProjectSettings
            {
                ProjectName = "UnitTestProject1",
                AssemblyName = "UnitTestProject1",
                ProjectFolder = "",
                DefaultNamespace = "UnitTestProject1",
                ProjectPlatformSettings = new ProjectPlatformSettings(),
                ConfigurationHolder = configurationHolder
            };
            try
            {
                var container = GeneratorContainerBuilder.CreateContainer(projectSettings.ConfigurationHolder, projectSettings);
                var test = container.Resolve<ITestGenerator>();

                test.GenerateTestFile(new FeatureFileInput("test.feature"), new GenerationSettings()
                {
                    WriteResultToFile = false
                });
                string s = "";
            }
            catch (Exception e)
            {
                string ex = "";
            }

        }
    }
}
