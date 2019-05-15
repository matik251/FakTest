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

        FileHandler _fileHandler = new FileHandler();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonBase1_OnClick(object sender, RoutedEventArgs e)
        {
            Sprzedaż win = new Sprzedaż();
            win.Show();
            this.Close();
            _fileHandler.writeMainFile();
        }

        private void ButtonBase2_OnClick(object sender, RoutedEventArgs e)
        {
            Obsługa win = new Obsługa();
            win.Show();
            this.Close();
        }

        private void ButtonBase12_OnClick(object sender, RoutedEventArgs e)
        {
            string msg;
            msg = _fileHandler.readMainFile();
            MessageBox.Show(msg);
        }
    }
}
