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
using System.Windows.Navigation;
using System.Windows.Shapes;

using FakTest.Properties;


namespace FakTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        FileHandler fh = new FileHandler();
        private Controler _controler;


        public MainWindow()
        {
            InitializeComponent();
            _controler = new Controler();

            fillDataGrid();
        }

        public MainWindow(Controler controler)
        {
            InitializeComponent();
            _controler = controler;
            fillDataGrid();
        }

        private void ButtonBase1_OnClick(object sender, RoutedEventArgs e)
        {
            Sprzedaż win = new Sprzedaż(_controler);
            win.Show();
            this.Close();
        }

        private void ButtonBase2_OnClick(object sender, RoutedEventArgs e)
        {
            Obsługa win = new Obsługa(_controler);
            win.Show();
            this.Close();
        }

        private void ButtonBase12_OnClickSave(object sender, RoutedEventArgs e)
        {
            /*string msg;
            msg = _fileHandler.readMainFile();
            MessageBox.Show(msg);*/
            /*
            _controler.saveAsortyment();
            _controler.saveKlienci();
            _controler.saveTransakcje();*/
            _controler.saveData();
        }

        private void ButtonBase12_OnClickLoad(object sender, RoutedEventArgs e)
        {
            /*string msg;
            msg = _fileHandler.readMainFile();
            MessageBox.Show(msg);*/

            /*_controler.loadAsortyment();
            _controler.loadKlienci();
            _controler.loadTransakcje();*/
            _controler.loadData();
            fillDataGrid();
        }

        public void fillDataGrid()
        {
            int count = _controler.Transkacje.Count;
            int forCounter = 0;
            for (int i = (count-1); i > 0; i--)
            {
                dataGridTransakcje transakcja = new dataGridTransakcje();
                _controler.Transkacje.TryGetValue(i, out Sprzedaz linia);

                transakcja.id = i;
                transakcja.id_firmy = linia.idFirmy;
                transakcja.nip = linia.nipFirmy;
                transakcja.adres = linia.adresFirmy;
                transakcja.nr_fak = linia.nr_dok;
                transakcja.id_prod = fh.parsujIntTabStr(linia.tabIDs);
                transakcja.data = linia.data;
                transakcja.netto = linia.kwotaNetto.ToString() + "zł";
                transakcja.vat = linia.podatekVat.ToString() + "zł";

                DataGridOstatnich.Items.Add(transakcja);

                forCounter++;

                if (forCounter > 7)
                {
                    i = 0;
                }
            }
        }
    }
}
