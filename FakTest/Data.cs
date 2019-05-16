using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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



        string testFilePath = @".\Asortyment.txt";

        public void writeMainFile()
        {
            StreamWriter sw = File.CreateText(testFilePath);

            Asortyment.TryGetValue(0, out Przedmiot linia);
    
            sw.Write(linia.nazwa + "\n");
            sw.Write(linia.cena + "\n");
            sw.Write(linia.VAT + "\n");

            sw.Close();
        }
    }
}
