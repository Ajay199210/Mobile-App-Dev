using Scores.Models;
using SQLite;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Scores
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NouvelleEquipe : ContentPage
    {
        private Equipe _equipe;

        public NouvelleEquipe()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            Title = "Ajouter une nouvelle équipe";
        }

        public NouvelleEquipe(Equipe equipe)
        {
            _equipe = equipe;
            txtNomEquipe.Text = equipe.Nom;
        }

        private void BtnNouvelleEquipe_Clicked(object sender, EventArgs e)
        {
            var nomEquipe = txtNomEquipe.Text;
            if(string.IsNullOrEmpty(nomEquipe))
            {
                DisplayAlert("Ajout", "Veuillez svp ajouter le nom de l'équipe", "Fermer");
                return;
            }

            using(var conn = new SQLiteConnection(App.CheminBD))
            {
                var equipes = conn.Table<Equipe>().ToList();
                if(_equipe == null)
                {
                    var exist = equipes.Any(eq => eq.Nom.ToLower() == nomEquipe.ToLower());
                    if (exist) 
                    {
                        DisplayAlert("Alerte", "Il existe déjà une équipe avec ce nom", "Fermer");
                        return;
                    }

                    conn.Insert(new Equipe() { Nom = nomEquipe });
                }
                else
                {
                    // Valider si une équipe existe déjà dans la liste avec le même nom et un Id different
                    var nomExist = conn.Table<Equipe>().Where(
                        eq => eq.Nom.ToLower() == nomEquipe.ToLower() && eq.Id != _equipe.Id).Count() > 0;
                    if (nomExist)
                    {
                        DisplayAlert("Alerte", "Il existe déjà une équipe avec ce nom", "Fermer");
                        return;
                    }

                    // Mettre à jour le nom de l'équipe
                    _equipe.Nom = txtNomEquipe.Text;
                    conn.Update(_equipe);
                }

                DisplayAlert("Ajout", "L'équipe a été ajoutée avec succès !", "Fermer");
                Navigation.PopAsync();
            }
        }
    }
}