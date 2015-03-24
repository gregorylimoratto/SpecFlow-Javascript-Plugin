using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow.Parser.SyntaxElements;

namespace SpecFlow.JavaScript.Generator
{
    public static class GeneratorHelper
    {
        /// <summary>
        /// Ignore un scenario ou une feature si le tag ignore est positionné
        /// </summary>
        /// <param name="tags"></param>
        /// <returns></returns>
        public static bool ShouldIgnore(Tags tags)
        {
            return tags != null && tags.Any(x => "ignore".Equals(x.Name, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
