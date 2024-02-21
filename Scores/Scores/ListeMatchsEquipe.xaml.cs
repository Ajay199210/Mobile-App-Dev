using Scores.Models;
using SQLite;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Scores
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListeMatchsEquipe : ContentPage
    {
        private Equipe _equipe;

        public ListeMatchsEquipe()
        {
            InitializeComponent();
        }

        public ListeMatchsEquipe(Equipe equipe)
        {
            InitializeComponent();
            _equipe = equipe;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Title = _equipe.Nom;

            using (var conn = new SQLiteConnection(App.CheminBD))
            {
                var matchsJouesEquipe = from match in conn.Table<Match>()
                                            .Where(m => m.IdEquipeHome == _equipe.Id || m.IdEquipeAway == _equipe.Id)
                                        join eqHome in conn.Table<Equipe>() on match.IdEquipeHome equals eqHome.Id
                                        join eqAway in conn.Table<Equipe>() on match.IdEquipeAway equals eqAway.Id
                                        select new Match()
                                        {
                                            Id = match.Id,
                                            IdEquipeHome = match.IdEquipeHome,
                                            IdEquipeAway = match.IdEquipeAway,
                                            NomEquipeHome = eqHome.Nom,
                                            NomEquipeAway = eqAway.Nom,
                                            ScoreEquipeHome = match.ScoreEquipeHome,
                                            ScoreEquipeAway = match.ScoreEquipeAway,
                                            DateMatch = match.DateMatch,
                                        };

                lstMatchsEquipes.ItemsSource = matchsJouesEquipe.OrderByDescending(m => m.DateMatch);
            }
        }
    }
}