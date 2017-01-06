using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Hangman
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GamePage : Page
    {
        static words myWords;   //ALL THE WORDS 
        bool game = true;   //is the game still going
        string word = "";   //word for single round. 
        char[] wordArr;     //array of the actual word
        char[] guesses;     //what the person guessed 
        int misses = 0;     //how many wrong guesses? 0->5
        HashSet<string> miss = new HashSet<string>();   //wrong letters go here
        HashSet<string> right = new HashSet<string>();  //correct letters go here       

        public GamePage()
        {
            this.InitializeComponent();           
            resetGame();  //get a word for the game
        }

        public static void initDictionary()
        {
            myWords = new words("ms-appx:///Assets/dictionary"); //load the words @start
        }

        //handle the reset button click.
        //picks a random words from the dictionary, clean up the remains of last game
        private void newGame_Click(object sender, RoutedEventArgs e)
        {
            resetGame();
        }

        //reset the remains of the game and delegate the word picking to newWord();
        private void resetGame()
        {
            hangman.Source = new BitmapImage(new Uri("ms-appx:///Assets/0.png"));

            Wrong.Text = "";
            Word.Text = "";
            misses = 0;

            game = true;
            reveal.IsEnabled = false;

            miss.Clear();
            right.Clear();
            newWord();
        }

        //pick a new word @ random
        public void newWord()
        {
            do{
                this.word = myWords.next();
            }while (word.Length > MainPage.level);

            Length.Text = word.Length.ToString();
            wordArr = word.ToCharArray();
            guesses = new char[wordArr.Length]; //set appropo length 

            for (int i = 0; i < wordArr.Length; ++i)
                guesses[i] = '-';

            Word.Text = new string(guesses);
            //Word.Text = this.word;  //for debug purposes. 
        }

        // handle any keyboard click beside the "reset" button
        // check to see if the word (name of the button) is a valid guess by calling guess function
        private void Click(object sender, RoutedEventArgs e)
        {
            if (misses < 5)
                guess(((Control)sender).Name);
        }

        /**
         * name: private void guess(string letter);
         * description: sees if the letter that the user clicked is in the word or not.
         *              If it is a duplicate guess, ignore it. 
         */
        private void guess(string letter)
        {
            if (game)   //is the game still going
            {
                if (word.Contains(letter))  //correct guess
                {
                    if (!right.Contains(letter))
                    {
                        for (int i = 0; i < wordArr.Length; ++i)
                            if (wordArr[i].ToString() == letter)
                                guesses[i] = wordArr[i];

                        Word.Text = new string(guesses);
                        right.Add(letter);
                        check();
                    }
                }
                else
                {  //wrong guess
                    if (!miss.Contains(letter))
                    {
                        Wrong.Text += " " + letter;
                        miss.Add(letter);
                        misses++;
                        changeImage(misses);
                    }
                }
            }
        }

        //check if the player has won the game 
        private async void check()
        {
            try
            {
                if (wordArr.SequenceEqual(guesses)) //player wins, try to set the image to win image
                {
                    game = false;
                    hangman.Source = new BitmapImage(new Uri("ms-appx:///Assets/win.png"));
                }
            }
            catch (Exception)
            {
                var mbox = new MessageDialog("YOU WIN\nbut something went wrong... sorry");
                await mbox.ShowAsync();
            }
        }

        //change the image to appropriate number. 
        private async void changeImage(int missCount)
        {
            if (missCount < 5)  //still got some guesses left 
            {
                try
                {
                    Uri baseUri = new Uri("ms-appx:///Assets/");
                    hangman.Source = new BitmapImage(new Uri(baseUri, misses.ToString() + ".png"));
                }
                catch (Exception)
                {
                    var mbox = new MessageDialog("Something went wrong while handling input", "Error");
                    await mbox.ShowAsync();
                }
            }
            else
            {   //game over at this point 
                try
                {
                    hangman.Source = new BitmapImage(new Uri("ms-appx:///Assets/5.png"));
                    reveal.IsEnabled = true;
                    game = false;
                }
                catch (Exception)
                {
                    var mbox = new MessageDialog("Something went wrong while handling input", "Error");
                    await mbox.ShowAsync();
                }
            }
        }

        // handler for reveal button, which only activates once the game ends
        // replace the word in the "Word" textbox with the correct word. 
        // game won't be able to be played at this point
        private void reveal_Click(object sender, RoutedEventArgs e)
        {
            Word.Text = word;
        }

        private void menu_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }
    }
}