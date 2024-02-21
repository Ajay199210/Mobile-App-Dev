using PlacesFavoris.Helper;
using PlacesFavoris.Models;
using SQLite;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace PlacesFavoris
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Carte : ContentPage
    {
        public Carte()
        {
            InitializeComponent();
            ObtenirLocalisation();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ObtenirLieux();
            ObtenirDerniereLocalisation();
        }

        private async void ObtenirLocalisation()
        {
            var statut = await App.ValiderEtDemanderLocalisation();
            if (statut == PermissionStatus.Granted)
            {
                var localisation = await Geolocation.GetLocationAsync();
                CentrerCarte(localisation.Latitude, localisation.Longitude);
            }
        }

        private async void ObtenirDerniereLocalisation()
        {
            var statut = await App.ValiderEtDemanderLocalisation();
            if (statut == PermissionStatus.Granted)
            {
                var lastKnownLocation = await Geolocation.GetLastKnownLocationAsync();
                CentrerCarte(lastKnownLocation.Latitude, lastKnownLocation.Longitude);
            }
        }

        private void CentrerCarte(double latitude, double longitude)
        {
            using (var conn = new SQLiteConnection(App.CheminBD))
            {
                var configuration = conn.Table<Configuration>().ToList().Find(
                    c => c.IdUtilisateur == Auth.RetournerIdentifiantUtilisateur());
                var centre = new Position(latitude, longitude);
                var span = new MapSpan(centre, configuration.DegreLatitude, configuration.DegreLongitude);
                carteLocalisation.MoveToRegion(span);
            }
        }

        private void ObtenirLieux()
        {
            carteLocalisation.Pins.Clear();

            using (var conn = new SQLiteConnection(App.CheminBD))
            {
                var idUtilisateurAuthentifie = Auth.RetournerIdentifiantUtilisateur();

                // Filtrer les lieux activés (switch ON) de l'utilisateur 
                var lieuxFavorisToggled = conn.Table<Lieu>().ToList()
                    .FindAll(l => l.IsCategorieToggled && l.IdUtilisateur == idUtilisateurAuthentifie); ;

                foreach (var lieu in lieuxFavorisToggled)
                {
                    var pin = new Pin()
                    {
                        Position = new Position(lieu.Latitude, lieu.Longitude),
                        Label = lieu.Nom,
                        Address = lieu.Adresse,
                        Type = PinType.Generic,
                    };

                    carteLocalisation.Pins.Add(pin);
                }
            }
        }
    }
}