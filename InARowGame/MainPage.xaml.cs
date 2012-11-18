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
using Microsoft.Phone.Marketplace;
using Microsoft.Phone.Tasks;

namespace InARowGame
{
    public partial class MainPage : PhoneApplicationPage
    {
        private readonly string PlayUrl = "/PlayPage.xaml?rows={0}&columns={1}";

        public MainPage()
        {
            InitializeComponent();

            //if (App.IsTrial)
            //{
            //    BuyButton.Visibility = System.Windows.Visibility.Visible;
            //}
        }

        private void SmallButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri(string.Format(PlayUrl, 10,6), UriKind.Relative));
        }

        private void MediumButton_Click(object sender, RoutedEventArgs e)
        {
            //if (App.IsTrial)
            //    AskMarketPlace();
            //else
                NavigationService.Navigate(new Uri(string.Format(PlayUrl, 13, 8), UriKind.Relative));
        }

        private void LargeButton_Click(object sender, RoutedEventArgs e)
        {
            //if (App.IsTrial)
            //    AskMarketPlace();
            //else
                NavigationService.Navigate(new Uri(string.Format(PlayUrl, 15, 10), UriKind.Relative));
        }


        private void BuyButton_Click(object sender, RoutedEventArgs e)
        {
            GoToMarketPlace();
        }

        private void AskMarketPlace()
        {
            MessageBoxResult mbResult = MessageBox.Show("This feature is only available in the full version. Go to the marketplace now?", "Limited trial", MessageBoxButton.OKCancel);
            if (mbResult == MessageBoxResult.OK)
            {
                GoToMarketPlace();
            }
        }

        private void GoToMarketPlace()
        {
            MarketplaceDetailTask detailTask = new MarketplaceDetailTask();
            detailTask.Show();
        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/AboutPage.xaml", UriKind.Relative));
        }
    }
}