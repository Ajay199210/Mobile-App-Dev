using WebKeep.Models;
using WebKeep.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WebKeep
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NouveauSite : ContentPage
    {
        private readonly int _idUtilisateur;
        private readonly Site _siteChoisi;

        public NouveauSite(int idUtilisateur)
        {
            InitializeComponent();

            _idUtilisateur = idUtilisateur;
        }

        public NouveauSite(Site site, int idUtilisateur)
        {
            InitializeComponent();

            _idUtilisateur = idUtilisateur;
            _siteChoisi = site;

            titrePage.Text = "Modification";
            txtNom.Text = site.Nom;
            txtLien.Text = site.Lien;
            txtNote.Text = site.Note;
            btnSupprimer.IsVisible = true;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var nouveauSiteVM = (Resources["vm"] as NouveauSiteViewModel);

            nouveauSiteVM.IdUtilisateur = _idUtilisateur;

            if (_siteChoisi != null)
            {
                nouveauSiteVM.SiteChoisi = _siteChoisi;
            }
        }
    }
}