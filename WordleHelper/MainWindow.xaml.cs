using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace WordleHelper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public ObservableCollection<string> Words { get; set; }
        private WordStore store = new WordStore();
        private int[] buttonState = { 0, 0, 0, 0, 0 };
        private TextBox[] letters;

        public MainWindow()
        {
            InitializeComponent();
            Words = new ObservableCollection<string>(store.FreshListAlpha);
            PossibleWords.DataContext = Words;
            letters = new TextBox[] { Letter0, Letter1, Letter2, Letter3, Letter4 };
        }

        private static int UpdateButton(Button button)
        {
            if (((SolidColorBrush)button.Background).Color == Colors.White)
            {
                button.Background = Brushes.Yellow;
                return 1;
            }
            else if (((SolidColorBrush)button.Background).Color == Colors.Yellow)
            {
                button.Background = Brushes.Green;
                return 2;
            }
            else
            {
                button.Background = Brushes.White;
                return 0;
            }
        }

        private void UpdateWords()
        {
            for(var i = 0; i<buttonState.Length; i++)
            {
                var letter = letters[i];
                if (string.IsNullOrEmpty(letter.Text))
                    break;
                var wordsToRemove = new List<string>();
                switch (buttonState[i])
                {
                    case 1: //Yellow
                        RemoveWords(Words.Where(c => !c.Contains(letter.Text[0]) || c[i] == letter.Text[0]).ToList());
                        break;
                    case 2: //Green
                        RemoveWords(Words.Where(c => c[i] != letter.Text[0]).ToList());
                        break;
                    default: //White
                        RemoveWords(Words.Where(c => c.Contains(letter.Text[0])).ToList());
                        break;
                }
            }
        }

        private void RemoveWords(List<string> wordsToRemove)
        {
            foreach (var word in wordsToRemove)
            {
                Words.Remove(word);
            }
        }

        #region textbox fiddling

        public void TBGotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            textBox.Dispatcher.BeginInvoke(new Action(() => textBox.SelectAll()));
        }

        #endregion

        #region button clicks
        private void Button0_Click(object sender, RoutedEventArgs e)
        {

            buttonState[0] = UpdateButton(Button0);
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            buttonState[1] = UpdateButton(Button1);
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            buttonState[2] = UpdateButton(Button2);
        }

        private void Button3_Click(object sender, RoutedEventArgs e)
        {
            buttonState[3] = UpdateButton(Button3);
        }

        private void Button4_Click(object sender, RoutedEventArgs e)
        {
            buttonState[4] = UpdateButton(Button4);
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateWords();
        }

        #endregion

        private void AnswersFirst_CheckedChanged(object sender, RoutedEventArgs e)
        {
            var isChecked = AnswersFirst.IsChecked == true;

            if (isChecked)
            {
                Words = new ObservableCollection<string>(store.FreshList);
            }
            else
            {
                Words = new ObservableCollection<string>(store.FreshListAlpha); 
            }
            PossibleWords.DataContext = Words;
            Button0.Background = Brushes.White;
            Button1.Background = Brushes.White;
            Button2.Background = Brushes.White;
            Button3.Background = Brushes.White;
            Button4.Background = Brushes.White;
            buttonState = new int[]{ 0, 0, 0, 0, 0};
        }
    }
}
