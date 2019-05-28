using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.TextFormatting;

namespace FakTest
{
    public struct Przedmiot
    {
        public string nazwa;
        public int cena;
        public int VAT;

        public Przedmiot(string _nazwa, int _cena, int _vat)
        {
            nazwa = _nazwa;
            cena = _cena;
            VAT = _vat;
        }
    }
    
    public struct Sprzedaz
    {
        string NIP;
        int nr_dok;
        int[] tabIDs;
        string data;
    }

    public struct Klient
    {
        public string nazwa;
        public string NIP;
        public string telefon;
        public string kod;
        public string adres;

        public Klient(string _nazwa, string _nip, string _telefon, string _kod, string _adres)
        {
            nazwa = _nazwa;
            NIP = _nip;
            telefon = _telefon;
            kod = _kod;
            adres = _adres;
        }

    }

 //-----------------------------------------------------------------------------------------------------
 //Controler
    public class Controler
    {

        public Dictionary<int, Przedmiot> Asortyment = new Dictionary<int, Przedmiot>();
        public Dictionary<int, Sprzedaz> Transkacje;
        public Dictionary<int, Klient> Klienci = new Dictionary<int, Klient>();

        public List<int> KoszykList = new List<int>();

//-----------------------------------------------------------------------------------------------------
//Nazwy plikow zapisu
        string asortymentFilePath = @".\Asortyment.txt";
        string transakcjeFilePath = @".\Transkacje.txt";
        string klienciFilePath = @".\Klienci.txt";

//-----------------------------------------------------------------------------------------------------
//Zapis asortymentu
        public Przedmiot parsujStrPrzedmiot(string linia)
        {
            int cena = 0, podatek = 0;
            string nazwa, cena_s, podatek_s;
            //MessageBox.Show(linia);

            nazwa = linia.Substring(0, linia.IndexOf(','));
            linia = linia.Substring((linia.IndexOf(',') + 1), linia.Length - (linia.IndexOf(',') + 1));

            cena_s = linia.Substring(0, linia.IndexOf(','));
            linia = linia.Substring((linia.IndexOf(',') + 1), linia.Length - (linia.IndexOf(',') + 1));

            podatek_s = linia.Substring(0, (linia.Length - 1));
            //MessageBox.Show(cena_s +" "+ podatek_s);

            try
            {
                cena = Int32.Parse(cena_s);
                podatek = Int32.Parse(podatek_s);
            }
            catch (FormatException)
            {
                string msg;
                msg = "Blad parsowania string to int";
                MessageBox.Show(msg);
            }
            Przedmiot temp = new Przedmiot(nazwa, cena, podatek);

            return temp;
        }

        public void saveAsortyment()
        {
            StreamWriter sw = File.CreateText(asortymentFilePath);

            sw.WriteLine(Asortyment.Count +";");

            for (int i = 0; i < Asortyment.Count; i++)
            {
                Asortyment.TryGetValue(i, out Przedmiot linia);
                
                sw.Write(linia.nazwa + ",");
                sw.Write(linia.cena + ",");
                sw.Write(linia.VAT + ";");
                sw.Write(System.Environment.NewLine);
            }

            sw.Close();
            
        }

        public void loadAsortyment()
        {
            if(Asortyment.Count == 0)
            {
                string msg;
                msg = "Wczytyatanie";
                //MessageBox.Show(msg);
                StreamReader se = File.OpenText(asortymentFilePath);
                int l = 0;
                string length = se.ReadLine();
                length = length.Substring(0, length.IndexOf(';'));
                Int32.TryParse(length, out l);

                string linia; 

                for (int i = 0; i < l; i++)
                {
                    linia = se.ReadLine();
                    Asortyment.Add(Asortyment.Count, parsujStrPrzedmiot(linia));
                }

                se.Close();
                MessageBox.Show("Wczytano: " + l + " rekordow.");
            }
            else
            {
                string msg;
                msg = "Juz wczytano dane.";
                MessageBox.Show(msg);
            }
        }

//-----------------------------------------------------------------------------------------------------
//Zapis klientow
        public Klient parsujStrKlient(string linia)
        {
            string nazwa, nip, telefon, kod, adres;
            //MessageBox.Show(linia);

            nazwa = linia.Substring(0, linia.IndexOf(','));
            linia = linia.Substring((linia.IndexOf(',') + 1), linia.Length - (linia.IndexOf(',') + 1));

            nip = linia.Substring(0, linia.IndexOf(','));
            linia = linia.Substring((linia.IndexOf(',') + 1), linia.Length - (linia.IndexOf(',') + 1));

            telefon = linia.Substring(0, linia.IndexOf(','));
            linia = linia.Substring((linia.IndexOf(',') + 1), linia.Length - (linia.IndexOf(',') + 1));

            kod = linia.Substring(0, linia.IndexOf(','));
            linia = linia.Substring((linia.IndexOf(',') + 1), linia.Length - (linia.IndexOf(',') + 1));

            adres = linia.Substring(0, (linia.Length - 1));
            

            Klient temp = new Klient(nazwa, nip, telefon, kod, adres);

            return temp;
        }

        public void saveKlienci()
        {
            StreamWriter sw = File.CreateText(klienciFilePath);

            sw.WriteLine(Klienci.Count + ";");

            for (int i = 0; i < Klienci.Count; i++)
            {
                Klienci.TryGetValue(i, out Klient linia);

                sw.Write(linia.nazwa + ",");
                sw.Write(linia.NIP + ",");
                sw.Write(linia.telefon + ",");
                sw.Write(linia.kod + ",");
                sw.Write(linia.adres + ";");
                sw.Write(System.Environment.NewLine);
            }

            sw.Close();

        }

        public void loadKlienci()
        {
            if (Klienci.Count == 0)
            {
                string msg;
                msg = "Wczytywanie";
                //MessageBox.Show(msg);
                StreamReader se = File.OpenText(klienciFilePath);
                int l = 0;
                string length = se.ReadLine();
                length = length.Substring(0, length.IndexOf(';'));
                Int32.TryParse(length, out l);

                string linia;

                for (int i = 0; i < l; i++)
                {
                    linia = se.ReadLine();
                    Klienci.Add(Klienci.Count, parsujStrKlient(linia));
                }

                se.Close();
                MessageBox.Show("Wczytano: " + l + " rekordow.");
            }
            else
            {
                string msg;
                msg = "Juz wczytano dane.";
                MessageBox.Show(msg);
            }
        }
//-----------------------------------------------------------------------------------------------------
//Obsluga koszyka

        public void addItemsToKoszyk(List<int> zaznaczone)
        {
            foreach(int nowa in zaznaczone)
            {
                KoszykList.Add(nowa);
            }
        }

        public void removeItemsFromKoszyk(List<int> zaznaczone)
        {
            foreach (int i in zaznaczone)
            {
                KoszykList.Remove(i);
            }
        }
//-----------------------------------------------------------------------------------------------------
    }
}
