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
        public decimal cena;
        public int VAT;

        public Przedmiot(string _nazwa, decimal _cena, int _vat)
        {
            nazwa = _nazwa;
            cena = _cena;
            VAT = _vat;
        }
    }
    
    public struct Sprzedaz
    {
        public int idFirmy;
        public string nipFirmy;
        public string adresFirmy;
        public string nr_dok;
        public int[] tabIDs;
        public string data;
        public decimal kwotaNetto;
        public decimal podatekVat;

        public Sprzedaz(int _idFirmy, string _nipFirmy, string _adresFirmy, string _nr_dok, int[] _tabIDs, string _data, decimal _kwotaNetto, decimal _podatekVat)
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

    public class dataGridProdukt
    {
        public int id { get; set; }
        public string nazwa { get; set; }
        public string typ { get; set; }
        public string netto { get; set; }
        public string stawka { get; set; }
        public string podatek { get; set; }
        public string brutto { get; set; }
    }

    public class dataGridKlient
    {
        public int id { get; set; }
        public string nazwa { get; set; }
        public string nip { get; set; }
        public string telefon { get; set; }
        public string kod { get; set; }
        public string adres { get; set; }
    }


    //-----------------------------------------------------------------------------------------------------
    //Controler
    public class Controler
    {

        public Dictionary<int, Przedmiot> Asortyment = new Dictionary<int, Przedmiot>();
        public Dictionary<int, Sprzedaz> Transkacje = new Dictionary<int, Sprzedaz>();
        public Dictionary<int, Klient> Klienci = new Dictionary<int, Klient>();

        public List<int> KoszykList = new List<int>();
        public decimal KoszykSuma = 0;
        public bool transakcjaWToku = false;
        public int KlientID = -1;
//-----------------------------------------------------------------------------------------------------
//Nazwy plikow zapisu
        string asortymentFilePath = @".\Asortyment.csv";
        string transakcjeFilePath = @".\Transkacje.csv";
        string klienciFilePath = @".\Klienci.csv";

//-----------------------------------------------------------------------------------------------------
//Zapis asortymentu
        public string zamiana(string liczba, char stary, char nowy)
        {
            if (liczba.Contains(stary))
            {
                int len = liczba.Length - (liczba.IndexOf(stary) + 1);
                string calkowita, ulamkowa;
                calkowita = liczba.Substring(0, liczba.IndexOf(stary));
                ulamkowa = liczba.Substring(liczba.IndexOf(stary) + 1, len);
                liczba = calkowita + nowy + ulamkowa;
                //cena_s = cena_s.Substring(0, cena_s.IndexOf('.')) + "," + cena_s.Substring((cena_s.IndexOf('.') + 1), (cena_s.Length - 2));
                //cena_s = zamiana(cena_s,'.',',');
            }
            return liczba;
        }

        public Przedmiot parsujStrPrzedmiot(string linia)
        {
            decimal cena = 0;
            int podatek = 0;
            string nazwa, cena_s, podatek_s;
            //MessageBox.Show(linia);

            nazwa = linia.Substring(0, linia.IndexOf(','));
            linia = linia.Substring((linia.IndexOf(',') + 1), linia.Length - (linia.IndexOf(',') + 1));

            cena_s = linia.Substring(0, linia.IndexOf(','));
            linia = linia.Substring((linia.IndexOf(',') + 1), linia.Length - (linia.IndexOf(',') + 1));

            podatek_s = linia.Substring(0, (linia.Length - 1));
            
            cena_s = zamiana(cena_s, '.', ',');
            try
            {
                cena = Convert.ToDecimal(cena_s);
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
                temp = zamiana(temp, ',', '.');
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
//Zapis transakcji

        public void saveTransakcje()
        {
            StreamWriter sw = File.CreateText(transakcjeFilePath);

            sw.WriteLine(Transkacje.Count + ";");

            for (int i = 0; i < Transkacje.Count; i++)
            {
                Transkacje.TryGetValue(i, out Sprzedaz linia);

                sw.Write(linia.idFirmy + ",");
                sw.Write(linia.nipFirmy + ",");
                sw.Write(linia.adresFirmy + ",");
                sw.Write(linia.nr_dok + ",");
                sw.Write(linia.tabIDs.Length + "{");
                //int[] temp = linia.tabIDs.ToArray();
                int j = 0;
                for (j = 0 ; j < linia.tabIDs.Length-1; j++)
                {
                    sw.Write(linia.tabIDs[j]+",");
                }
                sw.Write(linia.tabIDs[j+1] + "}");
                sw.Write(linia.data + ",");
                sw.Write(linia.kwotaNetto + ",");
                sw.Write(linia.podatekVat + ";");
                sw.Write(System.Environment.NewLine);
            }

            sw.Close();

        }

//-----------------------------------------------------------------------------------------------------
//Obsluga koszyka
        public void dodajDoKoszykCena(int index)
        {
            Asortyment.TryGetValue(index, out Przedmiot linia);
            KoszykSuma = KoszykSuma + linia.cena;
        }

        public void usunZKoszykCena(int index)
        {
            Asortyment.TryGetValue(index, out Przedmiot linia);
            KoszykSuma = KoszykSuma - linia.cena;
        }

        public void addItemsToKoszyk(List<int> zaznaczone)
        {
            foreach(int nowa in zaznaczone)
            {
                KoszykList.Add(nowa);
                dodajDoKoszykCena(nowa);
            }
        }

        public void removeItemsFromKoszyk(List<int> zaznaczone)
        {
            foreach (int i in zaznaczone)
            {
                KoszykList.Remove(i);
                usunZKoszykCena(i);
            }
        }
//-----------------------------------------------------------------------------------------------------
//Obsluga transakcji
       public void utworzTransakcje()
        {
            Klienci.TryGetValue(KlientID, out Klient linia);
            Sprzedaz nowyRekordSprzedazy = new Sprzedaz(
                KlientID,
                linia.NIP,
                linia.adres,
                "0/0/0",
                KoszykList.ToArray(),
                "03:06:2019",
                KoszykSuma,
                KoszykSuma
                );

            int counter = Transkacje.Count;

            Transkacje.Add(counter, nowyRekordSprzedazy);

            KoszykSuma = 0;
            KlientID = 0;
            KoszykList.Clear();
        }

    }
}
