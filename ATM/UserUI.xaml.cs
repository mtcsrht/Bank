using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ATM
{
    /// <summary>
    /// Interaction logic for UserUI2.xaml
    /// </summary>
    public partial class UserUI : Page
    {
        public string accountNumber;
        public UserUI(string accNumber)
        {
            accountNumber = accNumber;
            InitializeComponent();
            LBL_welcome.Content = $"Welcome {accNumber}";

            SecondFrame.Content = "Please Choose!";

        }

        private void BTN_Balance_Click(object sender, RoutedEventArgs e)
        {
            Balance bal = new Balance();

            SecondFrame.Content = bal;
        }

        private void BTN_Logout_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("Login.xaml",UriKind.Relative));
        }
    }
}
