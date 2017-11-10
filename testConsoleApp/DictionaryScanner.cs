using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using testConsoleApp.DictionaryClasses;
using System.Text;
using System.Threading.Tasks;
using testConsoleApp.Tools;

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

        public static List<string> FindSimilarWords(string word, int maxNumberOfResults, int levensteinDistance=3, int howManyChanges=5)
        {
            word = word.ToLower();
            
            var watch2 = new Stopwatch();
            var watch3 = new Stopwatch();
            var result = new List<KeyValuePair<string, int>>();

            int increaseRangeLC = 1;
            //Console.WriteLine("Find Start\n");
            watch3.Start();
            var list3 = UseSpaceAdder(word, howManyChanges-1);
            watch3.Stop();

            watch2.Start();
            var list2 = UseLetterChanger(word, howManyChanges);
            watch2.Stop();
          
            var list1 = UseLevensteinAsync(word, levensteinDistance);           

            list2.ForEach(x => new KeyValuePair<string,int>(x.Key,x.Value-increaseRangeLC));

            result.AddRange(list3);
            result.AddRange(list2);
            result.AddRange(list1.Result);
            result = Levenshtein.SortList(result);
            result = result.GroupBy(x => x.Key).Select(g => g.First()).ToList();

            Console.WriteLine("Czas Levenstein:  ms | Czas LetterChanger: {0} ms | Czas SpaceAdder: {1} ms", 1000.0 * watch2.ElapsedTicks / Stopwatch.Frequency, 1000.0 * watch3.ElapsedTicks / Stopwatch.Frequency);
            //Console.WriteLine("\nFind End");
            
            return result.Select(item => item.Key).ToList().GetRange(0,maxNumberOfResults>result.Count?result.Count:maxNumberOfResults);
        }
        #endregion

        #region Private
        private static async Task<List<KeyValuePair<string, int>>> UseLevensteinAsync(string word, int levensteinDistance)
        {
            var watch = new Stopwatch();
            watch.Start();

            if (word.Length < 3)
            {
                levensteinDistance = 2;
            }else if (word.Length < 6)
            {
                levensteinDistance = 3;
            }        

            Task<List<KeyValuePair<string,int>>> t2 = Levenshtein.FindSimilarWordsAsync(word, _dictionary.GetWordsByLength(word.Length - 1), levensteinDistance);
            Task<List<KeyValuePair<string, int>>> t1 = Levenshtein.FindSimilarWordsAsync(word, _dictionary.GetWordsByLength(word.Length), levensteinDistance);
            Task<List<KeyValuePair<string, int>>> t3 = Levenshtein.FindSimilarWordsAsync(word, _dictionary.GetWordsByLength(word.Length + 1), levensteinDistance);

            await Task.WhenAll(t1, t2, t3);

            var list1 = new List<KeyValuePair<string, int>>();
            list1.AddRange(t1.Result);
            list1.AddRange(t2.Result);
            list1.AddRange(t3.Result);
            list1 = Levenshtein.SortList(list1);
            watch.Stop();
            Console.WriteLine("Czas Levenstein: {0} ms", 1000.0 * watch.ElapsedTicks / Stopwatch.Frequency);

            return list1;
        }

        private static List<KeyValuePair<string, int>> UseLetterChanger(string word, int howManyChanges)
        {
            var result = new List<KeyValuePair<string, int>>();

            var list2 = LetterChanger.Start(word, howManyChanges);
            foreach (var item in list2)
            {
                if (IsWordInDictionary(item.Key))
                {
                    result.Add(item);
                }
            }

            return result;
        }

        private static List<KeyValuePair<string, int>> UseSpaceAdder(string word, int howManyChanges)
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
                        var list = UseLetterChanger(w,howManyChanges);
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
