using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow.BindingSkeletons;
using TechTalk.SpecFlow.Parser.Gherkin;

namespace SpecFlow.JavaScript.CodeDom
{
    public class JavascriptSpecWriter
    {
        private const string FeatureSpecTemplate = "(function(){{\n\tfeatureSteps('{0}: {1}'){2};\n}})();";

        private const string StepSpecTemplate = "\n\t.{0}('{1}', function({2}) {{\n\t\t/* Set test logic here */\n\t}})";

        private readonly string featureTitle;

        private readonly string featureDescription;

        private StringBuilder specText = new StringBuilder();

        public JavascriptSpecWriter(string featureTitle, string featureDescription)
        {
            this.featureDescription = featureDescription;
            this.featureTitle = featureTitle;
        }

        public string AppendStep(ScenarioBlock keyword, AnalyzedStepText analyzedStepText, bool hasTableArgs = false)
        {
            string keywordMethodName = keyword.ToString().ToLower();
            string methodText = GetMethodText(analyzedStepText);
            var parameters = analyzedStepText.Parameters.Select(x => x.Name + "/* " + x.Type + " */").ToList();
            if (hasTableArgs)
            {
                parameters.Add("table /* json */");
            }

            string methodParams = string.Join(",", parameters);

            specText.AppendFormat(StepSpecTemplate, keywordMethodName, methodText, methodParams);

            return methodText;
        }

        public string GetSpecText()
        {
            return string.Format(FeatureSpecTemplate, CodeDomHelper.CleanString(featureTitle), CodeDomHelper.CleanString(featureDescription), specText.ToString()); ;
        }

        private string GetMethodText(AnalyzedStepText analyzedStepText)
        {
            StringBuilder methodText = new StringBuilder();

            for (int i = 0; i < analyzedStepText.TextParts.Count; i++)
            {
                if (i != 0)
                {
                    methodText.Append("(" + analyzedStepText.Parameters[i - 1].RegexPattern + ")");
                }
                string clean = CodeDomHelper.CleanString(analyzedStepText.TextParts[i]);
                methodText.Append(clean);
            }

            return methodText.ToString();
        }

    }
}
