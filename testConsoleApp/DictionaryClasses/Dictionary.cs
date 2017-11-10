using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ProtoBuf;


namespace testConsoleApp.DictionaryClasses
{
    [ProtoContract(SkipConstructor = true)]
    public class Dictionary
    {
        #region Fields
        [ProtoMember(1)]
        private SortedWords[] sortedWords;
        #endregion

        #region Properties
        [ProtoMember(2)]
        public int MaxLength { get; private set; }
        #endregion

        #region Public
        public Dictionary()
        {
            var words = File.ReadAllLines(@"Dictionary\words.txt");
            SetOrder(words);
        }

        public List<string> GetWordsByLength(int length)
        {
            if (length <= MaxLength && length > 0 && sortedWords[length - 1] != null)
                return sortedWords[length - 1].LengthSorted ?? new List<string>();
            return new List<string>();
        }

        public List<string> GetWordsByLengthAndAlphabet(string word)
        {
            if (word.Length <= MaxLength && word.Length > 0 && sortedWords[word.Length - 1] != null)
                return sortedWords[word.Length - 1].GetConcreteAlphabetList(word) ?? new List<string>();
            return new List<string>();
        }
        #endregion

        #region Private
        private void SetOrder(string[] words)
        {
            MaxLength = words.Max(x=>x.Length);
            sortedWords = new SortedWords[MaxLength];
            for (int i = 0; i < MaxLength; ++i)
            {
                sortedWords[i] = new SortedWords();
            }

            foreach (var word in words)
            {
                sortedWords[word.Length - 1].Add(word);
            }
        }
        private void DisplayList(List<string> list)
        {
            foreach (var word in list)
                Console.WriteLine(word);
        }
        #endregion
    }
}
