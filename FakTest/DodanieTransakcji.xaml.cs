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
            for (int i = 0; i < _controler.Transkacje.Count; i++)
            {
                dataGridTransakcje transakcja = new dataGridTransakcje();
                _controler.Transkacje.TryGetValue(i, out Sprzedaz linia);

                transakcja.id = i;
                transakcja.id_firmy = linia.idFirmy;
                transakcja.nip = linia.nipFirmy;
                transakcja.adres = linia.adresFirmy;
                transakcja.nr_fak = linia.nr_dok;
                transakcja.id_prod = _controler.parsujIntTabStr(linia.tabIDs);
                transakcja.data = linia.data;
                transakcja.netto = linia.kwotaNetto.ToString() + "zł";
                transakcja.vat = linia.podatekVat.ToString() + "zł";

                DataGridTransakcji.Items.Add(transakcja);
            }
        }
    }
}
