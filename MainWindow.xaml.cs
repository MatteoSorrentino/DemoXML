using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Xml.Linq;

namespace DemoXML
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Attore> attori;
        public MainWindow()
        {
            InitializeComponent();
        }
        private void btn_Aggiorna_Click(object sender, RoutedEventArgs e)
        {
            Task.Factory.StartNew(() => CaricaDati());
        }

        private void CaricaDati()
        {
            attori = new List<Attore>();
            string path = @"Oscar.xml";
            XDocument xmlDoc = XDocument.Load(path);
            XElement xmlattori = xmlDoc.Element("attori");
            var xmlattore = xmlattori.Elements("attore");

            foreach (var item in xmlattore)
            {
                XElement xmlFirstName = item.Element("nome");
                XElement xmlLastName = item.Element("cognome");
                XElement xmlNascita = item.Element("data");
                XElement xmlOscar = item.Element("oscar");
                XElement xmlNomination = item.Element("nomination");

                Attore a = new Attore();
                a.Nome = xmlFirstName.Value;
                a.Cognome = xmlLastName.Value;
                a.DataNascita = Convert.ToDateTime(xmlNascita.Value);
                a.Oscar = Convert.ToInt32(xmlOscar.Value);
                a.Nomination = Convert.ToInt32(xmlNomination.Value);

                attori.Add(a);
            }

            Dispatcher.Invoke(() => lst_Attori.ItemsSource = attori);
        }

    }
}
