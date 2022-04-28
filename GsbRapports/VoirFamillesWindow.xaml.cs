using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
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

namespace GsbRapports
{
    /// <summary>
    /// Logique d'interaction pour VoirFamillesWindow.xaml
    /// </summary>
    public partial class VoirFamillesWindow : Window
    {

        private WebClient wb;
        private string site;
        private Secretaire laSecretaire;

        public VoirFamillesWindow(Secretaire laSecretaire, WebClient wb, string site)
        {
            InitializeComponent();

            this.laSecretaire = laSecretaire;
            this.site = site;
            this.wb = wb;
            string url = this.site + "familles?ticket=" + this.laSecretaire.getHashTicketMdp(); //retourne le mdp hashé
            string reponse = this.wb.DownloadString(url);
            dynamic d = JsonConvert.DeserializeObject(reponse);
            string familles = d.familles.ToString();
            string ticket=d.ticket;
            this.laSecretaire.ticket = ticket;
            List<Famille> l = JsonConvert.DeserializeObject<List<Famille>>(familles);
            this.dtgfamilles.ItemsSource = l; //this is how you bind using XAML (look at the Xaml file) binding sert pour afficher le contenu de famille

        }
    }
}
