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
    /// Interaction logic for Deposit.xaml
    /// </summary>
    public partial class Deposit : Page
    {
        private string accountNumber;
        private List<Szamla> szamla;
        public Deposit(string accountNumber)
        {
            InitializeComponent();
            this.accountNumber = accountNumber;
            szamla = GetSzamlakFromSql();
            FillUpSzamlaList();
        }
        private void RefreshSzamlaList()
        {
            szamla = GetSzamlakFromSql();
        }
        private void FillUpSzamlaList()
        {
            for (int i = 0; i < szamla.Count; i++)
            {
                DRP_Szamlak.Items.Add($"{szamla[i].szamlaNev}");
            }
        }
        private List<Szamla> GetSzamlakFromSql()
        {
            SQL Sql = new SQL();
            MySqlConnection conn = Sql.conn;
            List<Szamla> szamlak = new List<Szamla>();
            try
            {
                conn.Open();
                MySqlCommand cmd = Sql.cmd;
                cmd.Connection = conn;
                cmd.CommandText = "SELECT szamlaId, szamlaNev, szamlaOsszeg FROM szamla WHERE accountNumber = @accountNumber";
                cmd.Parameters.AddWithValue("@accountNumber", accountNumber);
                cmd.Prepare();
                var result = cmd.ExecuteReader();
                while (result.Read())
                {
                    szamlak.Add(new Szamla(result.GetString(0), result.GetString(1), result.GetInt32(2)));
                }

            }
            catch (Exception)
            {
                conn.Close();
            }

            return szamlak;
        }

        private void DRP_Szamlak_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = DRP_Szamlak.SelectedIndex;

            LBL_Osszeg.Visibility = Visibility.Visible;
            LBL_Osszeg_Value.Visibility = Visibility.Visible;
            LBL_Osszeg_Value.Content = szamla[index].osszeg + " HUF";
            TXT_Deposit_Value.Visibility = Visibility.Visible;
            BTN_Deposit.Visibility = Visibility.Visible;
        }

        private void BTN_Deposit_Click(object sender, RoutedEventArgs e)
        {
            int value;
            int index = DRP_Szamlak.SelectedIndex;
            if (int.TryParse(TXT_Deposit_Value.Text, out value))
            {

                UpdateSzamla(value, szamla[index].szamlaId);
                RefreshSzamlaList();
                LBL_Osszeg_Value.Content = szamla[index].osszeg + " HUF";
                TXT_Deposit_Value.Text = string.Empty;
                MessageBox.Show("Deposit was successful!", "Successful Deposit",MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("The input has to be numbers!", "Error", MessageBoxButton.OKCancel, MessageBoxImage.Warning);

            }
        }
        private void UpdateSzamla(int osszeg, string szamlaId)
        {
            SQL Sql = new SQL();
            MySqlConnection conn = Sql.conn;
            try
            {
                conn.Open();
                MySqlCommand cmd = Sql.cmd;
                cmd.Connection = Sql.conn;
                cmd.CommandText = "UPDATE `szamla` SET `szamlaOsszeg`= szamlaOsszeg + @osszeg WHERE accountNumber = @accountNumber AND szamlaId = @szamlaId";
                cmd.Parameters.AddWithValue("@osszeg", osszeg);
                cmd.Parameters.AddWithValue("@accountNumber", accountNumber);
                cmd.Parameters.AddWithValue("@szamlaId", szamlaId);
                cmd.Prepare();
                cmd.ExecuteNonQuery();
                DepositInsertTranzakcio(osszeg, szamlaId);
                conn.Close();
                return true;
            }
            catch (Exception)
            {
                conn.Close();
                return false;
            }
            
        }
        private void DepositInsertTranzakcio(int osszeg, string szamlaId)
        {
            SQL Sql = new SQL();
            MySqlConnection conn = Sql.conn;
            try
            {
                conn.Open();
                MySqlCommand cmd = Sql.cmd;
                cmd.Connection = Sql.conn;

                cmd.CommandText = "INSERT INTO `tranzakciok`(`szamlaId`, `tipus`, `feldolgozottOsszeg`, `tranzakcioDate`) VALUES ( @szamlaId ,\"DEPOSIT\" , @osszeg , NOW())";
                cmd.Parameters.AddWithValue("@szamlaId", szamlaId);
                cmd.Parameters.AddWithValue("@osszeg", osszeg);
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
