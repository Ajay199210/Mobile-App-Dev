using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PlacesFavoris.Helper
{
    public interface IAuth
    {
        Task<bool> CreerUtilisateur(string adresseCourriel, string motDePasse);
        Task<bool> ConnecterUtilisateur(string adresseCourriel, string motDePasse);
        Task<bool> DeconnecterUtilisateur();
        bool UtilisateurAuthentifie();
        string RetournerIdentifiantUtilisateur();
        string RetournerNomUtilisateur();
    }

    internal class Auth
    {
        public static IAuth auth = DependencyService.Get<IAuth>();

        public static async Task<bool> CreerUtilisateur(string adresseCourriel, string motDePasse)
        {
            try
            {
                return await auth.CreerUtilisateur(adresseCourriel, motDePasse);
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Erreur", ex.Message, "Fermer");
                return false;
            }
        }

        public static async Task<bool> ConnecterUtilisateur(string adresseCourriel, string motDePasse)
        {
            try
            {
                return await auth.ConnecterUtilisateur(adresseCourriel, motDePasse);
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Erreur", ex.Message, "Fermer");
                return false;
            }
        }

        public static async Task<bool> DeconnecterUtilisateur()
        {
            try
            {
                await auth.DeconnecterUtilisateur();
                return true;
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Erreur", ex.Message, "Fermer");
                return false;
            }
        }

        public static bool UtilisateurAuthentifie()
        {
            return auth.UtilisateurAuthentifie();
        }

        public static string RetournerIdentifiantUtilisateur()
        {
            return auth.RetournerIdentifiantUtilisateur();
        }

        public static string RetournerNomUtilisateur()
        {
            return auth.RetournerNomUtilisateur();
        }
    }
}
