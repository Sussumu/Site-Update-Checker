using System.Windows;

namespace UpdateChecker
{
    public partial class InfoWindow : Window
    {
        public InfoWindow(string message)
        {
            InitializeComponent();

            labelMessage.Content = message;
        }

        private void buttonCheck_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }
    }
}
