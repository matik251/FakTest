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
    /// Interaction logic for Obsługa.xaml
    /// </summary>
    public partial class Obsługa : Window
    {
        Controler _controler;
        PdfHandler pdfh = new PdfHandler();

        public Obsługa(Controler controler)
        {
            InitializeComponent();
            _controler = controler;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow win = new MainWindow(_controler);
            win.Show();
            this.Close();
        }

        private void addProduct(object sender, RoutedEventArgs e)
        {
            DodanieProduktu win = new DodanieProduktu(_controler);
            win.Show();
        }

        private void addClient(object sender, RoutedEventArgs e)
        {
            DodanieKlienta win = new DodanieKlienta(_controler);
            win.Show();
        }

        private void showTransactions(object sneder, RoutedEventArgs e)
        {
            DodanieTransakcji win = new DodanieTransakcji(_controler);
            win.Show();
        }

        private void generateRaport(object sender, RoutedEventArgs e)
        {
            pdfh.generateReport(_controler);
        }

        private void generateJPK(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
