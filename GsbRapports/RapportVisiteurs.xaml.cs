using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
using dllRapportVisites;
using Newtonsoft.Json;
using System.Collections.Specialized;
using System.Xml.Serialization;
using System.IO;

namespace GsbRapports
{
    /// <summary>
    /// Logique d'interaction pour RapportVisiteurs.xaml
    /// </summary>
    public partial class RapportVisiteurs : Window
    {
        private Secretaire laSecretaire;
        private WebClient wb;
        private string site;
        private List<Rapport> lst;

        List<Rapport> listeRapport;
        string date1Rapport;
        string date2Rapport;
        public RapportVisiteurs(Secretaire laSecretaire, WebClient wb, string site)
        {
            InitializeComponent();
            this.laSecretaire = laSecretaire;
            this.wb = wb;
            this.site = site;
            string url = this.site + "visiteurs?ticket=" + this.laSecretaire.getHashTicketMdp(); // Regarder Api pour charger lst visiteurs
            string reponse = this.wb.DownloadString(url);
            dynamic d = JsonConvert.DeserializeObject(reponse);
            string visiteurs = d.visiteurs.ToString();// charge la lst des visiteurs
            string ticket = d.ticket;
            this.laSecretaire.ticket = ticket;
            List<Visiteur> lst = JsonConvert.DeserializeObject<List<Visiteur>>(visiteurs);
            this.cmbVisiteurs.ItemsSource = lst;
            this.cmbVisiteurs.DisplayMemberPath = "concatNomPrenom ";// récupére Nom + prénom dans Classe Visiteur
        }

        private void valider_Click(object sender, RoutedEventArgs e)
        {
            try 
            {
            string date1 = dtpDate1.Text;
            string date2 = dtpDate2.Text;

            bool erreurs = true;

            if (date1 == "")
            {
                erreurs = false;
                erreurDate1.Text = "Veuillez entrer une date valide";
            }
            if (date1 == "")
            {
                erreurs = false;
                erreurDate2.Text = "Veuillez entrer une date valide";
            }

                if (erreurs)
                {
                    Visiteur v = (Visiteur)cmbVisiteurs.SelectedItem; //visiteur selectionné dans la liste
                    string idVisiteur = v.id;//recupere l'id du visiteur selectionné
                    DateTime dTime = Convert.ToDateTime(date1);
                   
                    string dateTime1 = dTime.ToString("yyyy-MM-dd");
                    date1Rapport = dateTime1;
                    DateTime dTime2 = Convert.ToDateTime(date2);
                    string dateTime2 = dTime2.ToString("yyyy-MM-dd");
                    date2Rapport = dateTime2;
                    string urlrapport = this.site + "rapports?ticket=" + this.laSecretaire.getHashTicketMdp() + "&idVisiteur=" + idVisiteur + "&dateDebut=" + dateTime1 + "&dateFin=" + dateTime2;
                    string reponse = this.wb.DownloadString(urlrapport);
                    dynamic r = JsonConvert.DeserializeObject(reponse);
                    string rapports = r.rapports.ToString();
                    string ticket = r.ticket;
                    this.laSecretaire.ticket = ticket;
                    List<Rapport> lst = JsonConvert.DeserializeObject<List<Rapport>>(rapports);
                    this.dtGridRapports.ItemsSource = lst;
                    this.listeRapport = lst;






                }
            }
            catch (WebException ex)
            {
                if (ex.Response is HttpWebResponse)
                    MessageBox.Show(((HttpWebResponse)ex.Response).StatusCode.ToString());
            }
        }

        private void exportationXml_Click(object sender, RoutedEventArgs e)
        {
            try 
            {
                Visiteur v = (Visiteur)cmbVisiteurs.SelectedItem; //visiteur selectionné dans la liste
                string idVisiteur = v.id;//recupere l'id du visiteur selectionné
                string date1 = dtpDate1.Text;
                string date2 = dtpDate2.Text;
                DateTime dTime = Convert.ToDateTime(date1);
                string dateTime1 = dTime.ToString("yyyy-MM-dd");
                DateTime dTime2 = Convert.ToDateTime(date2);
                string dateTime2 = dTime2.ToString("yyyy-MM-dd");
                string urlrapport = this.site + "rapports?ticket=" + this.laSecretaire.getHashTicketMdp() + "&idVisiteur=" + idVisiteur + "&dateDebut=" + dateTime1 + "&dateFin=" + dateTime2;
                string reponse = this.wb.DownloadString(urlrapport);
                dynamic r = JsonConvert.DeserializeObject(reponse);
                string rapports = r.rapports.ToString();
                string ticket = r.ticket;
                List<Rapport> lst = JsonConvert.DeserializeObject<List<Rapport>>(rapports);




                string dateDuRapport = date1Rapport + "au" + date2Rapport;
                FileStream f = new FileStream("rapports/listeRapports" + dateDuRapport + ".xml", FileMode.Create);
                XmlSerializer x = new XmlSerializer(typeof(List<Rapport>));
                x.Serialize(f, this.listeRapport); // Recuperation de la liste dans le global
                MessageBox.Show("Exportation réussie !");

            }
            catch (WebException ex)
            {
                MessageBox.Show(ex.Message);
            }





        }
    }
}
