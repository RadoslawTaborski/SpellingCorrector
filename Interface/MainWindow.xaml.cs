using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Interface.Extensions;
using SimilarWordsFinder;
using SimilarWordsFinder.DictionaryClasses;
using SimilarWordsFinder.Serializers;
using System.IO;
using System.Diagnostics;

namespace Interface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            GetDictionary(out Dictionary dictionary);
            DictionaryScanner.AddDictionary(dictionary);
        }

        private void rtb_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Space)
            {
                rtb.GetDocument().ColorFont(Brushes.Black);
                var words = rtb.GetWords();

                foreach(var word in words)
                {
                    var w = word;
                    if (!DictionaryScanner.IsLowerWordInDictionary(ref w))
                        foreach (var x in rtb.FindWord(w))
                            x.ColorFont(Brushes.Red);
                }
            }
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
    }
}
