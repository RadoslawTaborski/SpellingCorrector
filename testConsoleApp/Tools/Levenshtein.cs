﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace testConsoleApp.Tools
{
    public static class Levenshtein
    {
        #region Public
        public static List<KeyValuePair<string, int>> FindSimilarWords(String analyzedWord, List<string> dictionary,int levensteinDistance)
        {
            var findWords=CountForAll(analyzedWord, dictionary, levensteinDistance);
            //Console.WriteLine("\nFound Words: ");
            //DisplayWords(findWords.ToList());
            var sortedWords = SortList(findWords);
            //Console.WriteLine("\nSorted Found Words: ");
            //DisplayWords(sortedWords);

            return sortedWords;
        }

        public static Task<List<KeyValuePair<string, int>>> FindSimilarWordsAsync(String analyzedWord, List<string> dictionary, int levensteinDistance)
        {
            return Task.Run(() =>
            {
                return FindSimilarWords(analyzedWord,dictionary,levensteinDistance);
            });
        }


        public static List<KeyValuePair<string, int>> SortList(List<KeyValuePair<string, int>> list)
        {
            var myList = list.ToList();
            myList.Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value));

            return myList;
        }
        #endregion

        #region Private
        private static int Count(string s, string t)
        {
            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 1, m + 1];

            // Step 1
            if (n == 0)
            {
                return m;
            }

            if (m == 0)
            {
                return n;
            }

            // Step 2
            for (int i = 0; i <= n; d[i, 0] = i++)
            {
            }

            for (int j = 0; j <= m; d[0, j] = j++)
            {
            }

            // Step 3
            for (int i = 1; i <= n; i++)
            {
                //Step 4
                for (int j = 1; j <= m; j++)
                {
                    // Step 5
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;

                    // Step 6
                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }
            // Step 7
            return d[n, m];
        }

        private static List<KeyValuePair<string, int>> CountForAll(String analyzedWord, List<string> list, int levensteinDistance)
        {
            List<KeyValuePair<string, int>> foundWords = new List<KeyValuePair<string, int>>();
            int distance;

            foreach (var word in list)
            {
                distance = Count(analyzedWord, word);
                if (distance < levensteinDistance)
                {
                    foundWords.Add(new KeyValuePair<string,int>(word, distance));
                }
            }

            return foundWords;
        }

        private static void DisplayWords(List<KeyValuePair<string, int>> words)
        {
            foreach (var w in words)
                Console.WriteLine(w);
        }
        #endregion
    }
}