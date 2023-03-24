using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM.Classes
{
    internal class SQL
    {
        private string connectionString;
        public MySqlConnection conn;
        public MySqlCommand cmd;
        public SQL()
        {
            connectionString = "server=localhost;user=root;database=bank;password=";
            conn = new MySqlConnection(connectionString);
            cmd = new MySqlCommand();

        }

        public List<Account> GetAccountsFromSql(string customerNumber)
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

    }
}
