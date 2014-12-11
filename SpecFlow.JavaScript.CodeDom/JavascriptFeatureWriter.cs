using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SpecFlow.JavaScript.CodeDom
{
    public class JavascriptFeatureWriter
    {
        private const string FeatureTemplate = "(function(){{\n\tfeature('{0}: {1}'){2}\n}})();";

        private const string ScenarioTemplate = "\n\t\t.scenario('{0}')";

        private const string StepTemplate = "\n\t\t\t.{0}('{1}'{2})";

        private StringBuilder featureText = new StringBuilder();

        private string featureTitle = null;

        private string featureDescription = null;

        public JavascriptFeatureWriter(string featureTitle, string featureDescription)
        {
            this.featureTitle = featureTitle;
            this.featureDescription = featureDescription;
        }

        public void AddScenario(string text) {
            featureText.AppendFormat(ScenarioTemplate, CodeDomHelper.CleanString(text));
        }

        public void AddStep(string keyword, string text,  IList<IDictionary<string, string>> tableArgs)
        {
            if (tableArgs.Any())
            {
                string s = JsonConvert.SerializeObject(tableArgs);
                s = ", " + s;
                featureText.AppendFormat(StepTemplate, keyword.ToLower(), CodeDomHelper.CleanString(text), s);
            }
            else
            {
                featureText.AppendFormat(StepTemplate, keyword.ToLower(), CodeDomHelper.CleanString(text), string.Empty);
            }
        }

        public string GetFeatureText() {
            return string.Format(FeatureTemplate, CodeDomHelper.CleanString(featureTitle), CodeDomHelper.CleanString(featureDescription), featureText.ToString());
        }
    }
}
