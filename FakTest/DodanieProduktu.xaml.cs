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
        public int _cena { get; set; }
        public int _stawkaVat { get; set; }

        public DodanieProduktu(Controler controler)
        {
            InitializeComponent();
            _controler = controler;
            DataContext = this;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Add_Product(object sender, RoutedEventArgs e)
        {
            Przedmiot _przedmiot = new Przedmiot(_nazwa,_cena,_stawkaVat);

            int qounter = _controler.Asortyment.Count();

            _controler.Asortyment.Add(qounter, _przedmiot);

            this.Close();
        }
    }
}
