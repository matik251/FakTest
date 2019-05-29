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
        public double cena;
        public int VAT;

        public Przedmiot(string _nazwa, double _cena, int _vat)
        {
            nazwa = _nazwa;
            cena = _cena;
            VAT = _vat;
        }
    }
    
    public struct Sprzedaz
    {
        int idFirmy;
        string nipFirmy;
        string adresFirmy;
        string nr_dok;
        int[] tabIDs;
        string data;
        int kwotaNetto;
        int podatekVat;

        public Sprzedaz(int _idFirmy, string _nipFirmy, string _adresFirmy, string _nr_dok, int[] _tabIDs, string _data, int _kwotaNetto, int _podatekVat)
        {
            idFirmy = _idFirmy;
            nipFirmy = _nipFirmy;
            adresFirmy = _adresFirmy;
            nr_dok = _nr_dok;
            tabIDs = _tabIDs;
            data = _data;
            kwotaNetto = _kwotaNetto;
            podatekVat = _podatekVat;
        }

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
        string asortymentFilePath = @".\Asortyment.csv";
        string transakcjeFilePath = @".\Transkacje.csv";
        string klienciFilePath = @".\Klienci.csv";

//-----------------------------------------------------------------------------------------------------
//Zapis asortymentu
        public Przedmiot parsujStrPrzedmiot(string linia)
        {
            double cena = 0;
            int podatek = 0;
            string nazwa, cena_s, podatek_s;
            //MessageBox.Show(linia);

            nazwa = linia.Substring(0, linia.IndexOf(','));
            linia = linia.Substring((linia.IndexOf(',') + 1), linia.Length - (linia.IndexOf(',') + 1));

            cena_s = linia.Substring(0, linia.IndexOf(','));
            linia = linia.Substring((linia.IndexOf(',') + 1), linia.Length - (linia.IndexOf(',') + 1));

            podatek_s = linia.Substring(0, (linia.Length - 1));
            //MessageBox.Show(cena_s +" "+ podatek_s);

            //cena_s.Replace(@".", @",");//TODO
            char[] abc = cena_s.ToCharArray();
            if (cena_s.Contains('.'))
            { 
                //abc[cena_s.IndexOf('.')] = ',';
                cena_s = cena_s.Substring(0, cena_s.IndexOf('.')) + "," + cena_s.Substring((cena_s.IndexOf('.')+1), (cena_s.Length-2));
            }
            try
            {
                cena = Convert.ToDouble(cena_s);
                podatek = Int32.Parse(podatek_s);
            }
            catch (FormatException)
            {
                string msg;
                msg = "Blad parsowania stringow ";
                MessageBox.Show(msg);
            }
            Przedmiot temp = new Przedmiot(nazwa, cena, podatek);

            return temp;
        }

        public void saveAsortyment()
        {
            string temp;
            StreamWriter sw = File.CreateText(asortymentFilePath);

            sw.WriteLine(Asortyment.Count +";");

            for (int i = 0; i < Asortyment.Count; i++)
            {
                Asortyment.TryGetValue(i, out Przedmiot linia);
                
                sw.Write(linia.nazwa + ",");
                //sw.Write(linia.cena + ",");
                temp = linia.cena.ToString();
                temp.Replace(',','.');//TODo
                sw.Write(temp + ",");
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
                msg = "Wczytywanie";
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
