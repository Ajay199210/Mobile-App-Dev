using PlacesFavoris.Helper;
using System.Threading.Tasks;
using Foundation;
using System;
using Xamarin.Forms;

[assembly: Dependency(typeof(PlacesFavoris.iOS.Dependances.Auth))]
namespace PlacesFavoris.iOS.Dependances
{
    public class Auth : IAuth
    {
        public async Task<bool> ConnecterUtilisateur(string adresseCourriel, string motDePasse)
        {
            try
            {
                await Firebase.Auth.Auth.DefaultInstance.SignInWithPasswordAsync(adresseCourriel, motDePasse);
                return true;
            }
            catch (NSErrorException erreur)
            {
                throw new Exception(erreur.Message);
            }
            catch (Exception ex)
            {
                // il faut enregistrer la vraie exception quelque part
                // ne masquez jamais la vraie exception comme ça
                throw new Exception("Une erreur est survenue");
            }
        }

        public async Task<bool> CreerUtilisateur(string adresseCourriel, string motDePasse)
        {
            try
            {
                await Firebase.Auth.Auth.DefaultInstance.CreateUserAsync(adresseCourriel, motDePasse);
                return true;
            }
            catch (NSErrorException erreur)
            {
                throw new Exception(erreur.Message);
            }
            catch (Exception ex)
            {
                // il faut enregistrer la vraie exception quelque part
                // ne masquez jamais la vraie exception comme ça
                throw new Exception("Une erreur est survenue");
            }
        }

        public Task<bool> DeconnecterUtilisateur()
        {
            try
            {
                Firebase.Auth.Auth.DefaultInstance.SignOut(out NSError error);
                return Task.FromResult(true);
            }
            catch (NSErrorException erreur)
            {
                throw new Exception(erreur.Message);
            }
            catch (Exception ex)
            {
                // il faut enregistrer la vraie exception quelque part
                // ne masquez jamais la vraie exception comme ça
                throw new Exception("Une erreur est survenue");
            }
        }

        public string RetournerIdentifiantUtilisateur()
        {
            return Firebase.Auth.Auth.DefaultInstance.CurrentUser.Uid;
        }

        public string RetournerNomUtilisateur()
        {
            return Firebase.Auth.Auth.DefaultInstance.CurrentUser.DisplayName;
        }

        public bool UtilisateurAuthentifie()
        {
            return Firebase.Auth.Auth.DefaultInstance.CurrentUser != null;
        }
    }
}