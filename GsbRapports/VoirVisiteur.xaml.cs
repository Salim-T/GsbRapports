﻿using System;
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
    /// Logique d'interaction pour VoirVisiteur.xaml
    /// </summary>
    public partial class VoirVisiteur : Window
    {
        private WebClient wb;
        private string site;
        private Secretaire laSecretaire;
        public VoirVisiteur(Secretaire laSecretaire, WebClient wb, string site)
        {
            InitializeComponent();
            this.laSecretaire = laSecretaire;
            this.site = site;
            this.wb = wb;
            string url = this.site + "visiteurs?ticket=" + this.laSecretaire.getHashTicketMdp(); //retourne le mdp hashé
            string reponse = this.wb.DownloadString(url);
            dynamic d = JsonConvert.DeserializeObject(reponse);
            string visiteurs = d.visiteurs.ToString();
            string ticket = d.ticket;
            this.laSecretaire.ticket = ticket;
            List<Visiteur> l = JsonConvert.DeserializeObject<List<Visiteur>>(visiteurs);
            this.dtgVsiteur.ItemsSource = l;
        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            AjoutVisiteurWindow w = new AjoutVisiteurWindow(this.laSecretaire, this.wb, this.site);
            w.Show();
        }

        private void MenuItem_Click_4(object sender, RoutedEventArgs e)
        {
            modifierVisiteursWindow w = new modifierVisiteursWindow(this.laSecretaire, this.wb, this.site);
            w.Show();
        }

        private void ButtonAnnuler_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        
    }
}
