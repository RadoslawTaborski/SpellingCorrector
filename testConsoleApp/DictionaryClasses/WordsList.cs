using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimilarWordsFinder.DictionaryClasses
{
    [ProtoContract(SkipConstructor = false)]
    class WordsList
    {
        #region Properties
        [ProtoMember(1)]
        public List<string> List { get; private set; }
        #endregion

        #region Public
        public WordsList()
        {
            List = new List<string>();
        }

        public void Add(String str)
        {
            List.Add(str);
        }

        public void Sort()
        {
            List.Sort();
        }
        #endregion
    }
}
