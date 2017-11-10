using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testConsoleApp
{
    public static class SpaceAdder
    {
        public static List<KeyValuePair<string,int>> Start(string word)
        {
            var result = new List<KeyValuePair<string, int>>();

            for(var i=1; i < word.Length; ++i)
            {
                result.Add(new KeyValuePair<string, int>(word.Insert(i, " "),0));
            }

            return result;
        }
    }
}
