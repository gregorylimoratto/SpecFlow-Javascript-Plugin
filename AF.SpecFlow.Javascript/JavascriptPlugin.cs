﻿using System;
using System.IO;
using System.Text;
using BoDi;
using Javascript.Generator.SpecflowPlugin;
using SpecFlow.JavaScript.Generator;
using TechTalk.SpecFlow.Generator;
using TechTalk.SpecFlow.Generator.Configuration;
using TechTalk.SpecFlow.Generator.Interfaces;
using TechTalk.SpecFlow.Generator.Plugins;
using TechTalk.SpecFlow.Generator.UnitTestConverter;
using TechTalk.SpecFlow.Generator.UnitTestProvider;
using TechTalk.SpecFlow.Infrastructure;
using TechTalk.SpecFlow.Parser;
using TechTalk.SpecFlow.Parser.SyntaxElements;
using TechTalk.SpecFlow.Utils;

[assembly: GeneratorPlugin(typeof(JavascriptPlugin))]

namespace Javascript.Generator.SpecflowPlugin
{
    public class JavascriptPlugin : IGeneratorPlugin
    {
        public void RegisterDependencies(ObjectContainer container)
        {
        }

        public void RegisterCustomizations(ObjectContainer container, SpecFlowProjectConfiguration generatorConfiguration)
        {
            container.RegisterTypeAs<JavascriptTestGenerator, ITestGenerator>();
        }

        public void RegisterConfigurationDefaults(SpecFlowProjectConfiguration specFlowConfiguration)
        {
        }
    }

    [Serializable]
    public class JavascriptTestGenerator : ErrorHandlingTestGenerator, ITestGenerator
    {
        private ProjectSettings projectSettings = null;
        private GeneratorConfiguration generatorConfiguration = null;

        public JavascriptTestGenerator(GeneratorConfiguration generatorConfiguration, ProjectSettings projectSettings, ITestHeaderWriter testHeaderWriter, ITestUpToDateChecker testUpToDateChecker, IFeatureGeneratorRegistry featureGeneratorRegistry, CodeDomHelper codeDomHelper)
        {
            if (generatorConfiguration == null) throw new ArgumentNullException("generatorConfiguration");
            if (projectSettings == null) throw new ArgumentNullException("projectSettings");
            if (testHeaderWriter == null) throw new ArgumentNullException("testHeaderWriter");
            if (testUpToDateChecker == null) throw new ArgumentNullException("testUpToDateChecker");
            if (featureGeneratorRegistry == null) throw new ArgumentNullException("featureGeneratorRegistry");

            this.projectSettings = projectSettings;
            projectSettings.ProjectPlatformSettings.Language = "js";
            this.generatorConfiguration = generatorConfiguration;
        }

        public Version DetectGeneratedTestVersion(FeatureFileInput featureFileInput)
        {
            return new Version("8.8.8.8");
        }

        public TestGeneratorResult GenerateTestFile(FeatureFileInput featureFileInput, GenerationSettings settings)
        {
            Feature feature;
            SpecFlowLangParser parser = new SpecFlowLangParser(generatorConfiguration.FeatureLanguage);
            using (var contentReader = featureFileInput.GetFeatureFileContentReader(projectSettings))
            {
                feature = parser.Parse(contentReader, featureFileInput.GetFullPath(projectSettings));
            }

            JavascriptFeatureGenerator featureGenerator = new JavascriptFeatureGenerator();
            string generatedTestCode = featureGenerator.Generate(feature);
            var generatedFeatureFullPath = GetTestFullPath(featureFileInput) + ".js";

            File.WriteAllText(generatedFeatureFullPath, generatedTestCode, Encoding.UTF8);

            JavascriptSpecGenerator specGenerator = new JavascriptSpecGenerator();
            string generatedSpecCode = specGenerator.Generate(feature);
            var generatedTestFullPath = GetTestFullPath(featureFileInput) + ".generatedstep";

            File.WriteAllText(generatedTestFullPath, generatedSpecCode, Encoding.UTF8);

            return new TestGeneratorResult(null, false); // CodeDomProvider utilisé par SpecFlow pour l'ext du fichier généré.
            // donc on ne génére pas de fichier mais directement avec File.WriteAllText ci dessus.
        }

        public string GetTestFullPath(FeatureFileInput featureFileInput)
        {
            var path = featureFileInput.GetGeneratedTestFullPath(projectSettings);
            if (path != null)
                return path;

            return featureFileInput.GetFullPath(projectSettings);
        }

        public void Dispose()
        {
        }

        protected override Version DetectGeneratedTestVersionWithExceptions(FeatureFileInput featureFileInput)
        {
            throw new NotImplementedException();
        }

        protected override TestGeneratorResult GenerateTestFileWithExceptions(FeatureFileInput featureFileInput, GenerationSettings settings)
        {
            throw new NotImplementedException();
        }
    }
}