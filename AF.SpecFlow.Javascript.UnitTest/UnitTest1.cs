using System;
using Microsoft.CSharp.RuntimeBinder;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow.Generator.Interfaces;
using TechTalk.SpecFlow.Parser;
using TechTalk.SpecFlow.Parser.Gherkin;
using TechTalk.SpecFlow.Parser.SyntaxElements;
using TechTalk.SpecFlow.Utils;
using TechTalk.SpecFlow.Tracing;
using System.Text.RegularExpressions;
using TechTalk.SpecFlow.BindingSkeletons;
using TechTalk.SpecFlow.Bindings;
using System.Dynamic;
using Newtonsoft.Json;

namespace AF.SpecFlow.Javascript.UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        [DeploymentItem("SpecFlowFeature1.feature")]
        public void TestMethod1()
        {
            string feature = Generate("SpecFlowFeature1.feature");
            string spec = GenerateTest("SpecFlowFeature1.feature");
            string s = "";
        }
        private string GenerateTest(string inputFilePath)
        {
            string txt = File.ReadAllText(inputFilePath);
            FeatureFileInput featureFileInput =
                new FeatureFileInput(inputFilePath)
                {
                    FeatureFileContent = txt
                };

            SpecFlowLangParser parser = new SpecFlowLangParser(new CultureInfo("en-US")); // TODO

            Feature feature = null;
            using (var contentReader = GetFeatureFileContentReader(featureFileInput))
            {
                feature = parser.Parse(contentReader, inputFilePath);
            }

            StringBuilder str = new StringBuilder();
            str.Append(GenerateFeatureStep(feature));

            return str.ToString();

        }

        public string GenerateFeatureStep(Feature feature)
        {
            string template = "(function(){{\n\tfeatureSteps('{0}'){1};\n}})();";
            return string.Format(template, feature.Title.Replace("\'", "\\'") + ": " + feature.Description.Replace("\'", "\\'"), GenerateScenariosSteps(feature));
        }

        private string GenerateScenariosSteps(Feature feature)
        {
            StringBuilder sb = new StringBuilder();

            List<string> stepsText = new List<string>();
            StringBuilder scenarioTemplate = new StringBuilder();

            if (feature.Background != null)
            {
                foreach (ScenarioStep step in feature.Background.Steps)
                {
                    var stepText = GenerateStepRegexp(step);
                    if (!stepsText.Contains(stepText))
                    {
                        stepsText.Add(stepText);
                        scenarioTemplate.Append(stepText);
                    }
                }
            }

            foreach (Scenario scenario in feature.Scenarios)
            {
                //scenarioTemplate.Append(string.Format(template, scenario.Title));
                // TODO : vérifier que le test n'a pas deja été écrit.

                foreach (ScenarioStep step in scenario.Steps)
                {
                    var stepText = GenerateStepRegexp(step);
                    if (!stepsText.Contains(stepText))
                    {
                        stepsText.Add(stepText);
                        scenarioTemplate.Append(stepText);
                    }
                }
            }
            return scenarioTemplate.ToString();
        }

        public string GenerateStepRegexp(ScenarioStep step)
        {

            var textAnalyzer = new StepTextAnalyzer();
            var analyzedString = textAnalyzer.Analyze(step.Text, CultureInfo.InvariantCulture);

            var method = GetMethodName(step.ScenarioBlock.ToString().ToLower(), analyzedString);
            return method;

        }

        private string GetMethodName(string keyword, AnalyzedStepText analyzedStepText)
        {
            return GetMatchingMethodName(keyword, analyzedStepText, CultureInfo.InvariantCulture, "_{0}_");
        }
        private string GetMatchingMethodName(string keyword, AnalyzedStepText analyzedStepText, CultureInfo language, string paramFormat)
        {
            StringBuilder result = new StringBuilder("\n\t." + keyword);
            result.Append("('");
            result.Append(analyzedStepText.TextParts[0].Replace("\'", "\\'"));
            for (int i = 1; i < analyzedStepText.TextParts.Count; i++)
            {
                result.Append("(" + analyzedStepText.Parameters[i - 1].RegexPattern + ")");

                //result.AppendFormat(paramFormat, analyzedStepText.Parameters[i - 1].Name.ToUpper(CultureInfo.InvariantCulture));
                result.Append(analyzedStepText.TextParts[i].Replace("\'", "\\'"));
            }

            //if (result.Length > 0 && result[result.Length - 1] == '_')
            //    result.Remove(result.Length - 1, 1);
            result.AppendFormat("', function ({0}) {{\n", string.Join(",", analyzedStepText.Parameters.Select(x => x.Name + "/* " + x.Type + "*/")));

            result.Append("\t\t/* Set test logic here */\n\t})");

            return result.ToString();
        }

        private void AppendWordsPascalCase(string text, CultureInfo language, StringBuilder result)
        {
            foreach (var word in GetWords(text))
            {
                result.Append(word.Substring(0, 1).ToUpper(language));
                result.Append(word.Substring(1));
            }
        }

        static private readonly Regex wordRe = new Regex(@"[\w]+");
        private IEnumerable<string> GetWords(string text)
        {
            return wordRe.Matches(text).Cast<Match>().Select(m => m.Value);
        }


        public string Generate(string inputFilePath)
        {
            string txt = File.ReadAllText(inputFilePath);
            FeatureFileInput featureFileInput =
                new FeatureFileInput(inputFilePath)
                {
                    FeatureFileContent = txt
                };

            SpecFlowLangParser parser = new SpecFlowLangParser(new CultureInfo("en-US")); // TODO

            Feature feature = null;
            using (var contentReader = GetFeatureFileContentReader(featureFileInput))
            {
                feature = parser.Parse(contentReader, inputFilePath);
            }

            StringBuilder str = new StringBuilder();

            str.Append(GenerateFeature(feature));

            return str.ToString();
        }


        private string GenerateFeature(Feature feature)
        {
            string template = "function()({{\n\tfeature('{0}'){1}\n}})();";
            return string.Format(template, feature.Title.Replace("\'", "\\'") + ": " + feature.Description.Replace("\'", "\\'"), GenerateScenarios(feature));
        }

        private string GenerateScenarios(Feature feature)
        {
            string template = "\n\t\t.scenario('{0}')";
            StringBuilder sb = new StringBuilder();


            foreach (Scenario scenario in feature.Scenarios)
            {
                Dictionary<string, string[]> parameters = null;
                if (scenario is ScenarioOutline)
                {
                    var scenarioOutline = (ScenarioOutline)scenario;

                    parameters = new Dictionary<string, string[]>();

                    for (int i = 0; i < scenarioOutline.Examples.ExampleSets.Length; i++)
                    {
                        var example = scenarioOutline.Examples.ExampleSets[i];
                        var header = example.Table.Header.Cells;
                        var content = example.Table.Body;

                        for (int j = 0; j < header.Length; j++)
                        {
                            List<string> values = new List<string>();
                            if (!parameters.ContainsKey(header[j].Value))
                            {
                                parameters.Add(header[j].Value, null);
                            }
                            else
                            {
                                values = parameters[header[j].Value].ToList();
                            }
                            values.AddRange(content.Select(x => x.Cells[j].Value).ToArray());
                            parameters[header[j].Value] = values.ToArray();
                        }
                    }
                }

                StringBuilder scenarioTemplate = new StringBuilder();
                scenarioTemplate.Append(template);


                if (feature.Background != null)
                {
                    foreach (ScenarioStep step in feature.Background.Steps)
                    {

                        var stepText = GenerateStep(step);
                        scenarioTemplate.Append(stepText);
                    }

                }

                //scenarioTemplate.Append(string.Format(template, scenario.Title));
                foreach (ScenarioStep step in scenario.Steps)
                {

                    var stepText = GenerateStep(step);
                    scenarioTemplate.Append(stepText);
                }


                if (parameters != null)
                {
                    for (int i = 0; i < parameters.First().Value.Length; i++)
                    {
                        var strScenarioTemplate = string.Format(scenarioTemplate.ToString(), scenario.Title + " (TestSet " + i + ")");

                        foreach (var kvp in parameters)
                        {
                            strScenarioTemplate = strScenarioTemplate.Replace("<" + kvp.Key + ">", kvp.Value[i]);
                        }
                        sb.Append(strScenarioTemplate);
                    }
                }
                else
                {
                    sb.Append(string.Format(scenarioTemplate.ToString(), scenario.Title));
                }

            }
            return sb.ToString();
        }

        private string GenerateStep(ScenarioStep scenarioStep)
        {
            string method = scenarioStep.Keyword.ToLower().Trim();
            string template = "\n\t\t\t.{0}('{1}')";
            // TODO ajouter les exemples

            if (scenarioStep.TableArg != null)
            {
                Dictionary<string, string[]> parameters = new Dictionary<string,string[]>();
                var header = scenarioStep.TableArg.Header.Cells;
                var content = scenarioStep.TableArg.Body;

                for (int j = 0; j < header.Length; j++)
                {
                    List<string> values = new List<string>();
                    if (!parameters.ContainsKey(header[j].Value))
                    {
                        parameters.Add(header[j].Value, null);
                    }
                    else
                    {
                        values = parameters[header[j].Value].ToList();
                    }
                    values.AddRange(content.Select(x => x.Cells[j].Value).ToArray());
                    parameters[header[j].Value] = values.ToArray();
                }

                List<Dictionary<string, string>> exp = new List<Dictionary<string, string>>();
                for (int i = 0; i < parameters[parameters.Keys.First()].Count(); i++)
                {
                    Dictionary<string, string> expando = new Dictionary<string, string>();
                    var dict = (Dictionary<string, string>)expando;
                    foreach (var kvp in parameters) {
                        dict[kvp.Key] = kvp.Value[i];
                    }
                    exp.Add(expando);
                }

                string s = JsonConvert.SerializeObject(exp);
                Console.WriteLine(s);
            }

            return string.Format(template, method, scenarioStep.Text.Replace("\'", "\\'"));
        }



        public static TextReader GetFeatureFileContentReader(FeatureFileInput featureFileInput)
        {
            if (featureFileInput == null) throw new ArgumentNullException("featureFileInput");

            if (featureFileInput.FeatureFileContent != null)
                return new StringReader(featureFileInput.FeatureFileContent);

            return new StreamReader(new FileInfo(featureFileInput.ProjectRelativePath).OpenRead());
        }
    }
}
