using dllRapportVisites;
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
using Newtonsoft.Json;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

namespace GsbRapports
{
    /// <summary>
    /// Logique d'interaction pour AjoutVisiteurWindow.xaml
    /// </summary>
    public partial class AjoutVisiteurWindow : Window
    {
        private Secretaire laSecretaire;
        private WebClient wb;
        private string site;

        public AjoutVisiteurWindow(Secretaire laSecretaire, WebClient wb, string site)
        {
            InitializeComponent();
            this.laSecretaire = laSecretaire;
            this.wb = wb;
            this.site = site;
        }

        private void btnValider_Click1(object sender, RoutedEventArgs e)
        {
           //push final

            try
            {
                string id = this.txtIdVisiteur.Text;
                string cp = this.txtCodePostal.Text;
                string nom = this.txtNom.Text;
                string prenom = this.txtPrenom.Text;
                string adresse = this.txtAdresse.Text;
                string ville = this.txtVille.Text;
                string date1 = this.txtDateEmbauche.Text;

                bool ok = true;

                if (id.Length != 3)
                {
                    ok = false;
                    erreurId.Text = "L'id doit contenir 3 caracteres";
                }
                else
                {
                    erreurId.Text = "";
                }

                if (nom.Length == 0)
                {
                    ok = false;
                    erreurNom.Text = "Veuillez entrer un nom";
                }
                else
                {
                    erreurNom.Text = "";
                }

                if (nom.Length == 0)
                {
                    ok = false;
                    erreurNom.Text = "Veuillez entrer un nom";
                }

                if (prenom.Length == 0)
                {
                    ok = false;
                    erreurPrenom.Text = "Veuillez entrer un prénom";
                }
                else
                {
                    erreurPrenom.Text = "";
                }

                if (adresse.Length == 0)
                {
                    ok = false;
                    erreurAdresse.Text = "Veuillez entrer une adresse";
                }
                else
                {
                    erreurAdresse.Text = "";
                }

                if (cp.Length != 5)
                {
                    ok = false;
                    erreurCp.Text = "Le code postale doit contenir " + '\n' +" 5 caractères";
                }
                else
                {
                    erreurCp.Text = "";
                }


                if (ville.Length == 0)
                {
                    ok = false;
                    erreurVille.Text = "Veuillez entrer une ville";
                }
                else
                {
                    erreurVille.Text = "";
                }

                if(date1 == "")
                {
                    ok = false;
                    erreurdate.Text ="Veuillez entrer une date valide";
                }


                if (ok)
                {
                    string url = this.site + "visiteurs";
                    NameValueCollection parametres = new NameValueCollection();
                    parametres.Add("ticket", this.laSecretaire.getHashTicketMdp());
                    parametres.Add("idVisiteur", id);
                    parametres.Add("nom", nom);
                    parametres.Add("prenom", prenom);
                    parametres.Add("adresse", adresse);
                    parametres.Add("cp", cp);
                    parametres.Add("ville", ville);
                    DateTime dTime = Convert.ToDateTime(date1);
                    string dateTime1 = dTime.ToString("yyyy-MM-dd");
                    parametres.Add("dateEmbauche", dateTime1);
                    byte[] tabByte = wb.UploadValues(url, parametres); // envoie des donnés en post 
                    string ticket = UnicodeEncoding.UTF8.GetString(tabByte);
                    this.laSecretaire.ticket = ticket.Substring(2);// anti slash n 
                    MessageBox.Show("Ajout d'un visiteur effectué");
                    Close();
                }


            }
            catch (WebException ex)
            {
                if (ex.Response is HttpWebResponse)
                    MessageBox.Show(((HttpWebResponse)ex.Response).StatusCode.ToString());
            }

        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

      
    }
}
