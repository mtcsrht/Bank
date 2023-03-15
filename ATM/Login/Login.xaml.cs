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
            string customerNumberInput = TXT_Account.Text.Remove(4, 1);
            string passwordInput = PSWD_Passwd.Password;
            if (customerNumberInput.Length != 13)
            {
                MessageBox.Show("The account number's length has to be 13 numbers", "Error", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            }
            else
            {
                if (!IsAccNumberExists(customerNumberInput))
                {
                    MessageBox.Show("Couldn't find an account with this number", "Error", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                }
                else
                {
                    if (!CanLogin(customerNumberInput, ComputeSHA256(passwordInput)))
                    {
                        MessageBox.Show("Wrong password!", "Error", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                    }
                    else
                    {
                        UpdateLastLogin(customerNumberInput);
                        UserUI ui = new UserUI(customerNumberInput);
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


        private static void UpdateLastLogin(string customerNumber)
        {
            SQL Sql = new SQL();
            MySqlConnection conn = Sql.conn;
            try
            {
                conn.Open();
                string sql = $"UPDATE customers SET lastLogin = NOW() WHERE customerNumber = @customerNumber";
                MySqlCommand cmd = Sql.cmd;
                cmd.CommandText = sql;
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@customerNumber", customerNumber);
                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            { 
                conn.Close();

            }
        }


        private static bool IsAccNumberExists(string customerNumber)
        {
            SQL Sql = new SQL();
            MySqlConnection conn = Sql.conn;
            try 
            { 

                conn.Open();
                string sql = $"SELECT * FROM customers WHERE customerNumber = @customerNumber";
                MySqlCommand cmd = Sql.cmd;
                cmd.CommandText = sql;
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@customerNumber", customerNumber);
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


        private static bool CanLogin(string customerNumber, string pwd)
        {
            SQL Sql = new SQL();
            MySqlConnection conn = Sql.conn;
            try
            {
                conn.Open();
                string sql = $"SELECT * FROM customers WHERE customerNumber = @customerNumber AND password = @password";
                MySqlCommand cmd = Sql.cmd;
                cmd.CommandText = sql;
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@customerNumber", customerNumber);
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
