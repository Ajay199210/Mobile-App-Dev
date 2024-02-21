using PlacesFavoris.Helper;
using PlacesFavoris.Models;
using SQLite;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PlacesFavoris
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Configurations : ContentPage
    {
        public Configurations()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var idUtilisateur = Auth.RetournerIdentifiantUtilisateur();

            // Afficher les configurations sauvegardées de l'utilisateur
            using (var conn = new SQLiteConnection(App.CheminBD))
            {
                var configUtilisateur = conn.Table<Configuration>().ToList().Find(c => c.IdUtilisateur == idUtilisateur);
                switchPlacesConnues.IsToggled = configUtilisateur.IsSwitchLieuxConnusToggled;
                switchPlacesVisitees.IsToggled = configUtilisateur.IsSwitchLieuxVisitesToggled;
                switchPlacesSouhaitees.IsToggled = configUtilisateur.IsSwitchLieuxSouhaitesToggled;
                degreLatitude.Text = configUtilisateur.DegreLatitude.ToString();
                degreLongitude.Text = configUtilisateur.DegreLongitude.ToString();
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

        private void OnToggled_PlacesConnues(object sender, EventArgs e)
        {
            var idUtilisateur = Auth.RetournerIdentifiantUtilisateur();

            using (var conn = new SQLiteConnection(App.CheminBD))
            {
                var lieuxConnus = conn.Table<Lieu>().ToList().FindAll(
                l => l.Categorie == Categorie.Connue && l.IdUtilisateur == idUtilisateur);

                var configUtilisateur = conn.Table<Configuration>().ToList().Find(
                    c => c.IdUtilisateur == idUtilisateur);

                lieuxConnus.ForEach(l => l.IsCategorieToggled = switchPlacesConnues.IsToggled);
                configUtilisateur.IsSwitchLieuxConnusToggled = switchPlacesConnues.IsToggled;

                // MàJ les configurations de l'utilisateur ainsi que
                // les lieux activés dans la liste des favoris
                conn.Update(configUtilisateur);
                conn.UpdateAll(lieuxConnus);
            }
        }

        private void OnToggled_PlacesSouhaitees(object sender, EventArgs e)
        {
            var idUtilisateur = Auth.RetournerIdentifiantUtilisateur();

            using (var conn = new SQLiteConnection(App.CheminBD))
            {
                var lieuxSouhaites = conn.Table<Lieu>().ToList().FindAll(
                l => l.Categorie == Categorie.Souhaitee && l.IdUtilisateur == idUtilisateur);

                var configUtilisateur = conn.Table<Configuration>().ToList().Find(
                    c => c.IdUtilisateur == idUtilisateur);

                lieuxSouhaites.ForEach(l => l.IsCategorieToggled = switchPlacesSouhaitees.IsToggled);
                configUtilisateur.IsSwitchLieuxSouhaitesToggled = switchPlacesSouhaitees.IsToggled;

                conn.Update(configUtilisateur);
                conn.UpdateAll(lieuxSouhaites);
            }
        }

        private void OnToggled_PlacesVisitees(object sender, EventArgs e)
        {
            var idUtilisateur = Auth.RetournerIdentifiantUtilisateur();

            using (var conn = new SQLiteConnection(App.CheminBD))
            {
                var lieuxVisites = conn.Table<Lieu>().ToList().FindAll(
                 l => l.Categorie == Categorie.Visitee && l.IdUtilisateur == idUtilisateur);

                var configUtilisateur = conn.Table<Configuration>().ToList().Find(
                    c => c.IdUtilisateur == idUtilisateur);

                lieuxVisites.ForEach(l => l.IsCategorieToggled = switchPlacesVisitees.IsToggled);
                configUtilisateur.IsSwitchLieuxVisitesToggled = switchPlacesVisitees.IsToggled;

                conn.Update(configUtilisateur);
                conn.UpdateAll(lieuxVisites);
            }
        }

        private async void DegreLatitude_Unfocused(object sender, TextChangedEventArgs e)
        {
            var degreLatitudeEstConverti = double.TryParse(degreLatitude.Text, out double degreLatitudeSaisi);

            if (!degreLatitudeEstConverti)
            {
                await DisplayAlert("Alerte", "Veuillez svp saisir un degré de latitude valide (e.g. 2.5, 51.3)", "Fermer");
                return;
            }

            var idUtilisateur = Auth.RetournerIdentifiantUtilisateur();

            using (var conn = new SQLiteConnection(App.CheminBD))
            {
                var configUtilisateur = conn.Table<Configuration>().ToList().Find(
                    c => c.IdUtilisateur == idUtilisateur);

                configUtilisateur.DegreLatitude = double.Parse(degreLatitude.Text);

                // MàJ la configuration de l'utilisateur
                conn.Update(configUtilisateur);
            }
        }

        private async void DegreLongitude_Unfocused(object sender, TextChangedEventArgs e)
        {
            var degreLongitudeEstConverti = double.TryParse(degreLongitude.Text, out double degreLongitudeSaisi);

            if (!degreLongitudeEstConverti)
            {
                await DisplayAlert("Alerte", "Veuillez svp saisir un degré de longitude valide (e.g. 2.5, 51.3)", "Fermer");
                return;
            }

            var idUtilisateur = Auth.RetournerIdentifiantUtilisateur();

            using (var conn = new SQLiteConnection(App.CheminBD))
            {
                var configUtilisateur = conn.Table<Configuration>().ToList().Find(
                    c => c.IdUtilisateur == idUtilisateur);

                configUtilisateur.DegreLongitude = double.Parse(degreLongitude.Text);

                // MàJ la configuration de l'utilisateur
                conn.Update(configUtilisateur);
            }
        }
    }
}