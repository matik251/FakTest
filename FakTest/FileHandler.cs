using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.IO;
using System.Net.Http;
using System.Text;

using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

using iText.IO.Image;
using iText.Layout.Properties;

using FakTest.Properties;
using System.Windows;
using System.Threading;

namespace FakTest
{
    public class FileHandler
    {

        string asortymentFilePath = @".\Baza\Asortyment.csv";
        string transakcjeFilePath = @".\Baza\Transkacje.csv";
        string klienciFilePath = @".\Baza\Klienci.csv";

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

        public void saveAsortyment(Controler controler)
        {
            string temp;
            StreamWriter sw = File.CreateText(asortymentFilePath);

            sw.WriteLine(controler.Asortyment.Count + ";");
            try
            {
                for (int i = 0; i < controler.Asortyment.Count; i++)
                {
                    controler.Asortyment.TryGetValue(i, out Przedmiot linia);

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
            catch (IOException e)
            {
                MessageBox.Show("Blad zapisu asortymentu " + e.ToString());
                sw.Close();
            }
        }

        public void loadAsortyment(Controler controler)
        {
            try
            {
                if (controler.Asortyment.Count == 0)
                {
                    //string msg;
                    //msg = "Wczytywanie";
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
                        Przedmiot temp = parsujStrPrzedmiot(linia);
                        int nr = controler.Asortyment.Count;
                        controler.Asortyment.Add(nr, temp);
                        controler.asortymentIndeks.Add(temp.nazwa, nr);
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
            catch (IOException e)
            {
                MessageBox.Show("Blad odczytu asortymentu " + e.ToString());
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

        public void saveKlienci(Controler controler)
        {
            StreamWriter sw = File.CreateText(klienciFilePath);

            try
            {
                sw.WriteLine(controler.Klienci.Count + ";");
                for (int i = 0; i < controler.Klienci.Count; i++)
                {
                    controler.Klienci.TryGetValue(i, out Klient linia);

                    sw.Write(linia.nazwa + ",");
                    sw.Write(linia.NIP + ",");
                    sw.Write(linia.telefon + ",");
                    sw.Write(linia.kod + ",");
                    sw.Write(linia.adres + ";");
                    sw.Write(System.Environment.NewLine);
                }

                sw.Close();
            }
            catch (IOException e)
            {
                MessageBox.Show("Blad zapisu klientow " + e.ToString());
                sw.Close();
            }
        }

        public void loadKlienci(Controler controler)
        {
            try
            {
                if (controler.Klienci.Count == 0)
                {
                    //string msg;
                    //msg = "Wczytywanie";
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
                        controler.Klienci.Add(controler.Klienci.Count, parsujStrKlient(linia));
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
            catch (IOException e)
            {
                MessageBox.Show("Blad odczytu klientow " + e.ToString());
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
            for (int i = 0; i < tab.Length; i++)
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

        public void saveTransakcje(Controler controler)
        {
            StreamWriter sw = File.CreateText(transakcjeFilePath);
            String temp;

            sw.WriteLine(controler.Transkacje.Count + ";");
            try
            {
                for (int i = 0; i < controler.Transkacje.Count; i++)
                {
                    controler.Transkacje.TryGetValue(i, out Sprzedaz linia);

                    sw.Write(linia.idFirmy + ",");
                    sw.Write(linia.nipFirmy + ",");
                    sw.Write(linia.adresFirmy + ",");
                    sw.Write(linia.nr_dok + ",");
                    sw.Write(linia.tabIDs.Length + "{");
                    //int[] temp = linia.tabIDs.ToArray();
                    int j = 0;
                    for (j = 0; j < (linia.tabIDs.Length); j++)
                    {
                        sw.Write(linia.tabIDs[j] + ",");
                    }
                    sw.Write("}");
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
            catch (IOException e)
            {
                MessageBox.Show("Blad zapisu transakcji " + e.ToString());
                sw.Close();
            }

        }

        public void loadTransakcje(Controler controler)
        {
            try
            {

                if (controler.Transkacje.Count == 0)
                {
                    //string msg;
                    //msg = "Wczytywanie";
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
                        controler.Transkacje.Add(controler.Transkacje.Count, parsujstrSprzedaz(linia));
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
            catch (IOException e)
            {
                MessageBox.Show("Bląd ładowania transakcji " + e.ToString());
            }
        }

    }

    public class PdfHandler
    {
        FileHandler fh = new FileHandler();
        string fakturyPath = @".\Faktury\";
        string raportyPath = @".\Raporty\";
        //PDFy do faktur oraz tworzenia raportów

        static void createPDFDeprecated(string numer)
        {
            var exportFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string nazwa = numer.Replace('/', '_');
            var exportFile = System.IO.Path.Combine("./" + nazwa + ".pdf");

            using (var writer = new PdfWriter(exportFile))
            {
                using (var pdf = new PdfDocument(writer))
                {
                    var doc = new Document(pdf); ;
                    ImageData imageData = ImageDataFactory.Create("LOGO.png");
                    Image image = new Image(imageData).ScaleAbsolute(100, 100).SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.RIGHT);
                    doc.Add(image);
                    doc.Add(new Paragraph("Faktura " + numer).SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER));

                    var table = new Table(new float[] { 4, 4, 4, 4 });
                    table.SetWidth(UnitValue.CreatePercentValue(100));
                    table.AddHeaderCell(new Cell().Add(new Paragraph("Nr")));
                    table.AddHeaderCell(new Cell().Add(new Paragraph("Produkt")));
                    table.AddHeaderCell(new Cell().Add(new Paragraph("Cena")));
                    table.AddHeaderCell(new Cell().Add(new Paragraph("Vat")));

                    doc.Add(table);
                    doc.Close();
                }
            }
        }

        public string createFaktura(Sprzedaz nowyRekordSprzedazy, Controler controler)
        {
            var exportFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string nazwa = nowyRekordSprzedazy.nr_dok.Replace('/', '_');
            var exportFile = System.IO.Path.Combine(fakturyPath + nazwa + ".pdf");

            controler.Klienci.TryGetValue(controler.KlientID, out Klient klient);

            try
            {
                using (var writer = new PdfWriter(exportFile))
                {
                    using (var pdf = new PdfDocument(writer))
                    {
                        var doc = new Document(pdf); ;
                        ImageData imageData = ImageDataFactory.Create("LOGO.png");
                        Image image = new Image(imageData).ScaleAbsolute(100, 100).SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.RIGHT);

                        doc.Add(image);
                        var naglowek = new Paragraph("Faktura " + nowyRekordSprzedazy.nr_dok).SetRelativePosition(200, 0, 200, 0);
                        doc.Add(naglowek);
                        doc.Add(new Paragraph("Kupujacy:" + klient.nazwa));
                        doc.Add(new Paragraph("Adres:" + klient.adres));
                        doc.Add(new Paragraph("NIP/REGON:" + klient.NIP));
                        doc.Add(new Paragraph("Data:" + nowyRekordSprzedazy.data));

                        var table = new Table(new float[] { 2, 6, 4, 4 });
                        table.SetWidth(UnitValue.CreatePercentValue(100));
                        table.AddHeaderCell(new Cell().Add(new Paragraph("Nr")));
                        table.AddHeaderCell(new Cell().Add(new Paragraph("Produkt")));
                        table.AddHeaderCell(new Cell().Add(new Paragraph("Cena")));
                        table.AddHeaderCell(new Cell().Add(new Paragraph("Vat")));

                        int numerator = 1;
                        foreach (int id_prod in nowyRekordSprzedazy.tabIDs)
                        {
                            controler.Asortyment.TryGetValue(id_prod, out Przedmiot rzecz);

                            table.AddCell(new Cell().Add(new Paragraph(numerator.ToString())));
                            table.AddCell(new Cell().Add(new Paragraph(rzecz.nazwa)));
                            table.AddCell(new Cell().Add(new Paragraph(rzecz.cena.ToString())));
                            table.AddCell(new Cell().Add(new Paragraph((rzecz.cena * rzecz.VAT / 100).ToString())));
                            numerator++;
                        }

                        table.AddCell(new Cell().Add(new Paragraph("")));
                        table.AddCell(new Cell().Add(new Paragraph("")));
                        table.AddCell(new Cell().Add(new Paragraph(nowyRekordSprzedazy.kwotaNetto + "zl")));
                        table.AddCell(new Cell().Add(new Paragraph(nowyRekordSprzedazy.podatekVat + "zl")));

                        doc.Add(table);

                        doc.Add(new Paragraph("Kwota netto:" + nowyRekordSprzedazy.kwotaNetto + "zl").SetRelativePosition(400, 0, 0, 0));
                        doc.Add(new Paragraph("Podatek:" + nowyRekordSprzedazy.podatekVat + "zl").SetRelativePosition(400, 0, 0, 0));
                        doc.Add(new Paragraph("PDF created with itext7 under AGPL license").SetFixedPosition(0,0,300));

                        doc.Close();
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Blad tworzenia pdf'a " + e.ToString());
            }
            return exportFile;
        }

        public void createReport(Controler controler)
        {
            var exportFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            var exportFile = System.IO.Path.Combine(raportyPath + DateTime.UtcNow.ToLocalTime().ToString("dd_MM_yyyy") + ".pdf");

            controler.Klienci.TryGetValue(controler.KlientID, out Klient klient);

            try
            {
                using (var writer = new PdfWriter(exportFile))
                {
                    using (var pdf = new PdfDocument(writer))
                    {
                        var doc = new Document(pdf); ;
                        ImageData imageData = ImageDataFactory.Create("LOGO.png");
                        Image image = new Image(imageData).ScaleAbsolute(100, 100).SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER);

                        doc.Add(image);

                        var table = new Table(new float[] { 1, 2, 1, 2, 2, 2, 2 });
                        table.SetWidth(UnitValue.CreatePercentValue(100));
                        table.AddHeaderCell(new Cell().Add(new Paragraph("Nr")));
                        table.AddHeaderCell(new Cell().Add(new Paragraph("Data")));
                        table.AddHeaderCell(new Cell().Add(new Paragraph("Id")));
                        table.AddHeaderCell(new Cell().Add(new Paragraph("NIP")));
                        table.AddHeaderCell(new Cell().Add(new Paragraph("Produkty")));
                        table.AddHeaderCell(new Cell().Add(new Paragraph("Netto")));
                        table.AddHeaderCell(new Cell().Add(new Paragraph("VAT")));

                        for (int id_sprzed = 0; id_sprzed < controler.Transkacje.Count; id_sprzed++)
                        {
                            controler.Transkacje.TryGetValue(id_sprzed, out Sprzedaz sprzedaz);

                            table.AddCell(new Cell().Add(new Paragraph(id_sprzed.ToString())));
                            table.AddCell(new Cell().Add(new Paragraph(sprzedaz.data.ToString())));
                            table.AddCell(new Cell().Add(new Paragraph(sprzedaz.idFirmy.ToString())));
                            table.AddCell(new Cell().Add(new Paragraph(sprzedaz.nipFirmy.ToString())));
                            table.AddCell(new Cell().Add(new Paragraph(fh.parsujIntTabStr(sprzedaz.tabIDs).Replace(",", ", "))));
                            table.AddCell(new Cell().Add(new Paragraph(sprzedaz.kwotaNetto.ToString())));
                            table.AddCell(new Cell().Add(new Paragraph(sprzedaz.podatekVat.ToString())));
                        }

                        doc.Add(table);

						doc.Add(new Paragraph("PDF created with itext7 under AGPL license").SetFixedPosition(0,0,300));
						doc.Close();
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Bląd tworzenia PDF'a " + e.ToString());
            }

            MessageBox.Show("Raport utworzony ");
        }

        public void generateReport(Controler controler)
        {
            Thread thread = new Thread(() =>createReport(controler));

            thread.Start();
            thread.Join();
        }
    }
}
