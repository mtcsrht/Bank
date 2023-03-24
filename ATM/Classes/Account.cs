using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM.Classes
{
    internal class Account
    {
        public string accountId;
        public string accountName;
        public int accountBalance;
        public Account(string accountId, string accountName, int accountBalance)
        {
            this.accountId = accountId;
            this.accountName = accountName;
            this.accountBalance = accountBalance;
        }
    }
}
