using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakTest
{
    class KoszykHandler
    {
        //Obsluga koszyka

        public void dodajDoKoszykCena(int index, Controler controler)
        {
            controler.Asortyment.TryGetValue(index, out Przedmiot linia);
            controler.KoszykSuma = controler.KoszykSuma + linia.cena;
            controler.koszykPodatek = controler.koszykPodatek + (linia.cena * linia.VAT / 100);
        }

        public void usunZKoszykCena(int index, Controler controler)
        {
            controler.Asortyment.TryGetValue(index, out Przedmiot linia);
            controler.KoszykSuma = controler.KoszykSuma - linia.cena;
            controler.koszykPodatek = controler.koszykPodatek - (linia.cena * linia.VAT / 100);
        }

        public void addItemsToKoszyk(List<int> zaznaczone, Controler controler)
        {
            foreach (int nowa in zaznaczone)
            {
                controler.KoszykList.Add(nowa);
                dodajDoKoszykCena(nowa, controler);
            }
        }

        public void removeItemsFromKoszyk(List<int> zaznaczone, Controler controler)
        {
            foreach (int i in zaznaczone)
            {
                controler.KoszykList.Remove(i);
                usunZKoszykCena(i, controler);
            }
        }
    }
}
