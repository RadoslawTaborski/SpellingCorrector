using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using testConsoleApp.DictionaryClasses;
using testConsoleApp.Serializers;

namespace testConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            GetDictionary(out Dictionary dictionary);
            DictionaryScanner.AddDictionary(dictionary);

            Console.Write("\nPodaj słowa - jedno po drugim każde oddzielaj enterem:\n\n");

            do
            {
                string word = Console.ReadLine();
                StartAsync(word, 50, 3);
            } while (true);
        }

        #region Public
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

        public static void StartAsync(string word, int count, int distance)
        {
            Task.Run(() =>
            {
                Start(word, count, distance);
            });
        }
        #endregion

        #region Private
        private static void Start(string word, int count, int distance)
        {
            var result = new List<string>();
            Boolean isGood=false;
            string copy = word;

            if (!DictionaryScanner.IsLowerWordInDictionary(ref word) && word.Length > 1)
            {
                result = DictionaryScanner.FindSimilarWords(word, count);
            }
            else
            {
                if (copy != word)
                    result.Add(word);
                else
                    isGood = true;
            }

            if (isGood)
            {
                Console.WriteLine("- good");
            }
            else
            {
                Console.Write("-false\nPODPOWIEDZI:\n");
                foreach (var item in result)
                    Console.WriteLine(item);
                Console.Write("\n");
            }
        }
        #endregion
    }
}
