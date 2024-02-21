using System;
using PlacesFavoris.Helper;
using System.Threading.Tasks;
using Xamarin.Forms;
using Firebase.Auth;

[assembly: Dependency(typeof(PlacesFavoris.Droid.Dependances.Auth))]
namespace PlacesFavoris.Droid.Dependances
{
    public class Auth : IAuth
    {
        public async Task<bool> ConnecterUtilisateur(string adresseCourriel, string motDePasse)
        {
            try
            {
                await FirebaseAuth.Instance.SignInWithEmailAndPasswordAsync(adresseCourriel, motDePasse);
                return true;
            }
            catch (FirebaseAuthInvalidUserException erreur)
            {
                throw new Exception(erreur.Message);
            }
            catch (FirebaseAuthInvalidCredentialsException erreur)
            {
                throw new Exception(erreur.Message);
            }
            catch (Exception ex)
            {
                // il faut enregistrer la vraie exception quelque part
                // ne masquez jamais la vraie exception comme ça
                
                // Les informations de l'utilisateur ne sont pas correctes, 
                // donc cette exception va être levée pour différentes raisons...
                // (pas FirebaseAuthInvalidUserException)
                throw new Exception("Une erreur est survenue. Veuillez svp réessayer");
            }
        }

        public async Task<bool> CreerUtilisateur(string adresseCourriel, string motDePasse)
        {
            try
            {
                await FirebaseAuth.Instance.CreateUserWithEmailAndPasswordAsync(adresseCourriel, motDePasse);
                return true;
            }
            catch (FirebaseAuthUserCollisionException erreur)
            {
                throw new Exception(erreur.Message);
            }
            catch (FirebaseAuthInvalidCredentialsException erreur)
            {
                throw new Exception(erreur.Message);
            }
            catch (Exception ex)
            {
                // il faut enregistrer la vraie exception quelque part
                // ne masquez jamais la vraie exception comme ça
                throw new Exception("Une erreur est survenue. Veuillez svp réessayer");
            }
        }

        public Task<bool> DeconnecterUtilisateur()
        {
            try
            {
                FirebaseAuth.Instance.SignOut();
                return Task.FromResult(true);
            }
            catch(Exception)
            {
                throw new Exception("Une erreur est survenue. Veuillez svp réessayer");
            }
        }

        public string RetournerIdentifiantUtilisateur()
        {
            return FirebaseAuth.Instance.CurrentUser.Uid;
        }

        public string RetournerNomUtilisateur()
        {
            return FirebaseAuth.Instance.CurrentUser.DisplayName;
        }

        public bool UtilisateurAuthentifie()
        {
            return FirebaseAuth.Instance.CurrentUser != null;
        }
    }
}