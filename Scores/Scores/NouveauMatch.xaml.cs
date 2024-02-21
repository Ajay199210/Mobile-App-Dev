using Scores.Models;
using SQLite;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Scores
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NouveauMatch : ContentPage
    {
        private Match _match;

        public NouveauMatch()
        {
            InitializeComponent();
        }

        public NouveauMatch(Match match)
        {
            InitializeComponent();

            _match = match;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Title = "Enregistrer un nouveau match";

            using (var conn = new SQLiteConnection(App.CheminBD))
            {
                var listeEquipes = conn.Table<Equipe>().ToList();

                pickEquipeHome.ItemsSource = listeEquipes;
                pickEquipeHome.ItemDisplayBinding = new Binding("Nom");
                pickEquipeAway.ItemsSource = listeEquipes;
                pickEquipeAway.ItemDisplayBinding = new Binding("Nom");

                if (_match != null)
                {
                    pickEquipeHome.SelectedItem = listeEquipes.FirstOrDefault(eq => eq.Id == _match.IdEquipeHome);
                    pickEquipeAway.SelectedItem = listeEquipes.FirstOrDefault(eq => eq.Id == _match.IdEquipeAway);
                    scoreHome.Text = _match.ScoreEquipeHome.ToString();
                    scoreAway.Text = _match.ScoreEquipeAway.ToString();
                    pickDateMatch.Date = _match.DateMatch;
                }
            }
        }

        private void BtnNouveauMatch_Clicked(object sender, EventArgs e)
        {
            var equipeHomeChoisie = (pickEquipeHome.SelectedItem as Equipe);
            var equipeAwayChoisie = (pickEquipeAway.SelectedItem as Equipe);

            if (equipeHomeChoisie == null)
            {
                DisplayAlert("Alerte", "Veuillez svp choisir l'équipe Home", "Fermer");
                return;
            }

            if (equipeAwayChoisie == null)
            {
                DisplayAlert("Alerte", "Veuillez svp choisir l'équipe Away", "Fermer");
                return;
            }

            if (string.IsNullOrEmpty(scoreHome.Text))
            {
                DisplayAlert("Alerte", "Veuillez svp saisir le score de l'équipe Home", "Fermer");
                return;
            }

            if (string.IsNullOrEmpty(scoreAway.Text))
            {
                DisplayAlert("Alerte", "Veuillez svp saisir le score de l'équipe Away", "Fermer");
                return;
            }

            var scoreEquipeHome = int.Parse(scoreHome.Text);
            var scoreEquipeAway = int.Parse(scoreAway.Text);

            var matchDate = pickDateMatch.Date;

            if (matchDate == null)
            {
                DisplayAlert("Alerte", "Veuillez svp choisir une date pour le match", "Fermer");
                return;
            }

            // Un même match ne peut pas avoir la même équipe en Home et Away
            if (equipeHomeChoisie.Nom == equipeAwayChoisie.Nom)
            {
                DisplayAlert("Alerte", "Un même match ne peut pas avoir la même équipe en Home et Away", "Fermer");
                return;
            }

            // Un match ne peut pas être créé dans le futur
            if (matchDate > DateTime.Now)
            {
                DisplayAlert("Alerte", "Un match ne peut pas être créé dans le futur", "Fermer");
                return;
            }

            // Enregistrer dans la BD
            using (var conn = new SQLiteConnection(App.CheminBD))
            {
                // Une équipe ne peut pas jouer deux matchs dans la même journée
                var matchMemeJournee = conn.Table<Match>().Where(m => m.DateMatch == matchDate).
                    Any(m => equipeHomeChoisie.Id == m.IdEquipeHome ||
                             equipeHomeChoisie.Id == m.IdEquipeAway ||
                             equipeAwayChoisie.Id == m.IdEquipeHome ||
                             equipeAwayChoisie.Id == m.IdEquipeAway
                        );

                if (matchMemeJournee)
                {
                    DisplayAlert("Alerte", "Une équipe ne peut pas jouer deux matchs dans la même journée", "Fermer");
                    return;
                }

                if (_match == null)
                {
                    var nouveauMatch = new Match()
                    {
                        IdEquipeHome = equipeHomeChoisie.Id,
                        IdEquipeAway = equipeAwayChoisie.Id,
                        ScoreEquipeHome = scoreEquipeHome,
                        ScoreEquipeAway = scoreEquipeAway,
                        DateMatch = matchDate
                    };
                    
                    conn.Insert(nouveauMatch);

                    // Incrémenter le nombre de match joués pour chaque équipe
                    equipeHomeChoisie.NbTotalMatchs++;
                    equipeAwayChoisie.NbTotalMatchs++;

                    // Système de pointage
                    if (scoreEquipeHome > scoreEquipeAway)
                    {
                        equipeHomeChoisie.NbTotalPoints += 3;
                    }
                    else if (scoreEquipeHome < scoreEquipeAway)
                    {
                        equipeAwayChoisie.NbTotalPoints += 3;
                    }
                    else
                    {
                        equipeHomeChoisie.NbTotalPoints++;
                        equipeAwayChoisie.NbTotalPoints++;
                    }

                    // Mettre à jour le nombre total des matchs et le total des points pour chaque équipe
                    conn.Update(equipeHomeChoisie);
                    conn.Update(equipeAwayChoisie);
                }

                DisplayAlert("Succès", "Le match a été enregistré avec succès.", "Fermer");
                Navigation.PopAsync();
            }
        }
    }
}