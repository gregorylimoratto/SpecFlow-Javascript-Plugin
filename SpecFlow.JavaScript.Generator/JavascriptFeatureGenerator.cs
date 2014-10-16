using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpecFlow.JavaScript.CodeDom;
using TechTalk.SpecFlow.Parser.SyntaxElements;
using Microsoft.CSharp.RuntimeBinder;
using System.Dynamic;

namespace SpecFlow.JavaScript.Generator
{
    public class JavascriptFeatureGenerator
    {
        private JavascriptFeatureWriter writer;

        public string Generate(Feature feature)
        {
            writer = new JavascriptFeatureWriter(feature.Title, feature.Description);

            GenerateScenario(feature);

            return writer.GetFeatureText();
        }

        private void GenerateScenario(Feature feature)
        {
            foreach (Scenario scenario in feature.Scenarios)
            {
                var parameters = ExtractScenarioOutlineParameters(scenario);

                if (parameters != null)
                {
                    for (int i = 0; i < parameters.First().Value.Length; i++)
                    {
                        var testSetParameters = parameters.Select(x => new KeyValuePair<string,string>(x.Key, x.Value[i])).ToArray();

                        WriteScenario(feature, scenario, " (TestSet " + (i + 1) + ")", testSetParameters);
                    }
                } else {
                    WriteScenario(feature, scenario, string.Empty, null);
                }
            }
        }

        private void WriteScenario(Feature feature, Scenario scenario, string complement, KeyValuePair<string, string>[] testSetParameters)
        {
            writer.AddScenario(scenario.Title + complement);
            if (feature.Background != null)
            {
                GenerateSteps(feature.Background.Steps, null);
            }
            GenerateSteps(scenario.Steps, testSetParameters);
        }

        private void GenerateSteps(ScenarioSteps scenarioSteps, KeyValuePair<string, string>[] parameters)
        {
            foreach (ScenarioStep step in scenarioSteps)
            {
                GenerateStep(step, parameters);
            }
        }

        private void GenerateStep(ScenarioStep step, KeyValuePair<string, string>[] parameters)
        {
            string text = step.Text;
            if (parameters != null) {
                foreach(var kvp in parameters) {
                    text = text.Replace("<" + kvp.Key + ">", kvp.Value);
                }
            }

            IList<IDictionary<string, string>> tableArgParams = new List<IDictionary<string, string>>();

            if (step.TableArg != null)
            {
                var tableArgs = ExtractParameters(step.TableArg);
                for (int i = 0; i < tableArgs[tableArgs.Keys.First()].Count(); i++)
                {
                    IDictionary<string, string> expando = new Dictionary<string, string>();
                    foreach (var kvp in tableArgs)
                    {
                        expando[kvp.Key] = kvp.Value[i];
                    }
                    tableArgParams.Add(expando);
                }
            }

            writer.AddStep(step.Keyword, text, tableArgParams);
        }


        private Dictionary<string, string[]> ExtractParameters(GherkinTable table)
        {
            Dictionary<string, string[]> parameters = new Dictionary<string, string[]>();
            var header = table.Header.Cells;
            var content = table.Body;

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
            return parameters;
        }

        private Dictionary<string, string[]> ExtractScenarioOutlineParameters(Scenario scenario)
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
            return parameters;
        }
    }
}
