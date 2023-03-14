using ATM.Classes;
using MySql.Data.MySqlClient;
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
    /// Interaction logic for UserUI.xaml
    /// </summary>
    public partial class UserUI : Page
    {
        private string accountNumber;
        private string firstName = string.Empty;
        private string lastName = string.Empty;

        public UserUI(string accountNumber)
        {
            this.accountNumber = accountNumber;
            InitializeComponent();
            GetDataFromSql();
            LBL_welcome.Content = $"Welcome {firstName} {lastName}";

        }

        private void GetDataFromSql()
        {
            SQL Sql = new SQL();
            MySqlConnection conn = Sql.conn;
            try { 


                conn.Open();
                MySqlCommand cmd = Sql.cmd;
                cmd.Connection = conn;
                cmd.CommandText = "SELECT firstName, lastName FROM accounts WHERE accountNumber = @accountNumber";
                cmd.Parameters.AddWithValue("@accountNumber", accountNumber);
                cmd.Prepare();
                var result = cmd.ExecuteReader();
                if (result.Read())
                {
                    firstName = result.GetString(0);
                    lastName = result.GetString(1);
                }

            }
            catch (Exception)
            {
                conn.Close();
            }
        }
        private void BTN_Balance_Click(object sender, RoutedEventArgs e)
        {
            Balance bal = new Balance(accountNumber);
            SecondFrame.Navigate(bal);
        }

        private void BTN_Logout_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("Login/Login.xaml",UriKind.Relative));
        }

        private void BTN_Deposit_Click(object sender, RoutedEventArgs e)
        {
            Deposit dep = new Deposit(accountNumber);
            SecondFrame.Navigate(dep);
        }
    }
}
