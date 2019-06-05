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
    /// Interaction logic for DodanieTransakcji.xaml
    /// </summary>
    public partial class DodanieTransakcji : Window
    {
        Controler _controler;

        public string _nazwa { get; set; }
        public decimal _cena { get; set; }
        public int _stawkaVat { get; set; }

        public DodanieTransakcji(Controler controler)
        {
            InitializeComponent();
            _controler = controler;
            DataContext = this;

            fillDataGrid();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public void fillDataGrid()
        {
            for (int i = 0; i < _controler.Asortyment.Count; i++)
            {
                dataGridProdukt produkt = new dataGridProdukt();
                _controler.Asortyment.TryGetValue(i, out Przedmiot linia);
                produkt.id = i;//.ToString();
                produkt.nazwa = linia.nazwa;
                produkt.typ = "brak";
                produkt.netto = linia.cena.ToString() + "zł";
                produkt.stawka = linia.VAT.ToString() + "%";
                decimal temp = (linia.cena * linia.VAT / 100);
                produkt.podatek = temp.ToString() + " zł";
                decimal temp2 = linia.cena + temp;
                produkt.brutto = temp2.ToString() + " zł";
                DataGridTransakcji`.Items.Add(produkt);
            }
        }
    }
}
