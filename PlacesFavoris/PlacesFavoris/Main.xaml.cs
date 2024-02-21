using PlacesFavoris.Helper;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PlacesFavoris
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Main : TabbedPage
    {
        public Main()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        private async void ToolbarItemSignOut_Clicked(object sender, EventArgs e)
        {
            var seDeconnecter = await DisplayAlert("Alert", "Êtes-vous sûr de vouloir vous déconnecter ?", "Déconnecter", "Annuler");
            if (seDeconnecter)
            {
                var utilisateurDeconnecte = await Auth.DeconnecterUtilisateur();
                if (utilisateurDeconnecte)
                {
                    App.Current.MainPage = new NavigationPage(new PageLogin());
                }
            }
        }
    }
}