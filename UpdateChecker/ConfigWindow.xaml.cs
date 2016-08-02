using System.Windows;
using System.Windows.Input;
using UpdateChecker.Lib;

namespace UpdateChecker
{
    public partial class ConfigWindow : Window
    {
        private Config _config;

        public ConfigWindow(Config config)
        {
            InitializeComponent();

            _config = config;
            FillConfig();
        }

        private void FillConfig()
        {
            checkBoxStartWindows.IsChecked = _config.StartWithWindows;
            checkBoxMinimizeToTray.IsChecked = _config.MinimizeToTray;
            checkBoxUpdate.IsChecked = _config.Timer;
            textBoxUpdateTime.Text = _config.UpdateInterval.ToString();
        }

        #region GUI
        
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
           
        private void config_Changed(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            _config.UpdateInterval = float.Parse(textBoxUpdateTime.Text);
            FileHandler.Save("config.data", _config);
        }
        
        private void config_Changed(object sender, RoutedEventArgs e)
        {
            if (sender == checkBoxStartWindows)
            {
                _config.StartWithWindows = checkBoxStartWindows.IsEnabled;
            }
            else if (sender == checkBoxMinimizeToTray)
            {
                _config.MinimizeToTray = checkBoxMinimizeToTray.IsEnabled;
            }
            else if (sender == checkBoxUpdate)
            {
                _config.Timer = checkBoxUpdate.IsEnabled;
            }
            FileHandler.Save("config.data", _config);
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }

        #endregion
    }
}
