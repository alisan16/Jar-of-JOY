using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace JarOfJOYIntegrated
{
    public partial class Home : PhoneApplicationPage
    {
        /* CONSTRUCTOR */

        public Home()
        {
            InitializeComponent();
        }

        /* EVENT HANDLERS */

        // On click send user to HOME
        private void Home_Clicked(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Home.xaml", UriKind.Relative));
        }

        // On click send user to RANDOM
        private void Random_Clicked(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/RandomQuote.xaml", UriKind.Relative));
        }
        
        // On click send user to CREATE
        private void Create_Clicked(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Create.xaml", UriKind.Relative));
        }

        // On click send user to INFO
        private void Info_Clicked(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Info.xaml", UriKind.Relative));
        }
    }
}