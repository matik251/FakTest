using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FakTest
{
    class Regexy
    {
        //Regex input

        public bool sprawdzenieRegex(string linia)
        {
            Regex txRegex;
            txRegex = new Regex(@"^[^,;]+$");
            return txRegex.IsMatch(linia);
        }

        public bool kodPocztowyRegex(string linia)
        {
            Regex kodPocztowyWzor;
            kodPocztowyWzor = new Regex(@"[0-9]{2}-[0-9]{3}$");

            return kodPocztowyWzor.IsMatch(linia);
        }
        public bool telefonRegex(string linia)
        {
            Regex telefonWzor;
            telefonWzor = new Regex(@"(((00)|[+])(([0-9]){10,12}))$|(([0-9]){9})$");

            return telefonWzor.IsMatch(linia);
        }
        public bool nipRegex(string linia)
        {
            Regex nipRegex;
            nipRegex = new Regex(@"\d{9}$|\d{10}$|\d{14}$");

            return nipRegex.IsMatch(linia);
        }

        public bool cenaRegex(string linia)
        {
            Regex cenaWzor;
            cenaWzor = new Regex(@"[0-9]+\.[0-9]{2}$");
            return cenaWzor.IsMatch(linia);
        }
        public bool vatRegex(string linia)
        {
            Regex vat;
            vat = new Regex(@"[0-9]{1}$|[0-9]{2}$");
            return vat.IsMatch(linia);
        }
    }
}
