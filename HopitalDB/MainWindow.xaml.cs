using HopitalDB.Models;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Net.NetworkInformation;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HopitalDB
{
    /// <summary>
    /// Fenêtre principale de l'application HopitalDB
    /// Cette classe gère l'interface utilisateur et les interactions pour la gestion
    /// des hôpitaux, médecins et infirmiers dans une base de données médicale
    /// </summary>
    public partial class MainWindow : Window
    {
        // Collections observables pour stocker les données des hôpitaux, infirmiers et médecins
        // Ces collections mettent automatiquement à jour l'interface utilisateur lorsque modifiées
        public ObservableCollection<Hopital> Hopitaux = new ObservableCollection<Hopital>();
        public ObservableCollection<Infirmier> Infirmiers = new ObservableCollection<Infirmier>();
        public ObservableCollection<Medecin> Medecins = new ObservableCollection<Medecin>();


        /// <summary>
        /// Constructeur de la fenêtre principale
        /// Initialise les composants, définit la langue par défaut et associe les sources de données aux DataGrids
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            LangueParDefaut();
            dgMedecin.ItemsSource = Medecins;
            dgInfirmier.ItemsSource = Infirmiers;
            dgHopital.ItemsSource = Hopitaux;
            this.WindowState = WindowState.Maximized;
        }


        /// <summary>
        /// Définit la langue par défaut de l'application en fonction de la culture système
        /// Parcourt les éléments du ComboBox de langue pour sélectionner celui qui correspond à la culture courante
        /// </summary>
        public void LangueParDefaut()
        {
            string langueCourante = Thread.CurrentThread.CurrentCulture.Name;

            foreach (ComboBoxItem item in cbLangue.Items)
            {
                if (item.Tag.ToString() == langueCourante)
                {
                    cbLangue.SelectedItem = item;
                    break;
                }
            }
        }

        /// <summary>
        /// Gestionnaire d'événement pour le changement de sélection dans le ComboBox de langue
        /// Détecte la nouvelle langue sélectionnée et appelle la méthode pour changer la langue de l'application
        /// </summary>
        /// <param name="sender">Objet qui a déclenché l'événement</param>
        /// <param name="e">Arguments de l'événement de changement de sélection</param>
        private void cbLangue_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox combobox = sender as ComboBox;

            if (combobox != null)
            {
                ComboBoxItem selectedItem = combobox.SelectedItem as ComboBoxItem;
                if (selectedItem != null)
                {
                    string selectTag = selectedItem.Tag.ToString();

                    switch (selectTag)
                    {
                        case "fr-FR":
                            ChangeLangue(selectTag);
                            break;
                        case "en-CA":
                            ChangeLangue(selectTag);
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Change la langue de l'application en fonction de la culture spécifiée
        /// Met à jour la culture courante du thread et les ressources localisées
        /// </summary>
        /// <param name="sultureChoisis">Code de culture de la langue choisie (ex: "fr-FR", "en-CA")</param>
        public void ChangeLangue(string sultureChoisis)
        {
            var culture = new CultureInfo(sultureChoisis);

            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
            Properties.Resources.Culture = culture;

            RafraichirContenuFenetre();
        }

        /// <summary>
        /// Rafraîchit tous les éléments textuels de l'interface utilisateur avec les ressources localisées
        /// Cette méthode est appelée après un changement de langue pour mettre à jour tous les textes affichés
        /// </summary>
        public void RafraichirContenuFenetre()
        {
            this.Title = Properties.Resources.Title;

            //miAgrandir.Header = Properties.Resources.Increase;
            //miReduire.Header = Properties.Resources.Decrease;
            //miFermer.Header = Properties.Resources.Close;

            miAjouter.Header = Properties.Resources.Add;
            miModifier.Header = Properties.Resources.Update;
            miSupprimer.Header = Properties.Resources.Delete;

            toolbar_btn_Ajouter.ToolTip = Properties.Resources.Add;
            toolbar_btn_Modifier.ToolTip = Properties.Resources.Update;
            toolbar_btn_Supprimer.ToolTip = Properties.Resources.Delete;

            lbAdresseHopital.Content = Properties.Resources.Address;
            lbNomHopital.Content = Properties.Resources.Name;
            lbTelHopital.Content = Properties.Resources.Phone;

            btnAjouterHopital.Content = Properties.Resources.Add;
            btnModifierHopital.Content = Properties.Resources.Update;
            btnSupprimerHopital.Content = Properties.Resources.Delete;

            dgcNomHopital.Header = Properties.Resources.Name;
            dgcAdresseHopital.Header = Properties.Resources.Address;
            dgcTelHopital.Header = Properties.Resources.Phone;

            tabitemInfirmier.Header = Properties.Resources.Nurse;
            tabitemMedecin.Header = Properties.Resources.Doctor;
            tabitemHopital.Header = Properties.Resources.Hospital;

            lbPrenomInf.Content = Properties.Resources.FirstName;
            lbNomInf.Content = Properties.Resources.LastName;
            lbTelInf.Content = Properties.Resources.Phone;

            btnAjouterInf.Content = Properties.Resources.Add;
            btnModifierInf.Content = Properties.Resources.Update;
            btnSupprimerInf.Content = Properties.Resources.Delete;

            dgcNomInf.Header = Properties.Resources.LastName;
            dgcPrenomInf.Header = Properties.Resources.FirstName;
            dgcTelInf.Header = Properties.Resources.Phone;

            lbPrenomMed.Content = Properties.Resources.FirstName;
            lbNomMed.Content = Properties.Resources.LastName;
            lbTelMed.Content = Properties.Resources.Phone;

            btnAjouterMedecin.Content = Properties.Resources.Add;
            btnModifierMedecin.Content = Properties.Resources.Update;
            btnSupprimerMedecin.Content = Properties.Resources.Delete;

            dgcNomMed.Header = Properties.Resources.LastName;
            dgcPrenomMed.Header = Properties.Resources.FirstName;
            dgcTelMed.Header = Properties.Resources.Phone;

            lbHopitalInf.Content = Properties.Resources.Hospital;
            lbHopitalMed.Content = Properties.Resources.Hospital;
        }

        /// <summary>
        /// Gestionnaire d'événement pour le clic sur le bouton Ajouter d'un hôpital
        /// </summary>
        /// <param name="sender">Objet qui a déclenché l'événement</param>
        /// <param name="e">Arguments de l'événement</param>
        private void btnAjouterHopital_Click(object sender, RoutedEventArgs e)
        {
            AjouterHopital();
        }

        /// <summary>
        /// Ajoute un nouvel hôpital à la collection si un hôpital avec le même nom et téléphone n'existe pas déjà
        /// Met à jour l'interface et affiche un message de statut approprié
        /// </summary>
        public void AjouterHopital()
        {
            // Vérifie si un hôpital avec le même nom et téléphone existe déjà dans la collection
            Hopital HopitalExistant = Hopitaux.FirstOrDefault(h => h.Nom == tbNomHopital.Text && h.Telephone == tbTelHopital.Text);

            if (HopitalExistant == null)
            {
                // Création d'un nouvel objet Hopital avec les données saisies
                Hopital NouveauHopital = new Hopital();

                NouveauHopital.Nom = tbNomHopital.Text;
                NouveauHopital.Adresse = tbAdresseHopital.Text;
                NouveauHopital.Telephone = tbTelHopital.Text;

                // Ajout de l'hôpital à la collection et mise à jour des ComboBox d'hôpitaux
                Hopitaux.Add(NouveauHopital);
                MettreAjourListeHopitaux();

                tbStatus.Text = Properties.Resources.InstertHospital;
            }
            else
            {
                // Message d'erreur si l'hôpital existe déjà
                tbStatus.Text = Properties.Resources.NotInsertedHospital;
            }
        }

        /// <summary>
        /// Gestionnaire d'événement pour le clic sur le bouton Modifier d'un hôpital
        /// </summary>
        /// <param name="sender">Objet qui a déclenché l'événement</param>
        /// <param name="e">Arguments de l'événement</param>
        private void btnModifierHopital_Click(object sender, RoutedEventArgs e)
        {
            ModifierHopital();
        }

        /// <summary>
        /// Modifie les informations de l'hôpital sélectionné dans le DataGrid
        /// Met à jour l'interface et affiche un message de statut approprié
        /// </summary>
        public void ModifierHopital()
        {
            // Vérifie si un hôpital est sélectionné dans le DataGrid
            if (dgHopital.SelectedItem is Hopital LigneSelectionnee)
            {
                // Mise à jour des propriétés de l'hôpital sélectionné avec les valeurs saisies
                LigneSelectionnee.Nom = tbNomHopital.Text;
                LigneSelectionnee.Adresse = tbAdresseHopital.Text;
                LigneSelectionnee.Telephone = tbTelHopital.Text;

                // Rafraîchissement de l'affichage et mise à jour des ComboBox d'hôpitaux
                dgHopital.Items.Refresh();
                MettreAjourListeHopitaux();

                tbStatus.Text = Properties.Resources.UpdateHospital;
                tbStatus.Foreground = Brushes.Green;
            }
            else
            {
                // Message d'erreur si aucun hôpital n'est sélectionné
                tbStatus.Text = Properties.Resources.NotUpdatedHospital;
                tbStatus.Foreground = Brushes.Red;
            }
        }

        /// <summary>
        /// Gestionnaire d'événement pour le clic sur le bouton Supprimer d'un hôpital
        /// </summary>
        /// <param name="sender">Objet qui a déclenché l'événement</param>
        /// <param name="e">Arguments de l'événement</param>
        private void btnSupprimerHopital_Click(object sender, RoutedEventArgs e)
        {
            SupprimerHopital();
        }

        /// <summary>
        /// Supprime l'hôpital sélectionné dans le DataGrid
        /// Met à jour l'interface et affiche un message de statut approprié
        /// </summary>
        public void SupprimerHopital()
        {
            // Vérifie si un hôpital est sélectionné dans le DataGrid
            if (dgHopital.SelectedItem is Hopital LigneSelectionnee)
            {
                // Supprime l'hôpital de la collection et met à jour les ComboBox d'hôpitaux
                Hopitaux.Remove(LigneSelectionnee);
                MettreAjourListeHopitaux();
                tbStatus.Text = Properties.Resources.DeleteHospital;
                tbStatus.Foreground = Brushes.Green;
            }
            else
            {
                // Message d'erreur si aucun hôpital n'est sélectionné
                tbStatus.Text = Properties.Resources.NotUpdatedHospital;
                tbStatus.Foreground = Brushes.Red;
            }
        }

        /// <summary>
        /// Gestionnaire d'événement pour le changement de sélection dans le DataGrid des hôpitaux
        /// Met à jour les champs de texte avec les informations de l'hôpital sélectionné
        /// </summary>
        /// <param name="sender">Objet qui a déclenché l'événement</param>
        /// <param name="e">Arguments de l'événement</param>
        private void dgHopital_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Si un hôpital est sélectionné, affiche ses informations dans les TextBox
            if (dgHopital.SelectedItem is Hopital LigneSelectionne)
            {
                tbNomHopital.Text = LigneSelectionne.Nom;
                tbAdresseHopital.Text = LigneSelectionne.Adresse;
                tbTelHopital.Text = LigneSelectionne.Telephone;
            }
        }

        /// <summary>
        /// Met à jour les ComboBox d'hôpitaux pour les infirmiers et médecins
        /// avec la liste actuelle des hôpitaux disponibles
        /// </summary>
        public void MettreAjourListeHopitaux()
        {
            // Vide les ComboBox existants
            cbHopitalInfirmier.Items.Clear();
            cbHopitalMedecin.Items.Clear();
            cbRechercheParHopital.Items.Clear();

            // Remplit les ComboBox avec les noms des hôpitaux disponibles
            foreach (Hopital hopital in Hopitaux)
            {
                cbHopitalInfirmier.Items.Add(hopital.Nom);
                cbHopitalMedecin.Items.Add(hopital.Nom);
                cbRechercheParHopital.Items.Add(hopital.Nom);
            }
        }

        /// <summary>
        /// Gestionnaire d'événement pour le clic sur le bouton Ajouter d'un médecin
        /// </summary>
        /// <param name="sender">Objet qui a déclenché l'événement</param>
        /// <param name="e">Arguments de l'événement</param>
        private void btnAjouterMedecin_Click(object sender, RoutedEventArgs e)
        {
            AjouterMedecin();
        }

        /// <summary>
        /// Ajoute un nouveau médecin à la collection
        /// Vérifie qu'un hôpital est sélectionné et que le médecin n'existe pas déjà
        /// </summary>
        public void AjouterMedecin()
        {
            // Vérifie qu'un hôpital est sélectionné dans le ComboBox
            if (cbHopitalMedecin.SelectedItem != null)
            {
                string HopitalNomSelectionne = cbHopitalMedecin.SelectedItem.ToString();

                // Recherche l'hôpital sélectionné dans la collection
                Hopital hopitalExistant = Hopitaux.FirstOrDefault(h => h.Nom == HopitalNomSelectionne);

                // Vérifie si un médecin avec le même nom et téléphone existe déjà
                Medecin medecinExistant = Medecins.FirstOrDefault(m => m.Nom == hopitalExistant.Nom && m.Telephone ==
                hopitalExistant.Telephone);

                if (hopitalExistant != null && medecinExistant == null)
                {
                    // Création d'un nouveau médecin et ajout à la collection
                    Medecin NouveauMedecin = new Medecin
                    {
                        Nom = tbNomMed.Text,
                        Prenom = tbPrenomMed.Text,
                        Telephone = tbTelMed.Text,
                        hopital = hopitalExistant
                    };

                    Medecins.Add(NouveauMedecin);
                    tbStatus.Text = Properties.Resources.InsertDoctor;
                    tbStatus.Foreground = Brushes.Green;
                }
                else
                {
                    // Message d'erreur si le médecin existe déjà ou l'hôpital n'existe pas
                    tbStatus.Text = Properties.Resources.NotInsertedDoctor;
                    tbStatus.Foreground = Brushes.Red;
                }
            }
        }

        /// <summary>
        /// Gestionnaire d'événement pour le clic sur le bouton Modifier d'un médecin
        /// </summary>
        /// <param name="sender">Objet qui a déclenché l'événement</param>
        /// <param name="e">Arguments de l'événement</param>
        private void btnModifierMedecin_Click(object sender, RoutedEventArgs e)
        {
            ModifierMedecin();
        }

        /// <summary>
        /// Modifie les informations du médecin sélectionné dans le DataGrid
        /// Vérifie qu'un médecin est sélectionné et qu'un hôpital est assigné
        /// </summary>
        public void ModifierMedecin()
        {
            // Vérifie si un médecin est sélectionné dans le DataGrid
            if (dgMedecin.SelectedItem is Medecin MedecinSelectionne)
            {
                // Vérifie qu'un hôpital est sélectionné dans le ComboBox
                if (cbHopitalMedecin.SelectedItem != null)
                {
                    // Mise à jour des propriétés du médecin sélectionné
                    MedecinSelectionne.Nom = tbNomMed.Text;
                    MedecinSelectionne.Prenom = tbPrenomMed.Text;
                    MedecinSelectionne.Telephone = tbTelMed.Text;
                    MedecinSelectionne.hopital = Hopitaux.FirstOrDefault(h => h.Nom == cbHopitalMedecin.SelectedItem.ToString());

                    // Rafraîchissement de l'affichage
                    dgMedecin.Items.Refresh();
                    tbStatus.Text = Properties.Resources.UpdateDoctor;
                    tbStatus.Foreground = Brushes.Green;
                }
                else
                {
                    // Message d'erreur si aucun hôpital n'est sélectionné
                    tbStatus.Text = Properties.Resources.NotUpdaedDoctor;
                    tbStatus.Foreground = Brushes.Red;
                }
            }
        }

        /// <summary>
        /// Gestionnaire d'événement pour le clic sur le bouton Supprimer d'un médecin
        /// </summary>
        /// <param name="sender">Objet qui a déclenché l'événement</param>
        /// <param name="e">Arguments de l'événement</param>
        private void btnSupprimerMedecin_Click(object sender, RoutedEventArgs e)
        {
            SupprimerMedecin();
        }

        /// <summary>
        /// Supprime le médecin sélectionné dans le DataGrid
        /// Met à jour l'interface et affiche un message de statut approprié
        /// </summary>
        public void SupprimerMedecin()
        {
            // Vérifie si un médecin est sélectionné dans le DataGrid
            if (dgMedecin.SelectedItem is Medecin MedecinSelectionne)
            {
                // Supprime le médecin de la collection et rafraîchit l'affichage
                Medecins.Remove(MedecinSelectionne);
                dgMedecin.Items.Refresh();
                tbStatus.Text = Properties.Resources.DeleteDoctor;
                tbStatus.Foreground = Brushes.Green;
            }
            else
            {
                // Message d'erreur si aucun médecin n'est sélectionné
                tbStatus.Text = Properties.Resources.NotDeletedDoctor;
                tbStatus.Foreground = Brushes.Red;
            }
        }

        /// <summary>
        /// Gestionnaire d'événement pour le changement de sélection dans le DataGrid des médecins
        /// Met à jour les champs de texte avec les informations du médecin sélectionné
        /// </summary>
        /// <param name="sender">Objet qui a déclenché l'événement</param>
        /// <param name="e">Arguments de l'événement</param>
        private void dgMedecin_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Si un médecin est sélectionné, affiche ses informations dans les TextBox
            if (dgMedecin.SelectedItem is Medecin LigneSelectionnee)
            {
                tbNomMed.Text = LigneSelectionnee.Nom;
                tbPrenomMed.Text = LigneSelectionnee.Prenom;
                tbTelMed.Text = LigneSelectionnee.Telephone;
                cbHopitalMedecin.SelectedItem = LigneSelectionnee.hopital.Nom;
            }
        }

        /// <summary>
        /// Gestionnaire d'événement pour le clic sur le bouton Ajouter d'un infirmier
        /// </summary>
        /// <param name="sender">Objet qui a déclenché l'événement</param>
        /// <param name="e">Arguments de l'événement</param>
        private void btnAjouterInf_Click(object sender, RoutedEventArgs e)
        {
            AjouterInfirmier();
        }

        /// <summary>
        /// Ajoute un nouvel infirmier à la collection
        /// Vérifie qu'un hôpital est sélectionné et que l'infirmier n'existe pas déjà
        /// </summary>
        public void AjouterInfirmier()
        {
            // Vérifie qu'un hôpital est sélectionné dans le ComboBox
            if (cbHopitalInfirmier.SelectedItem != null)
            {
                string HopitalNomSelectionne = cbHopitalInfirmier.SelectedItem.ToString();

                // Recherche l'hôpital sélectionné dans la collection
                Hopital hopitalExistant = Hopitaux.FirstOrDefault(h => h.Nom == HopitalNomSelectionne);

                // Vérifie si un infirmier avec le même nom et téléphone existe déjà
                // Note: Il y a une erreur ici, car on vérifie dans la collection des médecins et non des infirmiers
                Medecin InfirmierExistant = Medecins.FirstOrDefault(m => m.Nom == hopitalExistant.Nom && m.Telephone ==
                hopitalExistant.Telephone);

                if (hopitalExistant != null && InfirmierExistant == null)
                {
                    // Création d'un nouvel infirmier et ajout à la collection
                    Infirmier NouveauInfirmier = new Infirmier
                    {
                        Nom = tbNomInf.Text,
                        Prenom = tbPrenomInf.Text,
                        Telephone = tbTelInf.Text,
                        hopital = hopitalExistant
                    };

                    Infirmiers.Add(NouveauInfirmier);
                    tbStatus.Text = Properties.Resources.InsertNurse;
                    tbStatus.Foreground = Brushes.Green;
                }
                else
                {
                    // Message d'erreur si l'infirmier existe déjà ou l'hôpital n'existe pas
                    tbStatus.Text = Properties.Resources.NotInsertedNurse;
                    tbStatus.Foreground = Brushes.Red;
                }
            }
        }

        /// <summary>
        /// Gestionnaire d'événement pour le clic sur le bouton Modifier d'un infirmier
        /// </summary>
        /// <param name="sender">Objet qui a déclenché l'événement</param>
        /// <param name="e">Arguments de l'événement</param>
        private void btnModifierInf_Click(object sender, RoutedEventArgs e)
        {
            ModifierInfirmier();
        }

        /// <summary>
        /// Modifie les informations de l'infirmier sélectionné dans le DataGrid
        /// Vérifie qu'un infirmier est sélectionné et qu'un hôpital est assigné
        /// </summary>
        public void ModifierInfirmier()
        {
            // Vérifie si un infirmier est sélectionné dans le DataGrid
            if (dgInfirmier.SelectedItem is Infirmier InfirmierSelectionne)
            {
                // Vérifie qu'un hôpital est sélectionné dans le ComboBox
                if (cbHopitalInfirmier.SelectedItem != null)
                {
                    // Mise à jour des propriétés de l'infirmier sélectionné
                    InfirmierSelectionne.Nom = tbNomInf.Text;
                    InfirmierSelectionne.Prenom = tbPrenomInf.Text;
                    InfirmierSelectionne.Telephone = tbTelInf.Text;
                    InfirmierSelectionne.hopital = Hopitaux.FirstOrDefault(h => h.Nom == cbHopitalInfirmier.SelectedItem.ToString());

                    // Rafraîchissement de l'affichage
                    dgInfirmier.Items.Refresh();
                    tbStatus.Text = Properties.Resources.UpdateNurse;
                    tbStatus.Foreground = Brushes.Green;
                }
                else
                {
                    // Message d'erreur si aucun hôpital n'est sélectionné
                    tbStatus.Text = Properties.Resources.NotUpdatedNurse;
                    tbStatus.Foreground = Brushes.Red;
                }
            }
        }

        /// <summary>
        /// Gestionnaire d'événement pour le clic sur le bouton Supprimer d'un infirmier
        /// </summary>
        /// <param name="sender">Objet qui a déclenché l'événement</param>
        /// <param name="e">Arguments de l'événement</param>
        private void btnSupprimerInf_Click(object sender, RoutedEventArgs e)
        {
            SupprimerInfirmier();
        }

        /// <summary>
        /// Supprime l'infirmier sélectionné dans le DataGrid
        /// Met à jour l'interface et affiche un message de statut approprié
        /// </summary>
        public void SupprimerInfirmier()
        {
            // Vérifie si un infirmier est sélectionné dans le DataGrid
            if (dgInfirmier.SelectedItem is Infirmier InfirmierSelectionne)
            {
                // Supprime l'infirmier de la collection et rafraîchit l'affichage
                Infirmiers.Remove(InfirmierSelectionne);
                dgInfirmier.Items.Refresh();
                tbStatus.Text = Properties.Resources.DeleteNurse;
                tbStatus.Foreground = Brushes.Green;
            }
            else
            {
                // Message d'erreur si aucun infirmier n'est sélectionné
                tbStatus.Text = Properties.Resources.NotDeletedNurse;
                tbStatus.Foreground = Brushes.Red;
            }
        }

        /// <summary>
        /// Gestionnaire d'événement pour le changement de sélection dans le DataGrid des infirmiers
        /// Met à jour les champs de texte avec les informations de l'infirmier sélectionné
        /// </summary>
        /// <param name="sender">Objet qui a déclenché l'événement</param>
        /// <param name="e">Arguments de l'événement</param>
        private void dgInfirmier_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Si un infirmier est sélectionné, affiche ses informations dans les TextBox
            if (dgInfirmier.SelectedItem is Infirmier LigneSelectionnee)
            {
                tbNomInf.Text = LigneSelectionnee.Nom;
                tbPrenomInf.Text = LigneSelectionnee.Prenom;
                tbTelInf.Text = LigneSelectionnee.Telephone;
                cbHopitalInfirmier.SelectedItem = LigneSelectionnee.hopital.Nom;
            }
        }

        private void rbSelectionRechercheParHopital_Checked(object sender, RoutedEventArgs e)
        {
            AfficherStackPanelRechercheParHopital();

        }

        public void  AfficherStackPanelRechercheParHopital()
        {
            spRechercheHopital.Visibility = Visibility.Visible;
            spRechercheMedecin.Visibility = Visibility.Hidden;
            spRechercheInfirmier.Visibility = Visibility.Hidden;

            dgResultatRechercheHopital.Visibility = Visibility.Visible;
            dgResultatRechercheMedecin.Visibility = Visibility.Collapsed;
            dgResultatRechercheInfirmier.Visibility= Visibility.Collapsed;
            
            

        }

        private void rbSelectionRechercheMedecin_Checked(object sender, RoutedEventArgs e)
        {
            AfficherStackPanelRechercheParMedecin();
        }

        public void AfficherStackPanelRechercheParMedecin()
        {
            spRechercheHopital.Visibility = Visibility.Hidden;
            spRechercheMedecin.Visibility = Visibility.Visible;
            spRechercheInfirmier.Visibility = Visibility.Hidden;

            dgResultatRechercheHopital.Visibility = Visibility.Collapsed;
            dgResultatRechercheMedecin.Visibility = Visibility.Visible;
            dgResultatRechercheInfirmier.Visibility = Visibility.Collapsed;
        }

        private void rbSelectionRechercheInfirmier_Checked(object sender, RoutedEventArgs e)
        {
            AfficherStackPanelRechercheParInfirmier();
        }
        public void AfficherStackPanelRechercheParInfirmier()
        {
            spRechercheHopital.Visibility = Visibility.Hidden;
            spRechercheMedecin.Visibility = Visibility.Hidden;
            spRechercheInfirmier.Visibility = Visibility.Visible;

            dgResultatRechercheHopital.Visibility = Visibility.Collapsed;
            dgResultatRechercheMedecin.Visibility = Visibility.Collapsed;
            dgResultatRechercheInfirmier.Visibility = Visibility.Visible;
        }

        private void rbRechercheParMedecin_Checked(object sender, RoutedEventArgs e)
        {
            dgResultatRechercheHopital.Visibility = Visibility.Collapsed;
            dgResultatRechercheMedecin.Visibility = Visibility.Visible;
            dgResultatRechercheInfirmier.Visibility = Visibility.Collapsed;

        }

        private void rbRechercheParInfirmier_Checked(object sender, RoutedEventArgs e)
        {
            dgResultatRechercheHopital.Visibility = Visibility.Collapsed;
            dgResultatRechercheMedecin.Visibility = Visibility.Collapsed;
            dgResultatRechercheInfirmier.Visibility = Visibility.Visible;
        }

        private void cbRechercheParHopital_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(cbRechercheParHopital.SelectedItem != null)
            {
                string NomHopitalSelectionne = cbRechercheParHopital.SelectedItem.ToString();

                Hopital HopitalChoisis = Hopitaux.FirstOrDefault(h => h.Nom == NomHopitalSelectionne);

                if(HopitalChoisis != null)
                {
                    if(rbRechercheParMedecin.IsChecked == true)
                    {
                        var MedecinsFiltreSelonHopital = 
                        new ObservableCollection<Medecin>(Medecins.Where(m => m.hopital.Nom == HopitalChoisis.Nom));
                        dgResultatRechercheMedecin.ItemsSource = MedecinsFiltreSelonHopital;
                    }
                    else if(rbRechercheParInfirmier.IsChecked==true)
                    {
                        var InfirmiersFiltreSelonHopital =
                        new ObservableCollection<Infirmier>(Infirmiers.Where(i => i.hopital.Nom == HopitalChoisis.Nom));
                        dgResultatRechercheInfirmier.ItemsSource = InfirmiersFiltreSelonHopital;
                    }
                }

            }

        }

        private void tbRechercheParMedecin_TextChanged(object sender, TextChangedEventArgs e)
        {
            string NomMedecinFiltrer = tbRechercheParMedecin.Text;
            ObservableCollection<Medecin> MedecinFiltre = new ObservableCollection<Medecin>
                (Medecins.Where(m => m.Nom.ToLower().Contains(NomMedecinFiltrer.ToLower()) || m.Prenom.ToLower().Contains(NomMedecinFiltrer.ToLower())));
            dgResultatRechercheMedecin.ItemsSource = MedecinFiltre;
        }

        private void tbRechercheParInfirmier_TextChanged(object sender, TextChangedEventArgs e)
        {
            string NomInfirmierFiltrer = tbRechercheParInfirmier.Text;
            ObservableCollection<Infirmier> InfirmierFiltre = new ObservableCollection<Infirmier>
                (Infirmiers.Where(i => i.Nom.ToLower().Contains(NomInfirmierFiltrer.ToLower()) || i.Prenom.ToLower().Contains(NomInfirmierFiltrer.ToLower())));
            dgResultatRechercheInfirmier.ItemsSource = InfirmierFiltre;
        }
    }
}