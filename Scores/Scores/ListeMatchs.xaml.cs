using Scores.Models;
using SQLite;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Scores
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListeMatchs : ContentPage
    {
        public ListeMatchs()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            lstMatchs.ItemsSource = null;
            using(var conn = new SQLiteConnection(App.CheminBD))
            {
                var listeMatches = from mat in conn.Table<Match>()
                                   join eqHome in conn.Table<Equipe>() on mat.IdEquipeHome equals eqHome.Id
                                   join eqAway in conn.Table<Equipe>() on mat.IdEquipeAway equals eqAway.Id
                                   select new Match()
                                   {
                                       Id = mat.Id,
                                       IdEquipeHome = mat.IdEquipeHome,
                                       IdEquipeAway = mat.IdEquipeAway,
                                       NomEquipeHome = eqHome.Nom,
                                       NomEquipeAway = eqAway.Nom,
                                       ScoreEquipeHome = mat.ScoreEquipeHome,
                                       ScoreEquipeAway = mat.ScoreEquipeAway,
                                       DateMatch = mat.DateMatch,
                                   };

                lstMatchs.ItemsSource = listeMatches.ToList().OrderByDescending(m => m.DateMatch);
            }
        }

        private void ToolbarItemAjoutMatch_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new NouveauMatch());
        }
    }
}