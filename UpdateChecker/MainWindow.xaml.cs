using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using UpdateChecker.Lib;

namespace UpdateChecker
{
    public partial class MainWindow : Window
    {
        Checker checker;
        DispatcherTimer timerAutoUpdate;
        Config config;

        public MainWindow()
        {
            InitializeComponent();
            checker = new Checker();
            
            LoadConfig();
        }

        public void LoadConfig()
        {
            config = (Config)FileHandler.Read("config.data");

            // First time launching program, set all configs to default
            if (config == null)
            {
                config = new Config();
                config.StartWithWindows = false;
                config.MinimizeToTray = false;
                config.Timer = false;
                config.UpdateInterval = 0;

                FileHandler.Save("config.data", config);
            }
            ConfigTimer(config.Timer);
        }

        private async void Check()
        {
            SetLoadingImage(true);
            switch (await checker.Check(textUrl.Text))
            {
                case Status.FIRST_QUERY:
                    SetStatus("First time requesting website.");
                    SetStatusColor(Color.FromRgb(196, 122, 79));
                    break;
                case Status.DIFFERENT:
                    SetStatus("Requested website is different from the last one.");
                    SetStatusColor(Color.FromRgb(196, 122, 79));
                    break;
                case Status.CHANGED:
                    SetStatus("Website has changed!");
                    SetStatusColor(Color.FromRgb(137, 176, 59));
                    break;
                case Status.NO_CHANGES:
                    SetStatus("Website still the same.");
                    SetStatusColor(Color.FromRgb(30, 30, 30));
                    break;
                case Status.CONNECTION_ERROR:
                    SetStatus("Error connecting to this page. Check your internet connection and try again.");
                    SetStatusColor(Color.FromRgb(130, 53, 43));
                    break;
                case Status.FILE_ERROR:
                    SetStatus("Error saving or reading the page. Check your permissions for this folder.");
                    SetStatusColor(Color.FromRgb(130, 53, 43));
                    break;
            }
            SetUpdateDate(DateTime.Now.ToString());
            SetLoadingImage(false);
        }
        
        #region Timer

        private void ConfigTimer(bool enabled)
        {
            if (config.Timer)
            {
                timerAutoUpdate = new DispatcherTimer();
                timerAutoUpdate.Interval = TimeSpan.FromMinutes(config.UpdateInterval);
                timerAutoUpdate.IsEnabled = true;
                timerAutoUpdate.Tick += TimerAutoUpdate_Tick;
            }
            else
            {
                timerAutoUpdate = null;
            }
        }

        private void TimerAutoUpdate_Tick(object sender, EventArgs e)
        {
            Check();
        }

        #endregion

        #region GUI

        private void SetStatus(string status)
        {
            labelResult.Content = status;
        }

        private void SetStatusColor(Color color)
        {
            labelResult.Foreground = new SolidColorBrush(color);
        }

        private void SetUpdateDate(string date)
        {
            labelInfo.Content = date;
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

        private void buttonCheck_Click(object sender, RoutedEventArgs e)
        {
            Check();
        }

        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void configButton_Click(object sender, RoutedEventArgs e)
        {
            ConfigWindow configWindow = new ConfigWindow(config);
            configWindow.ShowDialog();

            // Update config
            LoadConfig();
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
        
        #endregion


    }
}
