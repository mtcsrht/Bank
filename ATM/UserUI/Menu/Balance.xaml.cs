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
        private string accountNumber;
        private List<Szamla> szamla;
        public Balance(string accountNumber)
        {
            InitializeComponent();
            this.accountNumber = accountNumber;
            szamla = GetSzamlakFromSql();
            FillUpSzamlaList();

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
                cmd.CommandText = "SELECT szamlaNev, szamlaOsszeg FROM szamla WHERE accountNumber = @accountNumber";
                cmd.Parameters.AddWithValue("@accountNumber", accountNumber);
                cmd.Prepare();
                var result = cmd.ExecuteReader();
                while (result.Read())
                {
                    szamlak.Add(new Szamla(result.GetString(0), result.GetInt32(1)));
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
        }
    }
}
