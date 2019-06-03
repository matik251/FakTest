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
    /// Interaction logic for DodanieKlienta.xaml
    /// </summary>
    public partial class DodanieKlienta : Window
    {
        Controler _controler;

        public string _nazwa { get; set; }
        public string _nip { get; set; }
        public string _telefon { get; set; }
        public string _kod { get; set; }
        public string _adres { get; set; }

        public DodanieKlienta(Controler controler)
        {
            InitializeComponent();
            _controler = controler;
            DataContext = this;

            if(_controler.transakcjaWToku == true)
            {
                ContinueTransacitionBtn.Visibility = System.Windows.Visibility.Visible;
            }

            fillDataGrid();
        }
//-----------------------------------------------------------------------------------------------------
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Add_Client(object sender, RoutedEventArgs e)
        {
            Klient _nowyKlient = new Klient(_nazwa, _nip, _telefon, _kod, _adres);

            int qounter = _controler.Klienci.Count;

            _controler.Klienci.Add(qounter, _nowyKlient);

            addLastElementToDataGrid();

            tb_nazwa.Text = "";
            tb_nip.Text = "";
            tb_telefon.Text = "";
            tb_kod.Text = "";
            tb_adres.Text = "";
        }
//-----------------------------------------------------------------------------------------------------
        public void addLastElementToDataGrid()
        {
            dataGridKlient nowyKlient = new dataGridKlient();
            _controler.Klienci.TryGetValue((_controler.Klienci.Count - 1), out Klient linia);
            nowyKlient.id = (_controler.Klienci.Count - 1);//.ToString();
            nowyKlient.nazwa = linia.nazwa;
            nowyKlient.nip = linia.NIP;
            nowyKlient.telefon = linia.telefon;
            nowyKlient.kod = linia.kod;
            nowyKlient.adres = linia.adres;
            DataGridKlientow.Items.Add(nowyKlient);
        }

        public void fillDataGrid()
        {
            for (int i = 0; i < _controler.Klienci.Count; i++)
            {
                dataGridKlient nowyKlient = new dataGridKlient();
                _controler.Klienci.TryGetValue(i, out Klient linia);
                nowyKlient.id = i;//.ToString();
                nowyKlient.nazwa = linia.nazwa;
                nowyKlient.nip = linia.NIP;
                nowyKlient.telefon = linia.telefon;
                nowyKlient.kod = linia.kod;
                nowyKlient.adres = linia.adres;
                DataGridKlientow.Items.Add(nowyKlient);
            }
        }

        public int getZaznaczony(DataGrid dataGrid)
        {
            var cellInfos = dataGrid.SelectedCells;
            //var list1 = new List<Przedmiot>();
            int temp = -1;
            var listaIndeksow = new List<int>();
            foreach (DataGridCellInfo cellInfo in cellInfos)
            {
                if (cellInfo.IsValid)
                {
                    var content = cellInfo.Column.GetCellContent(cellInfo.Item);
                    var row = (dataGridKlient)content.DataContext;
                    temp = row.id;
                }
            }

            return temp;
        }

//-----------------------------------------------------------------------------------------------------
        public void ContinueTransaction(object sender, RoutedEventArgs e)
        {
            _controler.KlientID = getZaznaczony(DataGridKlientow);
            MessageBox.Show("KLientID: " + _controler.KlientID);

        }


    }
}
