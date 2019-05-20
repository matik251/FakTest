using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FakTest
{
    /// <summary>
    /// Interaction logic for Sprzedaż.xaml
    /// </summary>
    public partial class Sprzedaż : Window
    {
        Controler _controler = new Controler();

        public Sprzedaż(Controler controler)
        {
            InitializeComponent();
            _controler = controler;

            fillDataGrid();

        }

        public void fillDataGrid()
        {
           
            for (int i = 0; i < _controler.Asortyment.Count; i++)
            {
                produkty produkt = new produkty();
                _controler.Asortyment.TryGetValue(i, out Przedmiot linia);
                produkt.id = i.ToString();
                produkt.nazwa = linia.nazwa;
                produkt.typ = "brak";
                produkt.netto = linia.cena.ToString() + "zł";
                produkt.stawka = linia.VAT.ToString() + "%";
                int temp = (linia.cena * linia.VAT/100);
                produkt.podatek = temp.ToString()  + " zł";
                int temp2 = linia.cena + temp;
                produkt.brutto = temp2.ToString() + " zł";
                DataGridProduktow.Items.Add(produkt);
            }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Nie znaleziono podanego produktu");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow win = new MainWindow(_controler);
            win.Show();
            this.Close();

        }

        public class produkty
        {
            public string id { get; set; }
            public string nazwa { get; set; }
            public string typ { get; set; }
            public string netto { get; set; }
            public string stawka { get; set; }
            public string podatek { get; set; }
            public string brutto { get; set; }
        }
    }
}
