using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakTest
{
    class TransakcjeHandler
    {
        PdfHandler pdfh = new PdfHandler();
        KoszykHandler kh = new KoszykHandler();
        //Obsluga transakcji

        public void utworzTransakcje(Controler controler)
        {
            int counter = controler.Transkacje.Count;
            DateTime data = DateTime.UtcNow.ToLocalTime();
            string nr_fak = "0/0/0";
            nr_fak = data.ToString("yyyy") + '/' + data.ToString("MM") + '/' + counter.ToString();
            string czas = data.ToString("dd-MM-yyyy HH:mm:ss");
            controler.Klienci.TryGetValue(controler.KlientID, out Klient linia);
            Sprzedaz nowyRekordSprzedazy = new Sprzedaz(
                controler.KlientID,
                linia.NIP,
                linia.adres,
                //"0/0/0",
                nr_fak,
                controler.KoszykList.ToArray(),
                //"03:06:2019",
                czas,
                controler.KoszykSuma,
                controler.koszykPodatek
                );


            controler.Transkacje.Add(counter, nowyRekordSprzedazy);
            string pdf = pdfh.createFaktura(nowyRekordSprzedazy, controler);

            kh.wyczyscKoszyk(controler);

            //Process myProcess = new Process();
            //myProcess.StartInfo.FileName = "acroRd32.exe"; //not the full application path
            pdf = AppDomain.CurrentDomain.BaseDirectory + pdf;
            //pdf = "/A "  + pdf;
            //myProcess.StartInfo.Arguments = pdf;
            //myProcess.Start();
            System.Diagnostics.Process.Start(pdf);

        }

    }
}
