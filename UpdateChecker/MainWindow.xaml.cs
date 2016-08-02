using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using UpdateChecker.Lib;

namespace UpdateChecker
{
    public partial class MainWindow : Window
    {
        Checker checker;
        Brush closeButtonHoverEnter, closeButtonHoverExit, closeButtonPressed;

        public MainWindow()
        {
            InitializeComponent();
            checker = new Checker();

            // Thanks to http://loading.io for generating this free gif
            loadingImage.Source = null;
            closeButtonHoverEnter = new SolidColorBrush(Colors.PaleVioletRed);
            closeButtonHoverExit = new SolidColorBrush(Colors.White);
            closeButtonPressed = new SolidColorBrush(Colors.Crimson);
        }

        private void SetStatus(string status)
        {
            labelResult.Content = status;
        }
        
        private void SetLoadingImage(bool need)
        {
            if (need)
            {
                loadingImage.Source = new Uri("Resources/loading.gif", UriKind.Relative);
                loadingImage.Play();
            }
            else
            {
                loadingImage.Source = null;
                loadingImage.Stop();
            }
        }

        private async void buttonCheck_Click(object sender, RoutedEventArgs e)
        {
            SetLoadingImage(true);
            switch (await checker.Check(textUrl.Text))
            {
                case Status.FIRST_QUERY:
                    SetStatus("First time requesting website.");
                    break;
                case Status.DIFFERENT:
                    SetStatus("Requested website is different from the last one.");
                    break;
                case Status.CHANGED:
                    SetStatus("Website has changed!");
                    break;
                case Status.NO_CHANGES:
                    SetStatus("Website still the same.");
                    break;
                case Status.CONNECTION_ERROR:
                    SetStatus("Error connecting to this page. Check your internet connection and try again.");
                    break;
                case Status.FILE_ERROR:
                    SetStatus("Error saving or reading the page. Check your permissions for this folder.");
                    break;
            }
            SetLoadingImage(false);
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
