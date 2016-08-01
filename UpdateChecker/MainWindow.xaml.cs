using System.Windows;
using UpdateChecker.Lib;

namespace UpdateChecker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Checker checker;

        public MainWindow()
        {
            InitializeComponent();
            checker = new Checker();
        }

        private void buttonCheck_Click(object sender, RoutedEventArgs e)
        {
            if (checker.Check(textUrl.Text))
            {
                labelResult.Content = "Pages are equal";
            }
            else
            {
                labelResult.Content = "The page has changed";
            }
        }
    }
}
