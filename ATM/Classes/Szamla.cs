using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM.Classes
{
    internal class Szamla
    {
        public string szamlaNev;
        public int osszeg;
        public Szamla(string szamlaNevBe, int osszegBe)
        {
            szamlaNev = szamlaNevBe;
            osszeg = osszegBe;
        }
    }
}
