using System;
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
            checkBoxUpdate.IsChecked = _config.Timer;
            textBoxUpdateTime.Text = _config.UpdateInterval.ToString();
        }

        #region GUI

        private void config_Changed(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            float tmp = 0;
            if (float.TryParse(textBoxUpdateTime.Text, out tmp))
            {
                _config.UpdateInterval = float.Parse(textBoxUpdateTime.Text);
                FileHandler.Save("config.data", _config);
            }
            else
            {
                textBoxUpdateTime.Text = _config.UpdateInterval.ToString();
                InfoWindow info = new InfoWindow("Update interval must be a number!");
                info.ShowDialog();
            }
        }

        private void config_Changed(object sender, RoutedEventArgs e)
        {
            if (sender == checkBoxStartWindows)
            {
                _config.StartWithWindows = checkBoxStartWindows.IsChecked.Value;
                if (checkBoxStartWindows.IsChecked.Value)
                    Utils.CreateShortcutStartWithWindows("UpdateChecker");
                else
                    Utils.EraseFile(Environment.GetFolderPath(Environment.SpecialFolder.Startup) + "//UpdateChecker.lnk");
            }
            else if (sender == checkBoxUpdate)
            {
                _config.Timer = checkBoxUpdate.IsChecked.Value;
            }
            FileHandler.Save("config.data", _config);
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }

        #endregion
    }
}
