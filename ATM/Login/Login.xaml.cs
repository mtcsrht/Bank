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
using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using Org.BouncyCastle.Utilities.Collections;
using ATM.Classes;

namespace ATM
{

    public partial class Login : Page
    {
        public Login()
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
            /*
            const string accountNumber = "1234123456789";
            const string passwd = "54d5cb2d332dbdb4850293caae4559ce88b65163f1ea5d4e4b3ac49d772ded14";
            const string firstName = "Golden";
            const string lastName = "Brown";
            */
            string numberInput = TXT_Account.Text.Remove(4, 1);
            string passwordInput = PSWD_Passwd.Password;
            if (numberInput.Length != 13)
            {
                MessageBox.Show("The account number's length has to be 13 numbers", "Error", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            }
            else
            {
                if (!IsAccNumberExists(numberInput))
                {
                    MessageBox.Show("Couldn't find an account with this number", "Error", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                }
                else
                {
                    if (!CanLogin(numberInput, ComputeSHA256(passwordInput)))
                    {
                        MessageBox.Show("Wrong password!", "Error", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                    }
                    else
                    {
                        UpdateLastLogin(numberInput);
                        UserUI ui = new UserUI(numberInput);
                        this.NavigationService.Navigate(ui);
                    }
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
        private static void UpdateLastLogin(string accNumber)
        {
            SQL Sql = new SQL();
            MySqlConnection conn = Sql.conn;
            try
            {
                conn.Open();
                string sql = $"UPDATE `accounts` SET `lastLogin`= NOW() WHERE accountNumber = @accNumber";
                MySqlCommand cmd = Sql.cmd;
                cmd.CommandText = sql;
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@accNumber", accNumber);
                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            { 
                conn.Close();

            }
        }

        private static bool IsAccNumberExists(string accNumber)
        {
            SQL Sql = new SQL();
            MySqlConnection conn = Sql.conn;
            try 
            { 

                conn.Open();
                string sql = $"SELECT * FROM accounts WHERE accountNumber = @accNumber";
                MySqlCommand cmd = Sql.cmd;
                cmd.CommandText = sql;
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@accNumber", accNumber);
                cmd.Prepare();
                var result = cmd.ExecuteScalar();

                if (result != null)
                {
                    conn.Close();
                    return true;
                }
                else
                {
                    conn.Close();
                    return false;
                }
            }
            catch (Exception)
            {
                conn.Close();
                return false;

            }
        }
        private static bool CanLogin(string accnumber, string pwd)
        {
            SQL Sql = new SQL();
            MySqlConnection conn = Sql.conn;
            try
            {
                conn.Open();
                string sql = $"SELECT * FROM accounts WHERE accountNumber = @accNumber AND password = @password";
                MySqlCommand cmd = Sql.cmd;
                cmd.CommandText = sql;
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@accNumber", accnumber);
                cmd.Parameters.AddWithValue("@password", pwd);
                cmd.Prepare();
                var result = cmd.ExecuteScalar();

                if (result != null)
                {
                    conn.Close();
                    return true;
                }
                else
                {
                    conn.Close();
                    return false;
                }
            }
            catch (Exception)
            {
                conn.Close();
                return false;

            }
        }
    }
}
