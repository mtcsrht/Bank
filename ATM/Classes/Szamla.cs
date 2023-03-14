using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM.Classes
{
    internal class Szamla
    {
        public string szamlaId;
        public string szamlaNev;
        public int osszeg;
        public Szamla(string szamlaId, string szamlaNev, int osszeg)
        {
            this.szamlaId = szamlaId;
            this.szamlaNev = szamlaNev;
            this.osszeg = osszeg;
        }
    }
}
