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
    /// Interaction logic for Balance.xaml
    /// </summary>
    public partial class Balance : Page
    {
        private string customerNumber;
        private List<Account> accounts;


        public Balance(string customerNumber)
        {
            InitializeComponent();
            this.customerNumber = customerNumber;
            accounts = GetAccountsFromSql();
            FillUpAccountsList();

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
            List<Account> tempAccounts= new List<Account>();
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


        private void DRP_Accounts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = DRP_Accounts.SelectedIndex;

            LBL_Balance_Header.Visibility = Visibility.Visible;
            LBL_Balance_Value.Visibility = Visibility.Visible;
            LBL_Balance_Value.Content = accounts[index].accountBalance + " HUF";
        }
    }
}
