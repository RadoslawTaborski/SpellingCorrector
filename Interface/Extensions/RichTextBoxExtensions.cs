using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace Interface.Extensions
{
    public static class RichTextBoxExtensions
    {

        public static int Bound { get; set; } = 14;
        private static char[] whiteChars = new char[] { ' ', ',', ':', '"', '-', '\n', '.', '\t','(', ')', '{', '}', '[', ']', ';', '\\', '/', '?'
        ,'<','>','=','+'};

        public static TextRange GetDocument(this RichTextBox rtb)
        {
            return new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);
        }

        // zwraca słowo na którym jest aktualnie kursor
        public static TextRange GetSelectedWord(this RichTextBox rtb)
        {
            int bound = 14;
            int x = rtb.Document.ContentStart.GetOffsetToPosition(rtb.CaretPosition);
            int toEnd = -rtb.Document.ContentEnd.GetOffsetToPosition(rtb.CaretPosition);

            int start, end;

            start = x < bound ? 0 : x - bound;
            end = toEnd > bound ? x + bound : x + toEnd;

            var point = rtb.Document.ContentStart;

            var s = new TextRange(point.GetPositionAtOffset(start), point.GetPositionAtOffset(end)).Text;

            var s1 = new TextRange(point.GetPositionAtOffset(start), point.GetPositionAtOffset(x)).Text;
            var s2 = new TextRange(point.GetPositionAtOffset(x), point.GetPositionAtOffset(end)).Text;


            // ******************************************************************************************

            int l = 0;
            for (int i = s1.Length - 1; i >= 0; --i)
                if (isWhiteChar(s1[i]))
                {
                    l = i;
                    break;
                }
            // if (l != -1)
            s1 = s1.Remove(0, l);

            var r = -1;
            for (int i = 0; i < s2.Length; ++i)
                if (isWhiteChar(s2[i]))
                {
                    r = i;
                    break;
                }
            if (r != -1)
                s2 = s2.Remove(r);

            var tr = new TextRange(point.GetPositionAtOffset(x - s1.Length), point.GetPositionAtOffset(x + s2.Length));

            return tr;
        }

        public static void ColorFont(this TextRange tr, SolidColorBrush color)
        {
            tr.ApplyPropertyValue(TextElement.ForegroundProperty, color);
            Run r = new Run("", tr.End);
            r.Foreground = Brushes.Black;
        }

        private static bool isWhiteChar(char c)
        {
            foreach (var character in whiteChars)
                if (c == character)
                    return true;

            return false;
        }

        public static List<TextRange> FindWord(this RichTextBox rtb, string word)
        {
            var position = rtb.Document.ContentStart;
            List<TextRange> words = new List<TextRange>();

            while (position != null)
            {
                if (position.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text)
                {
                    string textRun = position.GetTextInRun(LogicalDirection.Forward);

                    // Find the starting index of any substring that matches "word".
                    var indexInRun = textRun.AllIndexesOf(word);
                    if (indexInRun.Count != 0)
                    {
                        foreach (var index in indexInRun)
                        {
                            TextPointer start = position.GetPositionAtOffset(index);
                            TextPointer end = start.GetPositionAtOffset(word.Length);
                            words.Add(new TextRange(start, end));
                        }
                    }
                }

                position = position.GetNextContextPosition(LogicalDirection.Forward);
            }

            // position will be null if "word" is not found.
            return words;
        }

        public static List<string> GetWords(this RichTextBox rtb)
        {
            List<string> wordsList = new List<string>();

            var text = rtb.GetDocument().Text;
            var words = text.Split(whiteChars);

            foreach (var w in words)
            {
                var ww = w.Trim();
                if (ww.Length > 1 && !wordsList.Contains(ww))
                    wordsList.Add(ww);
            }

            return wordsList;
        }

    }
}
