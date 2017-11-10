using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SimilarWordsFinder;
using SimilarWordsFinder.DictionaryClasses;
using SimilarWordsFinder.Serializers;
using System.Diagnostics;
using System.IO;

namespace WindowApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            GetDictionary(out Dictionary dictionary);
            DictionaryScanner.AddDictionary(dictionary);
        }

        private void Check_Click(object sender, EventArgs e)
        {
            tbList.Text = "";
            var word = tbText.Text;
            StartAsync(word, 8, 3, 5);
        }

        public void StartAsync(string word, int count, int distance, int howManyChanges)
        {
            Task.Run(() =>
            {
                Start(word, count, distance, howManyChanges);
            });
        }

        private void Start(string word, int count, int distance, int howManyChanges)
        {
            var result = new List<string>();
            Boolean isGood = false;
            string copy = word;

            if (!DictionaryScanner.IsLowerWordInDictionary(ref word) && word.Length > 1)
            {
                result = DictionaryScanner.FindSimilarWords(word, count, distance, howManyChanges);
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
                tbText.BackColor = Color.Green;
            }
            else
            {
                var list = "";
                tbText.BackColor = Color.Red;
                foreach (var item in result)
                    list+=(item+"\n");
                AppendTextBox(list);
            }
        }

        public static void GetDictionary(out Dictionary dictionary)
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

        public void AppendTextBox(string value)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string>(AppendTextBox), new object[] { value });
                return;
            }
            tbList.Text = value;
        }
    }
}
