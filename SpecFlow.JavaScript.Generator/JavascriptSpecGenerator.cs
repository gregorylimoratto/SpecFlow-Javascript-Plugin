using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpecFlow.JavaScript.CodeDom;
using TechTalk.SpecFlow.BindingSkeletons;
using TechTalk.SpecFlow.Parser.SyntaxElements;

namespace SpecFlow.JavaScript.Generator
{
    public class JavascriptSpecGenerator
    {
        private JavascriptSpecWriter writer;
        private List<string> knownSteps = new List<string>();

        public string Generate(Feature feature)
        {
            writer = new JavascriptSpecWriter(feature.Title, feature.Description);

            GenerateFeatureStep(feature);

            return writer.GetSpecText();
        }

        private void GenerateFeatureStep(Feature feature)
        {
            List<string> stepsText = new List<string>();
            var textAnalyzer = new StepTextAnalyzer();

            if (feature.Background != null)
            {
                foreach (ScenarioStep step in feature.Background.Steps)
                {
                    WriteStep(textAnalyzer, step);
                }
            }

            foreach (Scenario scenario in feature.Scenarios)
            {
                foreach (ScenarioStep step in scenario.Steps)
                {
                    WriteStep(textAnalyzer, step);
                }
            }
        }

        private void WriteStep(StepTextAnalyzer textAnalyzer, ScenarioStep step)
        {
            AnalyzedStepText analyzedStr;
            string stepText;
            GetStepText(textAnalyzer, step, out analyzedStr, out stepText);

            if (!knownSteps.Contains(stepText))
            {
                writer.AppendStep(step.ScenarioBlock, analyzedStr, step.TableArg != null);
            }
        }

        private static void GetStepText(StepTextAnalyzer textAnalyzer, ScenarioStep step, out AnalyzedStepText analyzedStr, out string stepText)
        {
            analyzedStr = textAnalyzer.Analyze(step.Text, CultureInfo.InvariantCulture);
            stepText = string.Join(string.Empty,analyzedStr.TextParts);
        }
    }
}
