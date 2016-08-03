using System;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
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
        NotifyIcon notifyIcon;
        ToastWindow toast;

        public MainWindow()
        {
            InitializeComponent();
            checker = new Checker();

            LoadConfig();                   // auto update timer is handled here
            ConfigMinimizeToTray();
        }
        
        private async void Check()
        {
            SetLoadingImage(true);
            switch (await checker.Check(textUrl.Text))
            {
                case Status.FIRST_QUERY:
                    SetStatus("First time requesting website.");
                    SetStatusColor(System.Windows.Media.Color.FromRgb(196, 122, 79));
                    break;
                case Status.DIFFERENT:
                    SetStatus("Requested website is different from the last one.");
                    SetStatusColor(System.Windows.Media.Color.FromRgb(196, 122, 79));
                    break;
                case Status.CHANGED:
                    SetStatus("Website has changed!");
                    SetStatusColor(System.Windows.Media.Color.FromRgb(137, 176, 59));
                    toast = new ToastWindow("Website has changed!");
                    break;
                case Status.NO_CHANGES:
                    SetStatus("Website still the same.");
                    SetStatusColor(System.Windows.Media.Color.FromRgb(30, 30, 30));
                    break;
                case Status.CONNECTION_ERROR:
                    SetStatus("Error connecting to this page. Check your internet connection and try again.");
                    SetStatusColor(System.Windows.Media.Color.FromRgb(130, 53, 43));
                    break;
                case Status.FILE_ERROR:
                    SetStatus("Error saving or reading the page. Check your permissions for this folder.");
                    SetStatusColor(System.Windows.Media.Color.FromRgb(130, 53, 43));
                    break;
            }
            SetUpdateDate(DateTime.Now.ToString());
            ConfigTimer(config.Timer);          // resets the counter (I prefer this way but it can be disabled)
            SetLoadingImage(false);
        }

        #region Config

        public void LoadConfig()
        {
            config = (Config)FileHandler.Read("config.data");

            // First time launching program, set all configs to default
            if (config == null)
            {
                config = new Config();
                config.StartWithWindows = false;
                config.Timer = false;
                config.UpdateInterval = 1;

                FileHandler.Save("config.data", config);
            }
            ConfigTimer(config.Timer);
        }

        private void ConfigTimer(bool enabled)
        {
            if (config.Timer)
            {
                timerAutoUpdate = new DispatcherTimer();
                timerAutoUpdate.Interval = TimeSpan.FromMinutes(config.UpdateInterval);
                timerAutoUpdate.IsEnabled = true;
                timerAutoUpdate.Tick += TimerAutoUpdate_Tick;

                SetTimeRemainingImage(true);
                labelTimeRemaining.Content = config.UpdateInterval;
            }
            else
            {
                timerAutoUpdate = null;
                SetTimeRemainingImage(false);
                labelTimeRemaining.Content = "";
            }
        }
        
        private void ConfigMinimizeToTray()
        {
            notifyIcon = new NotifyIcon();
            notifyIcon.Icon = new Icon("Resources/uc.ico");
            notifyIcon.Visible = true;
            notifyIcon.DoubleClick += delegate (object sender, EventArgs args)
            {
                Show();
                WindowState = WindowState.Normal;
            };
        }

        #endregion

        #region Timer

        private void TimerAutoUpdate_Tick(object sender, EventArgs e)
        {
            Check();
        }
        
        #endregion

        #region GUI

        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == WindowState.Normal)
                Hide();

            base.OnStateChanged(e);
        }

        private void SetStatus(string status)
        {
            labelResult.Content = status;
        }

        private void SetStatusColor(System.Windows.Media.Color color)
        {
            labelResult.Foreground = new SolidColorBrush(color);
        }

        private void SetUpdateDate(string date)
        {
            labelInfo.Content = date;
        }

        private void SetTimeRemainingImage(bool need)
        {
            if (need)
            {
                timeRemainingImage.Source = new Uri("Resources/hourglass.png", UriKind.Relative);
                timeRemainingImage.Play();
            }
            else
            {
                timeRemainingImage.Source = null;
                timeRemainingImage.Stop();
            }
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

        private void buttonMinimize_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        #endregion
    }
}
