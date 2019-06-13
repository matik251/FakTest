using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.TextFormatting;
using iText.IO.Image;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Layout;
using iText.Layout.Properties;

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
        FileHandler fh = new FileHandler();
        PdfHandler pdfh = new PdfHandler();

        public Dictionary<int, Przedmiot> Asortyment = new Dictionary<int, Przedmiot>();
        public Dictionary<string, int> asortymentIndeks = new Dictionary<string, int>();
        public Dictionary<int, Sprzedaz> Transkacje = new Dictionary<int, Sprzedaz>();
        public Dictionary<int, Klient> Klienci = new Dictionary<int, Klient>();

        public List<int> KoszykList = new List<int>();
        public decimal KoszykSuma = 0;
        public decimal koszykPodatek = 0;
        public bool transakcjaWToku = false;
        public int KlientID = -1;
        
        //-----------------------------------------------------------------------------------------------------

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
            string pdf = pdfh.createFaktura(nowyRekordSprzedazy, this);

            wyczyscKoszyk();

            //Process myProcess = new Process();
            //myProcess.StartInfo.FileName = "acroRd32.exe"; //not the full application path
            pdf = AppDomain.CurrentDomain.BaseDirectory + pdf;
            //pdf = "/A "  + pdf;
            //myProcess.StartInfo.Arguments = pdf;
            //myProcess.Start();
            System.Diagnostics.Process.Start(pdf);

        }

        public void wyczyscKoszyk()
        {
            KoszykSuma = 0;
            koszykPodatek = 0;
            KlientID = 0;
            KoszykList.Clear();
        }

        //-----------------------------------------------------------------------------------------------------
        //Obsluga ladowania i zapisu wielowatkowo

        public void loadData()
        {
            Thread asortymenThread = new Thread(()=>fh.loadAsortyment(this));
            Thread transakcjeThread = new Thread(()=>fh.loadTransakcje(this));
            Thread klienciThread = new Thread(()=>fh.loadKlienci(this));

            asortymenThread.Start();
            transakcjeThread.Start();
            klienciThread.Start();
            asortymenThread.Join();
            transakcjeThread.Join();
            klienciThread.Join();

        }

        public void saveData()
        {
            Thread asortymenThread = new Thread(()=>fh.saveAsortyment(this));
            Thread transakcjeThread = new Thread(()=>fh.saveTransakcje(this));
            Thread klienciThread = new Thread(()=>fh.saveKlienci(this));
            
            asortymenThread.Start();
            transakcjeThread.Start();
            klienciThread.Start();
            asortymenThread.Join();
            transakcjeThread.Join();
            klienciThread.Join();
            
            //createPDF("2019/06/01");

        }

        //-----------------------------------------------------------------------------------------------------
    }

}
