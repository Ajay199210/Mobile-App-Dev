using SQLite;
using System.Collections.ObjectModel;
using System.ComponentModel;
using WebKeep.Models;
using Xamarin.Forms;

namespace WebKeep.ViewModel
{
    public class PagePrincipaleViewModel : INotifyPropertyChanged
    {
        private int _idUtilisateur;
        private Site _siteChoisi;
        private ObservableCollection<Site> _sites;
        private string _searchTerm;

        public event PropertyChangedEventHandler PropertyChanged;

        public Command NouveauSiteCommande { get; set; }
        public Command RechercherSitesCommande { get; set; }

        public ObservableCollection<Site> ListeSites
        {
            get { return _sites; }
            set
            {
                if (_sites != value)
                {
                    _sites = value;
                    OnPropertyChanged("ListeSites");
                }
            }
        }

        public int IdUtilisateur
        {
            get { return _idUtilisateur; }
            set
            {
                _idUtilisateur = value;
            }
        }

        public string SearchTerm
        {
            get { return _searchTerm; }
            set
            {
                _searchTerm = value;
                OnPropertyChanged("SearchTerm");
            }
        }

        public Site SiteChoisi
        {
            get { return _siteChoisi; }
            set
            {
                _siteChoisi = value;
                if (_siteChoisi != null)
                {
                    App.Current.MainPage.Navigation.PushAsync(new NouveauSite(_siteChoisi, _idUtilisateur));
                }
            }
        }

        public PagePrincipaleViewModel()
        {
            NouveauSiteCommande = new Command(NaviguerPageNouveauSite);
            RechercherSitesCommande = new Command(RechercherSites);
            ListeSites = new ObservableCollection<Site>();
        }

        // Obtenir la liste des site par utilisateur
        public void ObtenirSites()
        {
            using (var conn = new SQLiteConnection(App.CheminBD))
            {
                ListeSites.Clear();
                var listeSites = conn.Table<Site>().Where(s => s.IdUtilisateur == _idUtilisateur).ToList();

                foreach (var site in listeSites)
                {
                    ListeSites.Add(site);
                }
            }
        }

        // Naviguer vers la page d'ajout d'un site
        private async void NaviguerPageNouveauSite()
        {
            await App.Current.MainPage.Navigation.PushAsync(new NouveauSite(_idUtilisateur));
        }

        // Rechercher des sites
        public void RechercherSites()
        {
            var searchTerm = SearchTerm.Trim().ToLower();

            using (var conn = new SQLiteConnection(App.CheminBD))
            {
                ListeSites.Clear();
                var listeSitesFiltree = conn.Table<Site>().Where(s => s.IdUtilisateur == _idUtilisateur
                    && s.Nom.ToLower().Contains(searchTerm)).ToList();
                foreach (var site in listeSitesFiltree)
                {
                    ListeSites.Add(site);
                }
            }
        }

        public void OnPropertyChanged(string nomProperty)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nomProperty));
        }
    }
}
