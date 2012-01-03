using ShakeGestures;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace JarOfJOYIntegrated
{
    public partial class RandomQuote : PhoneApplicationPage
    {
        /* VARIABLES */

        // Initialize string for quote data
        public static string textString;
        
        /* CONSTRUCTOR */

        // Setting up the page
        public RandomQuote()
        {
            // Register shake event
            // CREDIT: Microsoft ShakeGestures Example from http://windowsteamblog.com/windows_phone/b/wpdev/archive/2011/02/22/windows-phone-shake-gestures-library.aspx
            ShakeGesturesHelper.Instance.ShakeGesture +=
                new EventHandler<ShakeGestureEventArgs>(Instance_ShakeGesture);
            ShakeGesturesHelper.Instance.MinimumRequiredMovesForShake = 4;
            ShakeGesturesHelper.Instance.Active = true;

            // Initialize the page
            InitializeComponent();

            // Download the text information from the cloud
            // CREDIT: Joe Healy, Microsoft
            DownloadString("http://cloud.cs50.net/~jming/quotes.txt");

        }

        /* ANSWERS */

        // Create list of answers
        // CREDIT: Microsoft ShakeGesture Library Example
        public List<string> Answers
        {
            get
            {
                // Make sure this is either the first time or new quote added
                if ((App.Apple.firstTime == true) || (App.Apple.addedQuote == true))
                {
                    // Load the answers
                    LoadAnswers();

                    // Reinitialize variables
                    App.Apple.firstTime = false;
                    App.Apple.addedQuote = false;
                }

                // Return the list of quotes
                return App.Apple.quoteList;
            }
        }

        // Generate a random number and retrieve this item from the answer list.
        private string GetAnswer()
        {
            Random random = new Random();
            int randomNumber = random.Next(Answers.Count);
            return Answers[randomNumber];
        }

        // Load the answers from the text file.
        private void LoadAnswers()
        {
            // If unable to reach web source
            if (textString == null)
            {
                try
                {
                    // Use a StreamReader to read a local text file
                    using (StreamReader reader = new StreamReader(Application.GetResourceStream(new Uri("quotes.txt", UriKind.Relative)).Stream))
                    {
                        string line;

                        // Append each line to the quoteList
                        while ((line = reader.ReadLine()) != null)
                            App.Apple.quoteList.Add(line);
                    }
                }
                catch
                {
                }
            }
            // But if able to download quotes from web source
            else
            {
                // Transfer the characters of the string to an array
                char[] s = textString.ToCharArray();

                // Calculate the length of the string
                int length = s.Length;

                // Initialize a temporary string
                string temp = "";

                // Loop through each character in the array
                for (int i = 0; i < length; i++)
                {
                    // Keep appending to temporary string if not new line character
                    if (s[i] != '\n')
                    {
                        temp += s[i];
                    }
                    // Append to quoteList if reach end of line and reinitialize temporary
                    else
                    {
                        App.Apple.quoteList.Add(temp);
                        temp = "";
                    }
                }
            }
        } 
        
        /* DOWNLOAD */

        // Download from web source as string
        public static void DownloadString(string address)
        {
            // Create new WebClient and Uri
            WebClient client = new WebClient();
            Uri uri = new Uri(address);

            // Specify that the DownloadStringCallback2 method gets called when download completes.
            client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(DownloadStringCallback2);
            client.DownloadStringAsync(uri);
        }

        // When called by handler, save result as textString
        private static void DownloadStringCallback2(Object sender, DownloadStringCompletedEventArgs e)
        {
            if (!e.Cancelled && e.Error == null)
            {
                textString = (string)e.Result;
            }
        }
        
        /* SHAKE GESTURES */

        // Set the data context of the TextBlock to the answer.
        void Instance_ShakeGesture(object sender, ShakeGestureEventArgs e)
        {      
            // Use BeginInvoke to write to the UI thread.
            quoteBlock.Dispatcher.BeginInvoke(() =>
            {
                // Write the answer into the quoteBlock
                quoteBlock.DataContext = GetAnswer();
                quoteBlock.Text = GetAnswer();
            });
        }

        /* EVENT HANDLERS */

        // Application navigation bar, send user to HOME
        private void Home_Clicked(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Home.xaml", UriKind.Relative));
        }

        // Application navigation bar, send user to RANDOM page
        private void Random_Clicked(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/RandomQuote.xaml", UriKind.Relative));
        }

        // Application navigation bar, send user to CREATE page
        private void Create_Clicked(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Create.xaml", UriKind.Relative));
        }

        // Application navigation bar, send user to INFO
        private void Info_Clicked(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Info.xaml", UriKind.Relative));
        }

        // More information, send user to INFO_RANDOM
        private void Info_Random(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Info_Random.xaml", UriKind.Relative));
        }
    }
}