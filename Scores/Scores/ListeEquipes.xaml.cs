using Scores.Models;
using SQLite;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Scores
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListeEquipes : ContentPage
    {
        public ListeEquipes()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            lstEquipes.ItemsSource = null;
            using (var conn = new SQLiteConnection(App.CheminBD))
            {
                var equipes = conn.Table<Equipe>().ToList();
                var matchs = conn.Table<Match>().ToList();

                foreach (var equipe in equipes)
                {
                    // Total des scores marqués par l'équipe (GM)
                    var gm = matchs.Where(m => (equipe.Id == m.IdEquipeHome)).Sum(m => m.ScoreEquipeHome) + 
                        matchs.Where(m => (equipe.Id == m.IdEquipeAway)).Sum(m => m.ScoreEquipeAway);

                    // Total des scores marqués par tous les équipes adversaire (Gr)
                    var gr = matchs.Where(m => (equipe.Id == m.IdEquipeHome)).Sum(m => m.ScoreEquipeAway) + 
                        matchs.Where(m => (equipe.Id == m.IdEquipeAway)).Sum(m => m.ScoreEquipeHome);

                    equipe.TotalScores = gm;
                    equipe.TotalScoresAdversaires = gr;
                }

                lstEquipes.ItemsSource = equipes;
            }
        }

        private void ToolbarItemAjoutEquipe_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new NouvelleEquipe());
        }

        private void lstEquipes_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (lstEquipes.SelectedItem is Equipe equipe)
            {
                // Navigation vers la page de la liste des matchs joués par l'équipe séléctionné
                Navigation.PushAsync(new ListeMatchsEquipe(equipe));
                lstEquipes.SelectedItem = null;
            }
        }
    }
}