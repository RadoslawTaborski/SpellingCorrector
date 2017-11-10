using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using testConsoleApp.DictionaryClasses;
using System.Text;
using System.Threading.Tasks;

namespace testConsoleApp
{
    public static class DictionaryScanner
    {
        #region Fields
        private static Dictionary _dictionary;
        #endregion

        #region Public
        public static void AddDictionary(Dictionary dictionary)
        {
            _dictionary = dictionary;
        }

        public static Boolean IsWordInDictionary(string word)
        {
            var length = word.Length;

            if (_dictionary.GetWordsByLengthAndAlphabet(word).BinarySearch(word) > -1)
                return true;
            return false;
        }

        public static Boolean IsLowerWordInDictionary(ref string word)
        {
            word = word.ToLower();
            var length = word.Length;

            if (_dictionary.GetWordsByLengthAndAlphabet(word).BinarySearch(word)>-1)
                return true;
            return false;
        }

        public static List<string> FindSimilarWords(string word, int maxNumberOfResults, int levensteinDistance=3)
        {
            word = word.ToLower();
            var watch = new Stopwatch();
            var watch2 = new Stopwatch();
            var watch3 = new Stopwatch();
            var result = new List<KeyValuePair<string, int>>();

            int increaseRangeLC = 1;
            //Console.WriteLine("Find Start\n");
            watch3.Start();
            var list3 = UseSpaceAdder(word);
            watch3.Stop();

            watch2.Start();
            var list2 = UseLetterChanger(word);
            watch2.Stop();

            watch.Start();
            var list1 = UseLevenstein(word, levensteinDistance);
            watch.Stop();

            list1.ForEach(x => new KeyValuePair<string,int>(x.Key,x.Value-increaseRangeLC));

            result.AddRange(list3);
            result.AddRange(list2);
            result.AddRange(list1);
            result = Levenshtein.SortList(result);
            result = result.GroupBy(x => x.Key).Select(g => g.First()).ToList();

            Console.WriteLine("Czas Levenstein: {0} ms | Czas LetterChanger: {1} ms | Czas SpaceAdder: {2} ms", 1000.0 * watch.ElapsedTicks / Stopwatch.Frequency, 1000.0 * watch2.ElapsedTicks / Stopwatch.Frequency, 1000.0 * watch3.ElapsedTicks / Stopwatch.Frequency);
            //Console.WriteLine("\nFind End");
            
            return result.Select(item => item.Key).ToList().GetRange(0,maxNumberOfResults>result.Count?result.Count:maxNumberOfResults);
        }
        #endregion

        #region Private
        private static List<KeyValuePair<string, int>> UseLevenstein(string word, int levensteinDistance)
        {
            if (word.Length < 3)
            {
                levensteinDistance = 2;
            }else if (word.Length < 6)
            {
                levensteinDistance = 3;
            }

            var list2 = Levenshtein.FindSimilarWords(word, _dictionary.GetWordsByLength(word.Length - 1), levensteinDistance);
            var list1 = Levenshtein.FindSimilarWords(word, _dictionary.GetWordsByLength(word.Length), levensteinDistance);
            var list3 = Levenshtein.FindSimilarWords(word, _dictionary.GetWordsByLength(word.Length + 1), levensteinDistance);
            list1.AddRange(list2);
            list1.AddRange(list3);
            list1 = Levenshtein.SortList(list1);

            return list1;
        }

        private static List<KeyValuePair<string, int>> UseLetterChanger(string word)
        {
            var result = new List<KeyValuePair<string, int>>();

            var list2 = LetterChanger.Start(word);
            foreach (var item in list2)
            {
                if (IsWordInDictionary(item.Key))
                {
                    result.Add(item);
                }
            }

            return result;
        }

        private static List<KeyValuePair<string, int>> UseSpaceAdder(string word)
        {
            var result = new List<KeyValuePair<string, int>>();
            List<KeyValuePair<string, int>> result2;

            var list2 = SpaceAdder.Start(word);
            foreach (var item in list2)
            {
                result2 = new List<KeyValuePair<string, int>>();
                var tmp = item.Key.Split(' ');
                foreach (var w in tmp)
                {
                    if (!IsWordInDictionary(w))
                    {
                        var list = UseLetterChanger(w);
                        if(list.Count!=0)
                            result2.Add(new KeyValuePair<string, int>(list[0].Key,list[0].Value));
                    }
                    else
                    {
                        result2.Add(new KeyValuePair<string, int>(w, item.Value));
                    }
                }
                if(result2.Count>1)
                    result.Add(new KeyValuePair<string,int>(result2[0].Key+" "+result2[1].Key, result2[0].Value + result2[1].Value));
            }
            
            return result;
        }

        #endregion
    }
}
