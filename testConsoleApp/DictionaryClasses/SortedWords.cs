using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testConsoleApp.DictionaryClasses
{
    [ProtoContract(SkipConstructor = true)]
    public class SortedWords
    {
        #region Properties
        [ProtoMember(1)]
        public WordsList[] LengthAndAlphabetSorted { get; private set; }
        [ProtoMember(2)]
        public List<string> LengthSorted { get; private set; }
        #endregion

        #region Public
        public SortedWords()
        {
            LengthSorted = new List<string>();
            LengthAndAlphabetSorted = new WordsList[35];
            for (var i = 0; i < 35; ++i)
            {
                LengthAndAlphabetSorted[i] = new WordsList();
            }
        }

        public void Add(string str)
        {
            LengthSorted.Add(str);
            LengthAndAlphabetSorted[FirstLetterIndex(str)].Add(str);
        }

        public List<string> GetConcreteAlphabetList(string str)
        {
            var index = FirstLetterIndex(str);
            if (index != -1)
            {
                return LengthAndAlphabetSorted[index].List;
            }
            return new List<string>();
        }

        public void Sort()
        {
            LengthSorted.Sort();
            for (var i = 0; i < 35; ++i)
            {
                LengthAndAlphabetSorted[i].Sort();
            }
        }
        #endregion

        #region Private
        private int FirstLetterIndex(string str)
        {
            string[] alphabetPL = { "a", "ą", "b", "c", "ć", "d", "e", "ę", "f", "g", "h", "i", "j", "k", "l", "ł", "m", "n", "ń", "o", "ó", "p", "q", "r", "s", "ś", "t", "u", "v", "w", "x", "y", "z", "ź", "ż" };
            return Array.IndexOf(alphabetPL, str.ElementAt(0).ToString());
        }
        #endregion
    }
}
