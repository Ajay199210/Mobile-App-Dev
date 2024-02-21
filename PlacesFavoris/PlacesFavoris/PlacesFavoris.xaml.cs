using PlacesFavoris.Helper;
using PlacesFavoris.Models;
using SQLite;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PlacesFavoris
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlacesFavoris : ContentPage
    {
        public PlacesFavoris()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            using (var conn = new SQLiteConnection(App.CheminBD))
            {
                var idUtilisateurAuthentifie = Auth.RetournerIdentifiantUtilisateur();

                listeLieux.ItemsSource = conn.Table<Lieu>().ToList()
                    .FindAll(l => l.IsCategorieToggled && l.IdUtilisateur == idUtilisateurAuthentifie);
            }
        }

        private void ToolbarItemAjoutLieu_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new NouveauLieu());
        }

        private void listeLieux_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (listeLieux.SelectedItem is Lieu lieu)
            {
                Navigation.PushAsync(new NouveauLieu(lieu));
                listeLieux.SelectedItem = null;
            }
        }
    }
}