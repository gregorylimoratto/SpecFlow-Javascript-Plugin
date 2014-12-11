using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecFlow.JavaScript.CodeDom
{
    internal static class CodeDomHelper
    {
        internal static string CleanString(string text)
        {
            return text.Replace("'", "\\'").Replace("\r\n", "\n").Replace("\n", "\\n'+" + Environment.NewLine + "'");
        }
    }
}
