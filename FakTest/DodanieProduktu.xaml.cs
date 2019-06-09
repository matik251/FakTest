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
    /// Interaction logic for DodanieProduktu.xaml
    /// </summary>
    public partial class DodanieProduktu : Window
    {
        Controler _controler;

        public string _nazwa { get; set; }
        public decimal _cena { get; set; }
        public int _stawkaVat { get; set; }
        public bool _nazwaPoprawna = false;
        public bool _cenaPoprawna = false;
        public bool _stawkaPoprawna = false;

        public DodanieProduktu(Controler controler)
        {
            InitializeComponent();
            _controler = controler;
            DataContext = this;

            fillDataGrid();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _controler.wyczyscKoszyk();
            this.Close();
        }

        private void Add_Product(object sender, RoutedEventArgs e)
        {
            if (_nazwaPoprawna && _cenaPoprawna && _stawkaPoprawna)
            {

                Przedmiot _nowyPrzedmiot = new Przedmiot(_nazwa, _cena, _stawkaVat);

                int qounter = _controler.Asortyment.Count;

                _controler.Asortyment.Add(qounter, _nowyPrzedmiot);

                addLastElementToDataGrid();

                tb_nazwa.Text = "";
                tb_cena.Text = "0.0";
                tb_stawkaVAT.Text = "0";
            }
        }

        public void addLastElementToDataGrid()
        {
            dataGridProdukt produkt = new dataGridProdukt();
            _controler.Asortyment.TryGetValue((_controler.Asortyment.Count - 1), out Przedmiot linia);
            produkt.id = (_controler.Asortyment.Count - 1);//.ToString();
            produkt.nazwa = linia.nazwa;
            produkt.typ = "brak";
            produkt.netto = linia.cena.ToString() + "zł";
            //produkt.netto = linia.cena.ToString() + "zł";
            produkt.stawka = linia.VAT.ToString() + "%";
            decimal temp = (linia.cena * linia.VAT / 100);
            produkt.podatek = temp.ToString() + " zł";
            decimal temp2 = linia.cena + temp;
            produkt.brutto = temp2.ToString() + " zł";
            DataGridProduktow.Items.Add(produkt);
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
                DataGridProduktow.Items.Add(produkt);
            }
        }

        public void verifyNazwa(object sender, RoutedEventArgs e)
        {
            if (tb_nazwa.Text.Length == 0)
            {
                tb_nazwa.Background = Brushes.Red;
            }
            else
            {
                if (!_controler.sprawdzenieRegex(tb_nazwa.Text))
                {
                    tb_nazwa.Background = Brushes.Red;
                    _nazwaPoprawna = false;
                }
                else
                {
                    tb_nazwa.Background = Brushes.GreenYellow;
                    _nazwaPoprawna = true;
                }
            }
        }
        public void verifyCena(object sender, RoutedEventArgs e)
        {
            if (tb_cena.Text.Length == 0)
            {
                tb_cena.Background = Brushes.Red;
            }
            else
            {
                if (!_controler.cenaRegex(tb_cena.Text))
                {
                    tb_cena.Background = Brushes.Red;
                    _cenaPoprawna = false;
                }
                else
                {
                    tb_cena.Background = Brushes.GreenYellow;
                    _cenaPoprawna = true;
                }
            }
        }
        public void verifyVat(object sender, RoutedEventArgs e)
        {
            if (tb_stawkaVAT.Text.Length == 0)
            {
                tb_stawkaVAT.Background = Brushes.Red;
            }
            else
            {
                if (!_controler.vatRegex(tb_stawkaVAT.Text))
                {
                    tb_stawkaVAT.Background = Brushes.Red;
                    _stawkaPoprawna = false;
                }
                else
                {
                    tb_stawkaVAT.Background = Brushes.GreenYellow;
                    _stawkaPoprawna = true;
                }
            }
        }
    }
}
