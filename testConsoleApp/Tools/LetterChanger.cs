using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SimilarWordsFinder.Tools
{
    public static class LetterChanger
    {
        #region Fields
        private readonly static List<KeyValuePair<string, string>> _letterPairs = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("rz", "ż"),
            new KeyValuePair<string, string>("ż", "rz"),

            new KeyValuePair<string, string>("u", "ó"),
            new KeyValuePair<string, string>("ó", "u"),

            new KeyValuePair<string, string>("ch", "h"),
            new KeyValuePair<string, string>("h", "ch"),

            new KeyValuePair<string, string>("i", "j"),
            new KeyValuePair<string, string>("j", "i"),

            new KeyValuePair<string, string>("ę", "em"),

            new KeyValuePair<string, string>("ji", "i"),
            new KeyValuePair<string, string>("ji", "ii"),

            new KeyValuePair<string, string>("rs", "rws"),

            new KeyValuePair<string, string>("ą", "on"),
            new KeyValuePair<string, string>("on", "ą"),
            new KeyValuePair<string, string>("ą", "ę"),
            new KeyValuePair<string, string>("ę", "ą"),
            new KeyValuePair<string, string>("ą", "om"),
            new KeyValuePair<string, string>("en", "ę"),
            new KeyValuePair<string, string>("ę", "en"),
            new KeyValuePair<string, string>("en", "ę"),
            new KeyValuePair<string, string>("en", "ę"),

            new KeyValuePair<string, string>("trz", "cz"),
            new KeyValuePair<string, string>("cz", "trz"),

            new KeyValuePair<string, string>("dź", "ć"),
            new KeyValuePair<string, string>("ć", "dź"),

            new KeyValuePair<string, string>("s", "ś"),
            new KeyValuePair<string, string>("ś", "s"),
            new KeyValuePair<string, string>("ł", "l"),
            new KeyValuePair<string, string>("l", "ł"),
            new KeyValuePair<string, string>("a", "ą"),
            new KeyValuePair<string, string>("ą", "a"),
            new KeyValuePair<string, string>("e", "ę"),
            new KeyValuePair<string, string>("ę", "e"),
            new KeyValuePair<string, string>("z", "ź"),
            new KeyValuePair<string, string>("ź", "z"),
            new KeyValuePair<string, string>("z", "ż"),
            new KeyValuePair<string, string>("ż", "z"),
            new KeyValuePair<string, string>("o", "ó"),
            new KeyValuePair<string, string>("ó", "o"),
            new KeyValuePair<string, string>("ć", "c"),
            new KeyValuePair<string, string>("c", "ć"),
            new KeyValuePair<string, string>("n", "ń"),
            new KeyValuePair<string, string>("ń", "n"),

            new KeyValuePair<string, string>("sz", "ż"),
            new KeyValuePair<string, string>("ż", "sz"),
        };
        #endregion

        #region Public
        public static string ChangeLetter(string word, string oldStr, string newStr, int count, int position)
        {
            var regex = new Regex(Regex.Escape(oldStr));
            var newText = regex.Replace(word, newStr, count, position);

            return newText;
        }

        public static List<KeyValuePair<string, int>> Start(string word, int howManyChanges)
        {
            Boolean[] bools;
            KeyValuePair<string, int> variant;
            var result = new List<KeyValuePair<string, int>>();
            List<KeyValuePair<string, int>> tmp = null;
            result.Add(new KeyValuePair<string, int>(word, 0));
            foreach (var item in _letterPairs)
            {
                tmp = new List<KeyValuePair<string, int>>();
                for (var z = 0; z < result.Count; ++z)
                {
                    var copy = result.ElementAt(z);
                    for (var j = 0; j < Math.Pow(Regex.Matches(copy.Key, item.Key).Count, 2); ++j)
                    {
                        variant = copy;
                        if (variant.Value > howManyChanges)
                            break;
                        var howMany = 0;
                        bools = ConvertByteToBoolArray((byte)(j + 1));
                        for (int i = 1; i <= Regex.Matches(copy.Key, item.Key).Count; ++i)
                        {
                            if (bools[i - 1] == true)
                            {
                                variant = new KeyValuePair<string, int>(ChangeLetter(variant.Key, item.Key, item.Value, 1, variant.Key.NthIndexOf(item.Key, i - howMany)), variant.Value + 1);
                                ++howMany;
                            }
                        }
                        tmp.Add(variant);
                    }
                }
                result.AddRange(tmp);
                result = result.GroupBy(x => x.Key).Select(g => g.First().Value > g.Last().Value ? g.Last() : g.First()).ToList();
            }

            return result;
        }
        #endregion

        #region Private
        private static bool[] ConvertByteToBoolArray(byte b)
        {
            bool[] result = new bool[8];
            for (int i = 0; i < 8; i++)
                result[i] = (b & (1 << i)) == 0 ? false : true;

            return result;
        }
        #endregion
    }
}
