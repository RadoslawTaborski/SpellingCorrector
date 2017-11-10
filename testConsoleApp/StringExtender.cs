using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SimilarWordsFinder
{
    public static class StringExtender
    {
        #region Public
        public static int NthIndexOf(this string target, string value, int n)
        {
            Match m = Regex.Match(target, "((" + Regex.Escape(value) + ").*?){" + n + "}");

            if (m.Success)
                return m.Groups[2].Captures[n - 1].Index;
            else
                return -1;
        }
        #endregion
    }
}
