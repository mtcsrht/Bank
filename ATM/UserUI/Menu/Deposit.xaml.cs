using ATM.Classes;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
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
            accounts = new SQL().GetAccountsFromSql(customerNumber);
            FillUpAccountsList();
        }

        private void RefreshAccountsList()
        {
            accounts = new SQL().GetAccountsFromSql(customerNumber);
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


        private void UpdateAccounts(int depositValue, string accountId)
        {
            SQL Sql = new SQL();
            try
            {
                Sql.conn.Open();
                MySqlCommand cmd = Sql.cmd;

                cmd.Connection = Sql.conn;
                cmd.CommandText = "UPDATE accounts SET accountBalance = accountBalance + @depositValue WHERE customerNumber = @customerNumber AND accountId = @accountId";

                cmd.Parameters.AddWithValue("@depositValue", depositValue);
                cmd.Parameters.AddWithValue("@customerNumber", customerNumber);
                cmd.Parameters.AddWithValue("@accountId", accountId);

                cmd.Prepare();
                cmd.ExecuteNonQuery();

                DepositInsertTransaction(depositValue, accountId);
                Sql.conn.Close();
            }
            catch (Exception)
            {
                Sql.conn.Close();
            }
            
        }


        private void DepositInsertTransaction(int depositValue, string accountId)
        {
            SQL Sql = new SQL();
            try
            {
                Sql.conn.Open();

                Sql.cmd.Connection = Sql.conn;
                Sql.cmd.CommandText = "INSERT INTO `transactions`(`accountId`, `transactionType`, `processedAmount`, `transactionDate`) VALUES ( @accountId ,\"DEPOSIT\" , @depositValue , NOW())";

                Sql.cmd.Parameters.AddWithValue("@accountId", accountId);
                Sql.cmd.Parameters.AddWithValue("@depositValue", depositValue);

                Sql.cmd.Prepare();
                Sql.cmd.ExecuteNonQuery();

                Sql.conn.Close();
            }
            catch (Exception)
            {
                Sql.conn.Close();
            }
        }


    }
}
