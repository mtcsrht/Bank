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
using System.Security.Cryptography;


namespace ATM
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        static string ComputeSHA256(string s)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashValue = sha256.ComputeHash(Encoding.UTF8.GetBytes(s));
                return Convert.ToHexString(hashValue);
            }
        }

        private void BTN_Login_Click(object sender, RoutedEventArgs e)
        {
            const string accountNumber = "1234123456789";
            const string passwd = "54d5cb2d332dbdb4850293caae4559ce88b65163f1ea5d4e4b3ac49d772ded14";
            const string firstName = "Golden";
            const string lastName = "Brown";

            string numberInput = TXT_Account.Text.Remove(4, 1); ;
            string passwordInput = PSWD_Passwd.Password;

            if (accountNumber != numberInput)
            {
                MessageBox.Show("Couldn't find an account with this number", "Error", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                MessageBox.Show(numberInput);
            }
            else
            {
                if (passwd.ToUpper() != ComputeSHA256(passwordInput))
                {
                    MessageBox.Show("Wrong password!", "Error", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                    MessageBox.Show(ComputeSHA256(passwordInput));
                }
                else
                {
                    UserUI ui = new UserUI(firstName, lastName);
                    this.Content = ui;
                }
            }

        }

        private void TXT_Account_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TXT_Account.Text.Length == 4)
            {
                TXT_Account.Text += "-";
                TXT_Account.Select(TXT_Account.Text.Length, 0);
            }
        }
    }
}
