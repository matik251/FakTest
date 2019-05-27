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
        public int _cena { get; set; }
        public int _stawkaVat { get; set; }

        public DodanieKlienta(Controler controler)
        {
            InitializeComponent();
            _controler = controler;
            DataContext = this;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Add_Client(object sender, RoutedEventArgs e)
        {
            //TODO

            tb_nazwa.Text = "";
            tb_cena.Text = "0";
            tb_stawkaVAT.Text = "0";
        }

        public void addLastElementToDataGrid()
        {
            //TODO
        }

        public void fillDataGrid()
        {
            //TODO
        }
    }
}
