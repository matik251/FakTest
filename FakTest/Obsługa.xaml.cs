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
        public Obsługa()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow win = new MainWindow();
            win.Show();
            this.Close();
        }

        private void addProduct(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void addClient(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void generateRaport(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void generateJPK(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
