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
        string NIP;
        string nazwa;
        string telefon;
    }

    public class Controler
    {

        public Dictionary<int, Przedmiot> Asortyment = new Dictionary<int, Przedmiot>();
        public Dictionary<int, Sprzedaz> Transkacje;
        public Dictionary<int, Klient> Klienci;



        string asortymentFilePath = @".\Asortyment.txt";
        string transakcjeFilePath = @".\Transkacje.txt";
        string klienciFilePath = @".\Klienci.txt";

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
    }
}
