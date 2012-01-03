using System;
using System.Collections.Generic;
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
    public partial class Info : PhoneApplicationPage
    {
        /* CONSTRUCTOR */

        public Info()
        {
            InitializeComponent();
        }

        /* EVENT HANDLERS */

        // Application Bar, Sends user to CREATE
        private void Create_Clicked(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Create.xaml", UriKind.Relative));
        }

        // Application Bar, Sends user to RANDOM
        private void Random_Clicked(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/RandomQuote.xaml", UriKind.Relative));
        }

        // Application Bar, Sends user to HOME
        private void Home_Clicked(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Home.xaml", UriKind.Relative));
        }
        
        // Application Bar, Sends user to INFO
        private void Info_Clicked(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Info.xaml", UriKind.Relative));
        }

        // Menu item, sends user to INFO_CREATE
        private void Info_Create(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Info_Create.xaml", UriKind.Relative));
        }

        // Menu item, sends uer to INFO_RANDOM
        private void Info_Random(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Info_Random.xaml", UriKind.Relative));
        }
    }
}