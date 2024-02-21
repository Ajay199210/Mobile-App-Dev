using PlacesFavoris.Helper;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PlacesFavoris
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageLogin : ContentPage
    {
        public PageLogin()
        {
            InitializeComponent();
        }

        // Authentification
        private async void BtnConnexion_Clicked(object sender, EventArgs e)
        {
            var adresseCourriel = txtAdresseCourriel.Text;
            var motDePasse = txtMotDePasse.Text;

            if (string.IsNullOrEmpty(adresseCourriel) || string.IsNullOrEmpty(motDePasse))
            {
                await DisplayAlert("Alerte", "Veuillez svp saisir une adresse courriel et un mot de passe", "Fermer");
            }
            else
            {
                var existe = await Auth.ConnecterUtilisateur(adresseCourriel, motDePasse);

                if (existe)
                {
                    App.Current.MainPage = new NavigationPage(new Main());
                }
            }
        }

        private void BtnNouveauCompte_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new NouveauCompte());
        }
    }
}