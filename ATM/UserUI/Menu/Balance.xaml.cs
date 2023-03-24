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
            accounts = new SQL().GetAccountsFromSql(customerNumber);
            FillUpAccountsList();

        }


        private void FillUpAccountsList()
        {
            for (int i = 0; i < accounts.Count; i++)
            {
                DRP_Accounts.Items.Add($"{accounts[i].accountName}");
            }
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
