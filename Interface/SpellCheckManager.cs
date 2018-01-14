using SimilarWordsFinder;
using SimilarWordsFinder.DictionaryClasses;
using SimilarWordsFinder.Serializers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface
{
    public class SpellCheckManager
    {
        #region Properties

        private Dictionary<string, List<string>> misspelledWords = new Dictionary<string, List<string>>();
        private List<string> correctWords = new List<string>();

        private int maxResults = 5;
        private int levDistance = 3;
        private int howManyChnages = 5;

        #endregion

        #region Getters and Setters

        public int MaxResults
        {
            get { return maxResults; }
            set { maxResults = value; UpdateMisspelledWords(); }
        }

        public int LevDistance
        {
            get { return levDistance; }
            set { levDistance = value; UpdateMisspelledWords(); }
        }

        public int HowManyChanges
        {
            get { return howManyChnages; }
            set { howManyChnages = value; UpdateMisspelledWords(); }
        }

        #endregion

        #region Public methods

        public SpellCheckManager()
        {
            GetDictionary(out Dictionary dictionary);
            DictionaryScanner.AddDictionary(dictionary);

        }

        /// <summary>
        /// Check spelling of word parameter 
        /// </summary>
        /// <param name="word"></param>
        /// <returns>true if word is correct and false if it's not</returns>
        public bool CheckWord(string word)
        {
            if (correctWords.Contains(word)) // if word was already checked and is correct
                return true;
            else if (misspelledWords.ContainsKey(word)) // if word was already checked and is misspelled 
                return false;
            else if (!DictionaryScanner.IsLowerWordInDictionary(ref word)) // if word was never checked and it's misspelled
            {
                AddWordToDictionary(word);
                return false;
            }
            correctWords.Add(word);
            return true;
        }

        /// <summary>
        /// Returns spelling propositions for misspelled word
        /// </summary>
        /// <param name="word">misspelled word</param>
        /// <returns>List of correctly spelled words propositions</returns>
        public List<string> SpellingPropositions(string word)
        {
            var result = new List<string>();
            var copy = word;
            word = word.ToLower();
            if (misspelledWords.ContainsKey(word))
                result = misspelledWords[word];

            if (!copy.Equals(word))
            {
                result = FirstCharToUpper(result);
            }

            return result;
        }

        public List<string> RawSpellingPropositions(string word)
        {
            return DictionaryScanner.FindSimilarWords(word, MaxResults, LevDistance, HowManyChanges);
        }

        #endregion

        #region Private methods

        // Add word to dictionary if it's not already in it
        private async void AddWordToDictionary(string word)
        {
            try
            {
                if (!misspelledWords.ContainsKey(word))
                    misspelledWords.Add(
                        word,
                        await Task.Run(() => DictionaryScanner.FindSimilarWords(word, MaxResults, LevDistance, HowManyChanges))
                        );
            }
            catch { }
        }

        private static void GetDictionary(out Dictionary dictionary)
        {
            ISerializer serializer = new ProtocolBuffersSerializer();
            var watch = new Stopwatch();

            if (!File.Exists(@"Dictionary\serialized"))
            {
                watch.Start();
                dictionary = new Dictionary(@"Dictionary\words.txt");
                watch.Stop();

                var data = serializer.Serialize<Dictionary>(dictionary);
                File.WriteAllBytes(@"Dictionary\serialized", data);
            }
            else
            {
                watch.Start();
                var data = File.ReadAllBytes(@"Dictionary\serialized");
                dictionary = serializer.Deserialize<Dictionary>(data);
                watch.Stop();
            }
            Console.WriteLine("Wczytano słownik - czas: {0} ms", 1000.0 * watch.ElapsedTicks / Stopwatch.Frequency);
        }

        private List<string> FirstCharToUpper(List<string> list)
        {
            var result = new List<string>();
            foreach (var item in list)
            {
                result.Add(item.First().ToString().ToUpper() + item.Substring(1));
            }

            return result;
        }

        private async void UpdateMisspelledWords()
        {
            var keys = misspelledWords.Keys.ToArray();

            foreach (var key in keys)
            {
                misspelledWords[key] = await Task.Run(() =>
                        DictionaryScanner.FindSimilarWords(key, MaxResults, LevDistance, HowManyChanges)
                    );
            }
        }

        #endregion
    }
}
