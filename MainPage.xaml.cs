using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Hangman
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public static int level = (int)difficulty.medium;  //default difficulty is medium hardness
        private enum difficulty { easy = 5, medium = 7, hard = Int16.MaxValue};   //for game difficulty        
        
        public MainPage()
        {
            this.InitializeComponent();
            GamePage.initDictionary();
        }

        private void level_Click(object sender, RoutedEventArgs e)
        {
            string chosen = ((Control)sender).Name;
            level = (int)Enum.Parse(typeof(difficulty), chosen);
            Frame.Navigate(typeof(GamePage));
        }
    }
}
