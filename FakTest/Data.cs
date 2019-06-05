using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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

    public class dataGridTransakcje
    {
        public int id { get; set; }
        public int id_firmy { get; set; }
        public string nip { get; set; }
        public string adres { get; set; }
        public string nr_fak { get; set; }
        public string id_prod { get; set; }
        public string data { get; set; }
        public string netto { get; set; }
        public string vat { get; set; }
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
        public decimal koszykPodatek = 0;
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
                cena = 0;
                podatek = 0;
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

        public int[] parsujStrIntTab(string linia)
        {
            string count_s, temp;
            int count = 0;
            count_s = linia.Substring(0, linia.IndexOf('{'));
            linia = linia.Substring((linia.IndexOf('{') + 1), linia.Length - (linia.IndexOf('{') + 1));
            count = Int32.Parse(count_s);
            int[] tabInts = new int[count];

            for (int i = 0; i < count; i++)
            {
                temp = linia.Substring(0, linia.IndexOf(','));
                linia = linia.Substring((linia.IndexOf(',') + 1), linia.Length - (linia.IndexOf(',') + 1));
                tabInts[i] = Int32.Parse(temp);
            }

            return tabInts;
        }

        public string parsujIntTabStr(int[] tab)
        {
            string temp = "";
            for(int i=0; i < tab.Length; i++)
            {
                temp = tab[i].ToString() + ',' + temp;
            }
            return temp;
        }

        public Sprzedaz parsujstrSprzedaz(string linia)
        {
            string id_s, nip, adres, nr, tab_s, data, netto_s, vat_s;
            int[] tabOfIDs;
            int id;
            decimal netto, vat;
            //MessageBox.Show(linia);

            id_s = linia.Substring(0, linia.IndexOf(','));
            linia = linia.Substring((linia.IndexOf(',') + 1), linia.Length - (linia.IndexOf(',') + 1));
            Int32.TryParse(id_s, out id);

            nip = linia.Substring(0, linia.IndexOf(','));
            linia = linia.Substring((linia.IndexOf(',') + 1), linia.Length - (linia.IndexOf(',') + 1));

            adres = linia.Substring(0, linia.IndexOf(','));
            linia = linia.Substring((linia.IndexOf(',') + 1), linia.Length - (linia.IndexOf(',') + 1));

            nr = linia.Substring(0, linia.IndexOf(','));
            linia = linia.Substring((linia.IndexOf(',') + 1), linia.Length - (linia.IndexOf(',') + 1));

            tab_s = linia.Substring(0, linia.IndexOf('}'));
            linia = linia.Substring((linia.IndexOf('}') + 1), linia.Length - (linia.IndexOf('}') + 1));

            tabOfIDs = parsujStrIntTab(tab_s);

            data = linia.Substring(0, linia.IndexOf(','));
            linia = linia.Substring((linia.IndexOf(',') + 1), linia.Length - (linia.IndexOf(',') + 1));

            netto_s = linia.Substring(0, linia.IndexOf(','));
            linia = linia.Substring((linia.IndexOf(',') + 1), linia.Length - (linia.IndexOf(',') + 1));

            vat_s = linia.Substring(0, (linia.Length - 1));

            netto_s = zamiana(netto_s, '.', ',');
            vat_s = zamiana(vat_s, '.', ',');
            try
            {
                netto = Convert.ToDecimal(netto_s);
                vat = Convert.ToDecimal(vat_s);
            }
            catch (FormatException)
            {
                string msg;
                msg = "Blad parsowania stringow ";
                MessageBox.Show(msg);
                netto = 0;
                vat = 0;
            }

            Sprzedaz temp = new Sprzedaz(id, nip, adres, nr, tabOfIDs, data, netto, vat);
            
            return temp;
        }

        public void saveTransakcje()
        {
            StreamWriter sw = File.CreateText(transakcjeFilePath);
            String temp;

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
                for (j = 0 ; j < (linia.tabIDs.Length); j++)
                {
                    sw.Write(linia.tabIDs[j]+",");
                }
                sw.Write( "}");
                sw.Write(linia.data + ",");

                temp = linia.kwotaNetto.ToString();
                temp = zamiana(temp, ',', '.');
                sw.Write(temp + ",");
                temp = linia.podatekVat.ToString();
                temp = zamiana(temp, ',', '.');
                sw.Write(temp + ";");
                sw.Write(System.Environment.NewLine);
            }

            sw.Close();

        }

        public void loadTransakcje()
        {
            if (Transkacje.Count == 0)
            {
                string msg;
                msg = "Wczytywanie";
                //MessageBox.Show(msg);
                StreamReader se = File.OpenText(transakcjeFilePath);
                int l = 0;
                string length = se.ReadLine();
                length = length.Substring(0, length.IndexOf(';'));
                Int32.TryParse(length, out l);

                string linia;

                for (int i = 0; i < l; i++)
                {
                    linia = se.ReadLine();
                    Transkacje.Add(Transkacje.Count, parsujstrSprzedaz(linia));
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

        public void dodajDoKoszykCena(int index)
        {
            Asortyment.TryGetValue(index, out Przedmiot linia);
            KoszykSuma = KoszykSuma + linia.cena;
            koszykPodatek = koszykPodatek + (linia.cena * linia.VAT / 100);
        }

        public void usunZKoszykCena(int index)
        {
            Asortyment.TryGetValue(index, out Przedmiot linia);
            KoszykSuma = KoszykSuma - linia.cena;
            koszykPodatek = koszykPodatek - (linia.cena * linia.VAT /100);
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
            int counter = Transkacje.Count;
            DateTime data = DateTime.UtcNow.ToLocalTime();
            string nr_fak = "0/0/0";
            nr_fak = data.ToString("yyyy") + '/' + data.ToString("MM") + '/' + counter.ToString();
            string czas = data.ToString("dd-MM-yyyy HH:mm:ss");
            Klienci.TryGetValue(KlientID, out Klient linia);
            Sprzedaz nowyRekordSprzedazy = new Sprzedaz(
                KlientID,
                linia.NIP,
                linia.adres,
                //"0/0/0",
                nr_fak,
                KoszykList.ToArray(),
                //"03:06:2019",
                czas,
                KoszykSuma,
                koszykPodatek
                );


            Transkacje.Add(counter, nowyRekordSprzedazy);

            KoszykSuma = 0;
            koszykPodatek = 0;
            KlientID = 0;
            KoszykList.Clear();
        }

//-----------------------------------------------------------------------------------------------------
//Obsluga ladowania i zapisu

        public void loadData()
        {
            Thread asortymenThread = new Thread(loadAsortyment);
            Thread transakcjeThread = new Thread(loadTransakcje);
            Thread klienciThread = new Thread(loadKlienci);

            asortymenThread.Start();
            transakcjeThread.Start();
            klienciThread.Start();
            asortymenThread.Join();
            transakcjeThread.Join();
            klienciThread.Join();

        }

        public void saveData()
        {
            Thread asortymenThread = new Thread(saveAsortyment);
            Thread transakcjeThread = new Thread(saveTransakcje);
            Thread klienciThread = new Thread(saveKlienci);

            asortymenThread.Start();
            transakcjeThread.Start();
            klienciThread.Start();
            asortymenThread.Join();
            transakcjeThread.Join();
            klienciThread.Join();

        }

    }

}
