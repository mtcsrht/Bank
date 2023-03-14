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
    }
}
