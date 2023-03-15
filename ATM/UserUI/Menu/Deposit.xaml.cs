using ATM.Classes;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Interaction logic for Deposit.xaml
    /// </summary>
    public partial class Deposit : Page
    {
        private string customerNumber;
        private List<Account> accounts;


        public Deposit(string customerNumber)
        {
            InitializeComponent();
            this.customerNumber = customerNumber;
            accounts = GetAccountsFromSql();
            FillUpAccountsList();
        }

        private void RefreshAccountsList()
        {
            accounts = GetAccountsFromSql();
        }

        private void DRP_Accounts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = DRP_Accounts.SelectedIndex;

            LBL_Balance_Header.Visibility = Visibility.Visible;
            LBL_Balance_Value.Visibility = Visibility.Visible;
            LBL_Balance_Value.Content = accounts[index].accountBalance + " HUF";
            TXT_Deposit_Value.Visibility = Visibility.Visible;
            BTN_Deposit.Visibility = Visibility.Visible;
        }


        private void BTN_Deposit_Click(object sender, RoutedEventArgs e)
        {
            int value;
            int index = DRP_Accounts.SelectedIndex;
            if (int.TryParse(TXT_Deposit_Value.Text, out value))
            {

                UpdateAccounts(value, accounts[index].accountId);
                RefreshAccountsList();
                LBL_Balance_Value.Content = accounts[index].accountBalance + " HUF";
                TXT_Deposit_Value.Text = string.Empty;
                MessageBox.Show("Deposit was successful!", "Successful Deposit", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("The input has to be numbers!", "Error", MessageBoxButton.OKCancel, MessageBoxImage.Warning);

            }

        }


        private void FillUpAccountsList()
        {
            for (int i = 0; i < accounts.Count; i++)
            {
                DRP_Accounts.Items.Add($"{accounts[i].accountName}");
            }
        }


        private List<Account> GetAccountsFromSql()
        {
            SQL Sql = new SQL();
            MySqlConnection conn = Sql.conn;
            List<Account> tempAccounts = new List<Account>();
            try
            {
                conn.Open();
                MySqlCommand cmd = Sql.cmd;
                cmd.Connection = conn;
                cmd.CommandText = "SELECT accountId, accountName, accountBalance FROM accounts WHERE customerNumber = @customerNumber";
                cmd.Parameters.AddWithValue("@customerNumber", customerNumber);
                cmd.Prepare();
                var result = cmd.ExecuteReader();
                while (result.Read())
                {
                    tempAccounts.Add(new Account(result.GetString(0), result.GetString(1), result.GetInt32(2)));
                }
                conn.Close();

            }
            catch (Exception)
            {
                conn.Close();
            }

            return tempAccounts;
        }


        private void UpdateAccounts(int depositValue, string accountId)
        {
            SQL Sql = new SQL();
            MySqlConnection conn = Sql.conn;
            try
            {
                conn.Open();
                MySqlCommand cmd = Sql.cmd;

                cmd.Connection = Sql.conn;
                cmd.CommandText = "UPDATE accounts SET accountBalance = accountBalance + @depositValue WHERE customerNumber = @customerNumber AND accountId = @accountId";

                cmd.Parameters.AddWithValue("@depositValue", depositValue);
                cmd.Parameters.AddWithValue("@customerNumber", customerNumber);
                cmd.Parameters.AddWithValue("@accountId", accountId);

                cmd.Prepare();
                cmd.ExecuteNonQuery();

                DepositInsertTranzakcio(depositValue, accountId);
                conn.Close();
            }
            catch (Exception)
            {
                conn.Close();
            }
            
        }


        private void DepositInsertTranzakcio(int depositValue, string accountId)
        {
            SQL Sql = new SQL();
            MySqlConnection conn = Sql.conn;
            try
            {
                conn.Open();
                MySqlCommand cmd = Sql.cmd;

                cmd.Connection = Sql.conn;
                cmd.CommandText = "INSERT INTO `transactions`(`accountId`, `transactionType`, `processedAmount`, `transactionDate`) VALUES ( @accountId ,\"DEPOSIT\" , @depositValue , NOW())";

                cmd.Parameters.AddWithValue("@accountId", accountId);
                cmd.Parameters.AddWithValue("@depositValue", depositValue);

                cmd.Prepare();
                cmd.ExecuteNonQuery();

                conn.Close();
            }
            catch (Exception)
            {
                conn.Close();
            }
        }


    }
}
