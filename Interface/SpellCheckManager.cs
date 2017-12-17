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
        private Dictionary<string, List<string>> misspelledWords = new Dictionary<string, List<string>>();

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
            if (!DictionaryScanner.IsLowerWordInDictionary(ref word))
            {
                AddWordToDictionary(word);
                return false;
            }
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

            if (misspelledWords.ContainsKey(word))
                result = misspelledWords[word];

            return result;
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
                        await Task.Run(() => DictionaryScanner.FindSimilarWords(word, 5))
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

        #endregion
    }
}
