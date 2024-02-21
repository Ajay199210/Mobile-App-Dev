using PlacesFavoris.Helper;
using PlacesFavoris.Models;
using SQLite;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PlacesFavoris
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NouveauCompte : ContentPage
    {
        public NouveauCompte()
        {
            InitializeComponent();
        }

        private async void BtnCreerCompte_Clicked(object sender, EventArgs e)
        {
            var adresseCourriel = txtAdresseCourriel.Text;
            var motDePasse = txtMotDePasse.Text;
            var confirmerMotDePasse = txtConfirmerMotDePasse.Text;

            // Valider l'adresse courriel
            if (string.IsNullOrEmpty(adresseCourriel))
            {
                await DisplayAlert("Alerte", "Veuillzez svp ajouter une adresse courriel", "Fermer");
                return;
            }

            // Valider le mot de passe
            if (string.IsNullOrEmpty(motDePasse))
            {
                await DisplayAlert("Alerte", "Veuillzez svp ajouter un mot de passe", "Fermer");
                return;
            }

            // Valider la confirmation du mot de passe
            if (string.IsNullOrEmpty(confirmerMotDePasse))
            {
                await DisplayAlert("Alerte", "Veuillez svp confirmer le mot de passe", "Fermer");
                return;
            }

            // Valider que le mot de passe et la confirmation sont les mêmes
            if (motDePasse != confirmerMotDePasse)
            {
                await DisplayAlert("Alerte", "La confirmation du mot de passe ne correspond pas." +
                    " Veuillez svp ajouter une confirmation correcte.", "Fermer");
                return;
            }

            // Création du compte
            var succes = await Auth.CreerUtilisateur(adresseCourriel, motDePasse);

            if (succes)
            {
                await DisplayAlert("Message", "L'utilisateur a été créé avec succès", "Fermer");

                // Créer les configurations initiales de l'utilisateur
                using (var conn = new SQLiteConnection(App.CheminBD))
                {
                    conn.Insert(
                        new Configuration
                        {
                            IdUtilisateur = Auth.RetournerIdentifiantUtilisateur(),
                            DegreLatitude = 0.1,
                            DegreLongitude = 0.1,
                            IsSwitchLieuxConnusToggled = true,
                            IsSwitchLieuxSouhaitesToggled = true,
                            IsSwitchLieuxVisitesToggled = true
                        }
                    );
                }

                await Navigation.PopAsync();
            }
        }

        private async void BtnAnnuler_Clicked(object sender, EventArgs e)
        {
            var annuler = await DisplayAlert("Alerte", "Voulez-vous quitter l'enregistrement d'un nouveau compte ?",
                "Oui", "Non");
            if (annuler)
            {
                await Navigation.PopAsync();
            }
        }
    }
}