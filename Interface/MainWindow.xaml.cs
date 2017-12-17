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
        SpellCheckManager scManager;

        public MainWindow()
        {
            InitializeComponent();
            scManager = new SpellCheckManager();
        }



        private void rtb_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                rtb.GetDocument().ColorFont(Brushes.Black);
                var words = rtb.GetWords();

                foreach (var word in words)
                {
                    var w = word;
                    if (!scManager.CheckWord(w))
                        foreach (var x in rtb.FindWord(w))
                            x.ColorFont(Brushes.Red);
                }
            }
        }

        private async void rtb_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            var word = rtb.GetSelectedWord();
            var results = new List<string>();

            if (word.GetPropertyValue(TextElement.ForegroundProperty) == Brushes.Red)
                results = await Task.Run(() => scManager.SpellingPropositions(word.Text));

            rtb.SetContextMenu(results);
        }
    }
}
