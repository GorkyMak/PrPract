using System;
using System.Windows;

namespace PrPract
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            if (Properties.Settings.Default.UserRole != "")
            {
                LblWelcome.Content += Properties.Settings.Default.UserRole;
                if (Properties.Settings.Default.UserRole == "Оператор")
                    BtnOpenUser.Visibility = Visibility.Collapsed;
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            if (FrMain.CanGoBack)
                FrMain.GoBack();
        }

        private void BtnGo_Click(object sender, RoutedEventArgs e)
        {
            if (FrMain.CanGoForward)
                FrMain.GoForward();
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            new Authorization().Show();
            Close();
        }

        private void BtnOpenUser_Click(object sender, RoutedEventArgs e)
        {
            FrMain.Navigate(new Uri("../Pages/Info/InfoUsers.xaml", UriKind.Relative));
        }

        private void BtnOpenAct_Click(object sender, RoutedEventArgs e)
        {
            FrMain.Navigate(new Uri("../Pages/Info/InfoActs.xaml", UriKind.Relative));
        }

        private void BtnOpenStat_Click(object sender, RoutedEventArgs e)
        {
            FrMain.Navigate(new Uri("../Pages/Info/InfoStat.xaml", UriKind.Relative));

        }

        private void BtnOpenTariff_Click(object sender, RoutedEventArgs e)
        {
            FrMain.Navigate(new Uri("../Pages/Info/InfoTariff.xaml", UriKind.Relative));

        }

        private void BtnOpenAbonent_Click(object sender, RoutedEventArgs e)
        {
            FrMain.Navigate(new Uri("../Pages/Info/InfoAbonent.xaml", UriKind.Relative));
        }
    }
}