using ATM.Classes;
using Google.Protobuf.WellKnownTypes;
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
    /// Interaction logic for Withdraw.xaml
    /// </summary>
    public partial class Withdraw : Page
    {
        private string customerNumber;
        private List<Account> accounts;

        public Withdraw(string customerNumber)
        {
            InitializeComponent();
            this.customerNumber = customerNumber;
            accounts = new SQL().GetAccountsFromSql(customerNumber);
            FillUpAccountsList();
        }


        private void BTN_Withdraw_Click(object sender, RoutedEventArgs e)
        {
            int value;
            int index = DRP_Accounts.SelectedIndex;
            if (int.TryParse(TXT_Withdraw_Value.Text, out value))
            {
                if (value < accounts[index].accountBalance)
                {
                    UpdateAccounts(value, accounts[index].accountId);
                    RefreshAccountsList();
                    LBL_Balance_Value.Content = accounts[index].accountBalance + " HUF";
                    TXT_Withdraw_Value.Text = string.Empty;
                    MessageBox.Show("Withdraw was successful!", "Successful Withdraw", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else if (value == accounts[index].accountBalance)
                {
                   var result = MessageBox.Show("Are you sure to withdraw all your money from this account?", "Error", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if(result == MessageBoxResult.Yes)
                    {
                        UpdateAccounts(value, accounts[index].accountId);
                        RefreshAccountsList();
                        LBL_Balance_Value.Content = accounts[index].accountBalance + " HUF";
                        TXT_Withdraw_Value.Text = string.Empty;
                        MessageBox.Show("Withdraw was successful!", "Successful Withdraw", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    MessageBox.Show("You don't have enough money in the account!", "Error", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("The input has to be numbers!", "Error", MessageBoxButton.OKCancel, MessageBoxImage.Warning);

            }
        }


        private void DRP_Accounts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = DRP_Accounts.SelectedIndex;

            LBL_Balance_Header.Visibility = Visibility.Visible;
            LBL_Balance_Value.Visibility = Visibility.Visible;
            LBL_Balance_Value.Content = accounts[index].accountBalance + " HUF";
            TXT_Withdraw_Value.Visibility = Visibility.Visible;
            BTN_Withdraw.Visibility = Visibility.Visible;
        }

        private void RefreshAccountsList()
        {
            accounts = new SQL().GetAccountsFromSql(customerNumber);
        }

        private void FillUpAccountsList()
        {
            for (int i = 0; i < accounts.Count; i++)
            {
                DRP_Accounts.Items.Add($"{accounts[i].accountName}");
            }
        }

        

        private void UpdateAccounts(int withdrawValue, string accountId)
        {
            SQL Sql = new SQL();
            try
            {
                Sql.conn.Open();
                MySqlCommand cmd = Sql.cmd;

                cmd.Connection = Sql.conn;
                cmd.CommandText = "UPDATE accounts SET accountBalance = accountBalance - @withdrawValue WHERE customerNumber = @customerNumber AND accountId = @accountId";

                cmd.Parameters.AddWithValue("@withdrawValue", withdrawValue);
                cmd.Parameters.AddWithValue("@customerNumber", customerNumber);
                cmd.Parameters.AddWithValue("@accountId", accountId);

                cmd.Prepare();
                cmd.ExecuteNonQuery();

                WithdrawInsertTransaction(withdrawValue, accountId);
                Sql.conn.Close();
            }
            catch (Exception)
            {
                Sql.conn.Close();
            }

        }

        private void WithdrawInsertTransaction(int withdrawValue, string accountId)
        {
            SQL Sql = new SQL();
            try
            {
                Sql.conn.Open();

                Sql.cmd.Connection = Sql.conn;
                Sql.cmd.CommandText = "INSERT INTO `transactions`(`accountId`, `transactionType`, `processedAmount`, `transactionDate`) VALUES ( @accountId ,\"WITHDRAW\" , @withdrawValue , NOW())";

                Sql.cmd.Parameters.AddWithValue("@accountId", accountId);
                Sql.cmd.Parameters.AddWithValue("@withdrawValue", withdrawValue);

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
