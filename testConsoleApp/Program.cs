using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using testConsoleApp.DictionaryClasses;
using testConsoleApp.Interfaces;

namespace testConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            GetDictionary(out Dictionary dictionary);
            DictionaryScanner.AddDictionary(dictionary);

            do
            {
                Console.Write("\nPodaj słowo: ");
                string word = Console.ReadLine();
                var result = Start(word, 8, 3, out List<string> list);
                if (result)
                {
                    Console.WriteLine("- good");
                }
                else
                {
                    foreach (var item in list)
                        Console.WriteLine(item);
                    Console.Write("\n");
                }
            } while (true);
        }

        #region Methods
        public static void GetDictionary(out Dictionary dictionary)
        {
            ISerializer serializer = new ProtocolBuffersSerializer();
            var watch = new Stopwatch();

            if (!File.Exists(@"Dictionary\serialized"))
            {
                watch.Start();
                dictionary = new Dictionary();
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

        public static Boolean Start(string word, int count, int distance, out List<string> result)
        {
            result = new List<string>();
            string copy = word;

            if (!DictionaryScanner.IsLowerWordInDictionary(ref word) && word.Length > 1)
            {
                result = DictionaryScanner.FindSimilarWords(word, 8, 3);
            }
            else
            {
                if (copy != word)
                    result.Add(word);
                else
                    return true;
            }

            return false;
        }
        #endregion
    }
}
