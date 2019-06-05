using System;
using System.Collections.Generic;
using System.Data;
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
//-----------------------------------------------------------------------------------------------------
        public Sprzedaż(Controler controler)
        {
            InitializeComponent();
            _controler = controler;

            fillDataGrid(DataGridProduktow);
        }
//-----------------------------------------------------------------------------------------------------
        public List<int> getZaznaczoneProdukty(DataGrid dataGrid)
        {
            var cellInfos = dataGrid.SelectedCells;
            //var list1 = new List<Przedmiot>();
            var temp = new List<int>();
            var listaIndeksow = new List<int>();
            foreach (DataGridCellInfo cellInfo in cellInfos)
            {
                if (cellInfo.IsValid)
                {
                    var content = cellInfo.Column.GetCellContent(cellInfo.Item);
                    var row = (dataGridProdukt)content.DataContext;
                    temp.Add(row.id);
                }
            }

            foreach (int wartosc in temp)
            {
                bool dodac = true;
                foreach (int sprawdzana in listaIndeksow)
                {
                    if (wartosc == sprawdzana)
                    {
                        dodac = false;
                    }
                }

                if (dodac)
                {
                    listaIndeksow.Add(wartosc);
                }
            }

            return listaIndeksow;
        }
        
        public void fillDataGrid(DataGrid dataGrid)
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
                decimal temp = (linia.cena * linia.VAT/100);
                produkt.podatek = temp.ToString()  + " zł";
                decimal temp2 = linia.cena + temp;
                produkt.brutto = temp2.ToString() + " zł";
                dataGrid.Items.Add(produkt);
            }
        }

        public void clearDataGrid(DataGrid dataGrid)
        {
            dataGrid.Items.Clear();
            dataGrid.Items.Refresh();
        }

        public void fillDataGridWithListedItems(DataGrid dataGrid, List<int> zaznaczone)
        {
            foreach (int i in zaznaczone)
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
                dataGrid.Items.Add(produkt);
            }
        }
//-----------------------------------------------------------------------------------------------------
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

        public void finalizujBtnClk(object sender, RoutedEventArgs e)
        {
            //Finalizowanie z wartosciami
            string msg = "";
            foreach(int index in _controler.KoszykList)
            {
                msg = msg + " " + index.ToString();        
            }
            msg = msg + Environment.NewLine + "Suma= " + _controler.KoszykSuma + "zł";

            MessageBox.Show(msg);

            //TODO

            _controler.transakcjaWToku = true;

            DodanieKlienta win = new DodanieKlienta(_controler);
            win.Show();
            //this.Close();

        }

        public void dodajDoKoszyka(object sender, RoutedEventArgs e)
        {
            //dodanie do koszyka
            List<int> zaznaczone = new List<int>();
            zaznaczone = getZaznaczoneProdukty(DataGridProduktow);
            if (zaznaczone.Count != 0)
            {
                _controler.addItemsToKoszyk(zaznaczone);
                fillDataGridWithListedItems(DataGridKoszyk, zaznaczone);
                zaznaczone = new List<int>();
            }
            updateSumy();
        }

        public void usunZKoszyka(object sender, RoutedEventArgs e)
        {
            //usuwanie z koszyka
            List<int> zaznaczone = new List<int>();
            zaznaczone = getZaznaczoneProdukty(DataGridKoszyk);
            if(zaznaczone.Count != 0)
            {
                clearDataGrid(DataGridKoszyk);
                _controler.removeItemsFromKoszyk(zaznaczone);
                fillDataGridWithListedItems(DataGridKoszyk, _controler.KoszykList);
            }
            zaznaczone = new List<int>();
            updateSumy();
        }

        public void updateSumy()
        {
            sumaKoszykaTextView.Text = _controler.KoszykSuma.ToString() + "zł";
            sumaVatTextView.Text = _controler.koszykPodatek.ToString() + "zł";
        }
 //-----------------------------------------------------------------------------------------------------
    }
}
