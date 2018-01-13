using Interface.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

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
            maxNumberOfResults.Text = scManager.MaxResults.ToString();
            levenshteinDistance.Text = scManager.LevDistance.ToString();
            howManyChanges.Text = scManager.HowManyChanges.ToString();
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

        private void rtb_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            Task.Run(() => rtb.Dispatcher.Invoke(new UpdateInterface(checkSpeling)));
        }

        public delegate void UpdateInterface();

        private async void checkSpeling()
        {
            var word = rtb.GetSelectedWord();
            var results = new List<string>();

            if (word.GetPropertyValue(TextElement.ForegroundProperty) == Brushes.Red)
                results = await Task.Run(() => scManager.SpellingPropositions(word.Text));

            if (results.Count == 0)
                results = await Task.Run(() => scManager.RawSpellingPropositions(word.Text));

            rtb.SetContextMenu(results);
        }

        private void maxNumberOfResults_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                scManager.MaxResults = Int32.Parse(maxNumberOfResults.Text);
            }
            catch { }
        }

        private void levenshteinDoistance_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                scManager.LevDistance = Int32.Parse(levenshteinDistance.Text);
            }
            catch { }
        }

        private void howManyChanges_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                scManager.HowManyChanges = Int32.Parse(howManyChanges.Text);
            }
            catch { }
        }
    }
}
