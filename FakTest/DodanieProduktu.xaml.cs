﻿using System;
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

            fillDataGrid();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Add_Product(object sender, RoutedEventArgs e)
        {
            Przedmiot _przedmiot = new Przedmiot(_nazwa,_cena,_stawkaVat);

            int qounter = _controler.Asortyment.Count;

            _controler.Asortyment.Add(qounter, _przedmiot);

            addLastElementToDataGrid();

            tb_nazwa.Text = "";
            tb_cena.Text = "0";
            tb_stawkaVAT.Text = "0";
        }

        public void addLastElementToDataGrid()
        {
            Sprzedaż.dataGridProdukt produkt = new Sprzedaż.dataGridProdukt();
            _controler.Asortyment.TryGetValue((_controler.Asortyment.Count - 1), out Przedmiot linia);
            produkt.id = (_controler.Asortyment.Count - 1);//.ToString();
            produkt.nazwa = linia.nazwa;
            produkt.typ = "brak";
            produkt.netto = linia.cena.ToString() + "zł";
            produkt.stawka = linia.VAT.ToString() + "%";
            int temp = (linia.cena * linia.VAT / 100);
            produkt.podatek = temp.ToString() + " zł";
            int temp2 = linia.cena + temp;
            produkt.brutto = temp2.ToString() + " zł";
            DataGridProduktow.Items.Add(produkt);
        }

        public void fillDataGrid()
        {

            for (int i = 0; i < _controler.Asortyment.Count; i++)
            {
                Sprzedaż.dataGridProdukt produkt = new Sprzedaż.dataGridProdukt();
                _controler.Asortyment.TryGetValue(i, out Przedmiot linia);
                produkt.id = i;//.ToString();
                produkt.nazwa = linia.nazwa;
                produkt.typ = "brak";
                produkt.netto = linia.cena.ToString() + "zł";
                produkt.stawka = linia.VAT.ToString() + "%";
                int temp = (linia.cena * linia.VAT / 100);
                produkt.podatek = temp.ToString() + " zł";
                int temp2 = linia.cena + temp;
                produkt.brutto = temp2.ToString() + " zł";
                DataGridProduktow.Items.Add(produkt);
            }
        }
    }
}