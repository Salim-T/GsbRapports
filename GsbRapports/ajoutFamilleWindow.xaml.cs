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
using System.Net;
using dllRapportVisites;
using Newtonsoft.Json;
using System.Collections.Specialized;//pour api

namespace GsbRapports
{
    /// <summary>
    /// Logique d'interaction pour ajoutFamilleWindow.xaml
    /// </summary>
    public partial class ajoutFamilleWindow : Window
    {
        private Secretaire laSecretaire;
        private WebClient wb;
        private string site;

        public ajoutFamilleWindow(Secretaire laSecretaire, WebClient wb, string site)
        {
            InitializeComponent();
            this.laSecretaire = laSecretaire;
            this.site = site;
            this.wb = wb;
        }

        private void btnValider_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                string url = this.site + "familles";
                NameValueCollection n = new NameValueCollection();
                n.Add("ticket", this.laSecretaire.getHashTicketMdp());
                n.Add("idFamille", txtId.Text.ToString());
                n.Add("libelle", txtLibelle.Text);
                byte[] tabByte = wb.UploadValues(url, n);
                string ticket = UnicodeEncoding.UTF8.GetString(tabByte);
                this.laSecretaire.ticket = ticket.Substring(2);//methode qui enleve les 2 derniers caracteres

                MessageBox.Show("Vous avez bien ajouté la famille");


            }
            catch (WebException exc)
            {
                if (exc.Response is HttpWebResponse)
                    MessageBox.Show(((HttpWebResponse)exc.Response).StatusCode.ToString());
            }
        }
    }
}

