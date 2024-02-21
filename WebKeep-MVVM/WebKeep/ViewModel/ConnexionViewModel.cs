using SQLite;
using System.ComponentModel;
using System.Linq;
using WebKeep.Models;
using Xamarin.Forms;

namespace WebKeep.ViewModel
{
    public class ConnexionViewModel : INotifyPropertyChanged
    {
        private string _nomUtilisateur;
        private string _motDePasse;

        public event PropertyChangedEventHandler PropertyChanged;

        public Command ConnexionCommande { get; set; }

        public string NomUtilisateur
        {
            get { return _nomUtilisateur; }
            set
            {
                _nomUtilisateur = value;
                OnPropertyChanged("NomUtilisateur");
            }
        }

        public string MotDePasse
        {
            get { return _motDePasse; }
            set
            {
                _motDePasse = value;
                OnPropertyChanged("MotDePasse");
            }
        }

        public ConnexionViewModel()
        {
            ConnexionCommande = new Command(ConnecterUtilisateur);
        }

        private async void ConnecterUtilisateur()
        {
            if (string.IsNullOrEmpty(NomUtilisateur) || string.IsNullOrEmpty(MotDePasse))
            {
                await App.Current.MainPage.DisplayAlert("Attention",
                    "Le nom de l'utilisateur et le mot de passe sont obligatoires",
                    "Fermer");
                return;
            }

            var nomUtilisateurSaisi = NomUtilisateur.Trim().ToLower(); // Ignorer la diff. entre maj. et min.
            var motDePasseSaisi = MotDePasse;

            using (var conn = new SQLiteConnection(App.CheminBD))
            {
                var utilisateurs = conn.Table<Utilisateur>().ToList();

                if (utilisateurs.All(u => u.NomUtilisateur.ToLower() != nomUtilisateurSaisi))
                {
                    var creerCompte = await App.Current.MainPage.DisplayAlert("Attention",
                        $"Un utilisateur avec ce nom n'existe pas. Voulez-vous créer un utilisateur avec " +
                        $"ce nom et ce mot de passe ?", "Oui", "Non");

                    if (creerCompte)
                    {
                        conn.Insert(new Utilisateur
                        {
                            NomUtilisateur = NomUtilisateur.Trim(),
                            MotDePasse = motDePasseSaisi,
                        });

                        await App.Current.MainPage.DisplayAlert("Alerte", "Le compte a été créée en succès !", "Fermer");

                        // Vider les deux champs
                        NomUtilisateur = "";
                        MotDePasse = "";
                    }
                }
                else
                {
                    // Le compte doit exister dans la BD
                    var utilisateur = utilisateurs.First(u => u.NomUtilisateur.ToLower() == nomUtilisateurSaisi);

                    if (utilisateur.MotDePasse == motDePasseSaisi)
                    {
                        await App.Current.MainPage.Navigation.PushAsync(new PagePrincipale(utilisateur.Id));
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Attention", "Mot de passe est incorrecte", "Fermer");
                        return;
                    }
                }
            }
        }

        public void OnPropertyChanged(string nomProperty)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nomProperty));
        }
    }
}
