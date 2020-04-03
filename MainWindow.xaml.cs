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
        CancellationTokenSource ct;
        public MainWindow()
        {
            InitializeComponent();
        }
        private void lst_Attori_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Attore a = (Attore)lst_Attori.SelectedItem;
            if (a != null)
            {
                lbl_Attore.Content = a.ToString();
                txt_Oscar.Text = a.Oscar.ToString();
                txt_Nomination.Text = a.Nomination.ToString();
            }
        }
        private void btn_Aggiorna_Click(object sender, RoutedEventArgs e)
        {
            ct = new CancellationTokenSource();
            btn_Aggiorna.IsEnabled = false;
            btn_Stop.IsEnabled = true;
            lst_Attori.Items.Clear();
            Task.Factory.StartNew(() => CaricaDati());
        }

        private void CaricaDati()
        {
            string path = @"Oscar.xml";
            XDocument xmlDoc = XDocument.Load(path);
            XElement xmlattori = xmlDoc.Element("attori");
            var xmlattore = xmlattori.Elements("attore");
            Thread.Sleep(1000);

            foreach (var item in xmlattore)
            {
                XElement xmlFirstName = item.Element("nome");
                XElement xmlLastName = item.Element("cognome");
                XElement xmlNascita = item.Element("data");
                XElement xmlOscar = item.Element("oscar");
                XElement xmlNomination = item.Element("nomination");

                Attore a = new Attore();
                {
                    a.Nome = xmlFirstName.Value;
                    a.Cognome = xmlLastName.Value;
                    a.DataNascita = Convert.ToDateTime(xmlNascita.Value);
                    a.Oscar = Convert.ToInt32(xmlOscar.Value);
                    a.Nomination = Convert.ToInt32(xmlNomination.Value);
                }

                Dispatcher.Invoke(() => lst_Attori.Items.Add(a));

                if (ct.Token.IsCancellationRequested)
                {
                    break;
                }

                Thread.Sleep(1000);
            }

            Dispatcher.Invoke(() =>
            {
                btn_Aggiorna.IsEnabled = true;
                btn_Stop.IsEnabled = false;
                ct = null;
            });
        }

        private void btn_Stop_Click(object sender, RoutedEventArgs e)
        {
            ct.Cancel();
        }

        private void btn_Salva_Click(object sender, RoutedEventArgs e)
        {
            Attore a = (Attore)lst_Attori.SelectedItem;
            int OscarCambiati = Convert.ToInt32(txt_Oscar.Text);
            int NominationCambiate = Convert.ToInt32(txt_Nomination.Text);

            if (a.Oscar != OscarCambiati)
            {
                a.Oscar = OscarCambiati;
            }

            if (a.Nomination != NominationCambiate)
            {
                a.Nomination = NominationCambiate;
            }

            Task.Factory.StartNew(Scrivi);

            //string path = @"Oscar.xml";
            //XDocument xmlDoc = XDocument.Load(path);
            //XElement xmlattori = xmlDoc.Element("attori");
            //var xmlattore = xmlattori.Elements("attore");

            //foreach (var item in xmlattore)
            //{
            //    XElement xmlFirstName = item.Element("nome");
            //    XElement xmlLastName = item.Element("cognome");
            //    XElement xmlNascita = item.Element("data");
            //    XElement xmlOscar = item.Element("oscar");
            //    XElement xmlNomination = item.Element("nomination");

            //    if(xmlFirstName.ToString() == a.Nome && xmlLastName.ToString() == a.Cognome)
            //    {
            //        item.SetElementValue("oscar", OscarCambiati);

            //        item.SetElementValue("nomination", NominationCambiate);
            //    }

            //}
            //xmlDoc.Save(@"Oscar.xml");
        }

        private void Scrivi()
        {
            string path = @"OscarModificato.xml";

            XElement xmlAttori = new XElement("attori");

            foreach (Attore attore in lst_Attori.Items)
            {
                XElement xmlAttore = new XElement("Attore");
                XElement xmlNome = new XElement("Nome", attore.Nome);
                XElement xmlCognome = new XElement("Cognome", attore.Cognome);
                XElement xmlNascita = new XElement("Data", attore.DataNascita);
                XElement xmlOscar = new XElement("Oscar", attore.Oscar);
                XElement xmlNomination = new XElement("Nomination", attore.Nomination);

                xmlAttore.Add(xmlNome);
                xmlAttore.Add(xmlCognome);
                xmlAttore.Add(xmlNascita);
                xmlAttore.Add(xmlOscar);
                xmlAttore.Add(xmlNomination);
                xmlAttori.Add(xmlAttore);
            }

            xmlAttori.Save(path);
        }
    }
}
