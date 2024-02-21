using WebKeep.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WebKeep
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PagePrincipale : ContentPage
    {
        private readonly int _idUtilisateur;

        public PagePrincipale(int idUtilisateur)
        {
            InitializeComponent();

            _idUtilisateur = idUtilisateur;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var pagePrincipaleVM = (Resources["vm"] as PagePrincipaleViewModel);

            pagePrincipaleVM.IdUtilisateur = _idUtilisateur;
            pagePrincipaleVM.ObtenirSites();
        }
    }
}