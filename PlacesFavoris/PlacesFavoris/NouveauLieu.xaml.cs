using PlacesFavoris.Helper;
using PlacesFavoris.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PlacesFavoris
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NouveauLieu : ContentPage
    {
        private Lieu _lieu;

        // Mappage entre les membres de l'enum et les strings affichés
        // (pour gérer les accents dans le picker)
        private Dictionary<string, Categorie> categorieDisplayNames = new Dictionary<string, Categorie>
        {
            { "Connue" , Categorie.Connue },
            { "Souhaitée" , Categorie.Souhaitee },
            { "Visitée", Categorie.Visitee }
        };

        public NouveauLieu()
        {
            InitializeComponent();
        }

        public NouveauLieu(Lieu lieu)
        {
            InitializeComponent();
            Title = $"Modifier le lieu";
            _lieu = lieu;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            //pickCategorie.ItemsSource = Enum.GetValues(typeof(Categorie)).Cast<Categorie>().ToList();
            pickCategorie.ItemsSource = categorieDisplayNames.Keys.ToList();

            if (_lieu != null)
            {
                titrePage.Text = $"{_lieu.Nom}";

                txtNomPlace.Text = _lieu.Nom;
                txtAdresse.Text = _lieu.Adresse;
                pickCategorie.SelectedItem = categorieDisplayNames.FirstOrDefault(c => c.Value == _lieu.Categorie).Key;
                txtLatitude.Text = _lieu.Latitude.ToString();
                txtLongitude.Text = _lieu.Longitude.ToString();

                btnSupprimer.IsVisible = true;
            }
        }

        private async void BtnEnregistrer_Clicked(object sender, EventArgs e)
        {
            // Vérifier le nom de la place et l'adresse
            if (string.IsNullOrEmpty(txtNomPlace.Text) || string.IsNullOrEmpty(txtAdresse.Text))
            {
                await DisplayAlert("Alerte", "Veuillez svp saisir le nom de la place et l'adresse correspondante", "Fermer");
                return;
            }

            // Vérifier la catégorie
            if (pickCategorie.SelectedItem == null)
            {
                await DisplayAlert("Alerte", "Veuille svp choisir une catégorie", "Fermer");
                return;
            }

            // Vérifier les coordonnées saisies
            if (string.IsNullOrEmpty(txtLatitude.Text) || string.IsNullOrEmpty(txtLongitude.Text))
            {
                await DisplayAlert("Alerte", "Veuillez svp saisir les coordonnées (lat. et long.) de la place", "Fermer");
                return;
            }

            var nomPlace = txtNomPlace.Text;
            var adresse = txtAdresse.Text;
            var categorieChoisie = categorieDisplayNames[pickCategorie.SelectedItem.ToString()];
            var latitudeSaisieConvertie = double.TryParse(txtLatitude.Text, out double latitudeSaisie);
            var longitudeSaisieConvertie = double.TryParse(txtLongitude.Text, out double longitudeSaisie);

            // Vérifier le format de la latitude et la longitude
            if (!latitudeSaisieConvertie)
            {
                await DisplayAlert("Alerte", "Veuillez svp vérifier le format de la latitude saisie", "Fermer");
                return;
            }

            if (!longitudeSaisieConvertie)
            {
                await DisplayAlert("Alerte", "Veuillez svp vérifier le format de la longitude saisie", "Fermer");
                return;
            }

            // Enregistrer dans la BD
            using (var conn = new SQLiteConnection(App.CheminBD))
            {
                var idUtilisateurAuthentifie = Auth.RetournerIdentifiantUtilisateur();

                var placesFavoris = conn.Table<Lieu>().ToList().FindAll(
                    l => l.IdUtilisateur == idUtilisateurAuthentifie);

                var configurations = conn.Table<Configuration>().ToList().Find(
                c => c.IdUtilisateur == idUtilisateurAuthentifie);

                if (_lieu == null)
                {
                    // Valider l'existance du la place selon les coordonnées (nom, lat., long.)
                    var exist = placesFavoris.Any(p => p.Nom == nomPlace &&
                                                    p.Latitude == latitudeSaisie &&
                                                    p.Longitude == longitudeSaisie);
                    if (exist)
                    {
                        await DisplayAlert("Alerte", "Il existe déjà un lieux avec le même nom et les mêmes coordonnés", "Fermer");
                        return;
                    }

                    // Créer un nouveau lieu
                    var nouveauLieu = new Lieu();

                    nouveauLieu.IdUtilisateur = idUtilisateurAuthentifie;
                    nouveauLieu.Nom = nomPlace;
                    nouveauLieu.Adresse = adresse;
                    nouveauLieu.Categorie = categorieChoisie;
                    nouveauLieu.Latitude = latitudeSaisie;
                    nouveauLieu.Longitude = longitudeSaisie;

                    getUserSwitches(nouveauLieu, configurations);

                    conn.Insert(nouveauLieu);
                    await DisplayAlert("Alerte", $"`{nouveauLieu.Nom}` a été enregistrée dans la liste des favoris !", "Fermer");
                    await Navigation.PopAsync();
                }
                else
                {
                    // Mettre à jour les informations du lieu à modifier
                    _lieu.Nom = nomPlace;
                    _lieu.Adresse = adresse;
                    _lieu.Categorie = categorieChoisie;
                    _lieu.Latitude = latitudeSaisie;
                    _lieu.Longitude = longitudeSaisie;

                    getUserSwitches(_lieu, configurations);

                    var lieuModifie = await DisplayAlert("Alerte", $"Voulez-vous enregistrer les modifications apportées ?",
                        "Enregistrer", "Annuler");

                    if (lieuModifie)
                    {
                        conn.Update(_lieu);
                        await DisplayAlert("Succès", $"Le lieu a été modifiée avec succès !", "Fermer");
                        await Navigation.PopAsync();
                    }
                }
            }
        }

        // Supprimer un lieu de la liste des favoris
        private async void BtnSupprimer_Clicked(Object sender, EventArgs e)
        {
            var supprimerLieuConfirme = await DisplayAlert("Alert", $"Êtes-vous sûr de " +
                $"supprimer `{_lieu.Nom}` ?", "Confirmer", "Annuler");

            if (supprimerLieuConfirme)
            {
                using (var conn = new SQLiteConnection(App.CheminBD))
                {
                    conn.Delete(_lieu);
                }

                await Navigation.PopAsync();
            }
        }

        // Mettre en place les bonnes configurations correspondantes des switchs de l'utilisateur correspondant
        private void getUserSwitches(Lieu lieu, Configuration config)
        {
            switch (lieu.Categorie)
            {
                case Categorie.Connue:
                    lieu.IsCategorieToggled = config.IsSwitchLieuxConnusToggled;
                    break;
                case Categorie.Souhaitee:
                    lieu.IsCategorieToggled = config.IsSwitchLieuxSouhaitesToggled;
                    break;
                case Categorie.Visitee:
                    lieu.IsCategorieToggled = config.IsSwitchLieuxVisitesToggled;
                    break;
                default:
                    lieu.IsCategorieToggled = true;
                    break;
            }
        }
    }
}