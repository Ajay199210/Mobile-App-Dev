using SQLite;
using System;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using WebKeep.Models;
using Xamarin.Forms;

namespace WebKeep.ViewModel
{
    public class NouveauSiteViewModel : INotifyPropertyChanged
    {
        private int _idUtilisateur;

        public string _nomSite;
        public string _lienSite;
        public string _noteSite;

        private Site _siteChoisi;

        public event PropertyChangedEventHandler PropertyChanged;

        public Command EnregistrerCommande { get; set; }
        public Command SupprimerCommande { get; set; }

        public int IdUtilisateur
        {
            get { return _idUtilisateur; }
            set
            {
                _idUtilisateur = value;
            }
        }

        public Site SiteChoisi
        {
            get { return _siteChoisi; }
            set
            {
                _siteChoisi = value;
            }
        }

        public string NomSite
        {
            get { return _nomSite; }
            set
            {
                _nomSite = value;
                OnPropertyChanged("NomSite");
            }
        }

        public string LienSite
        {
            get { return _lienSite; }
            set
            {
                _lienSite = value;
                OnPropertyChanged("LienNote");
            }
        }

        public string NoteSite
        {
            get { return _noteSite; }
            set
            {
                _noteSite = value;
                OnPropertyChanged("NoteSite");
            }
        }

        public NouveauSiteViewModel()
        {
            EnregistrerCommande = new Command(Enregistrer);
            SupprimerCommande = new Command(Supprimer);
        }

        public async void Enregistrer()
        {
            if (string.IsNullOrEmpty(NomSite))
            {
                await App.Current.MainPage.DisplayAlert("Alerte", "Le nom du site est obligatoire", "Fermer");
                return;
            }

            if (!string.IsNullOrEmpty(LienSite))
            {
                var lienSaisi = LienSite.ToLower();

                /*
                 * 
                 * Dans notre cas, un lien:
                 *  - Commence par www.
                 *  - Contient les caractères alphanumériques (en plus le tiret comme séparateur)
                 *  - Contient ou fini par .com ou .ca (on peut ajouté autre TLD (Top-Level Domains) )
                 *  
                 */

                string pattern = @"^(www\.)[a-zA-Z0-9]+[a-zA-Z0-9.-]*[a-zA-Z0-9]+\.(com|ca)$";

                if (!Regex.IsMatch(lienSaisi, pattern))
                {
                    await App.Current.MainPage.DisplayAlert("Alerte", "Le format du lien est incorrecte. " +
                        "Veuillez svp saisir un lien valide.", "Fermer");
                    return;
                }
            }

            // Si l'un ou les deux deux champs Notes et Lien sont vides
            if (string.IsNullOrEmpty(LienSite) || string.IsNullOrEmpty(NoteSite))
            {
                var confirmer = await App.Current.MainPage.DisplayAlert("Alerte", "Vouz n'avez pas saisi le lien ou une note. " +
                        "Voulez-vous continuer ?", "Oui", "Non");

                if (!confirmer)
                {
                    return;
                }
            }

            // Enregistrer dans la BD
            using (var conn = new SQLiteConnection(App.CheminBD))
            {
                var sites = conn.Table<Site>().ToList().FindAll(
                    s => s.IdUtilisateur == _idUtilisateur);

                var nomSiteSaisi = NomSite.Trim().ToLower();

                // Un nouveau site
                if (_siteChoisi == null)
                {
                    // Valider s'il existe déjà un site avec le même nom
                    var siteMemeNomExiste = sites.Any(s => s.Nom.ToLower() == nomSiteSaisi);

                    if (siteMemeNomExiste)
                    {
                        await App.Current.MainPage.DisplayAlert("Alerte", "Il existe un site avec le même nom", "Fermer");
                        return;
                    }

                    // Valider si un site existe avec le même lien saisi
                    if (!string.IsNullOrEmpty(LienSite))
                    {
                        var lienSiteSaisi = LienSite.Trim().ToLower();

                        var siteMemeLienExiste = sites.Any(s => s.Lien != "" && s.Lien.ToLower() == lienSiteSaisi);

                        if (siteMemeLienExiste)
                        {
                            var confirmerAjout = await App.Current.MainPage.DisplayAlert("Alerte", "Il existe un site avec le même lien. " +
                                "Voulez-vous continuer ?", "Oui", "Non");

                            if (!confirmerAjout)
                            {
                                return;
                            }
                        }
                    }

                    conn.Insert(new Site
                    {
                        Nom = NomSite.Trim(),
                        Lien = string.IsNullOrEmpty(LienSite) ? "" : LienSite.Trim(),
                        Note = string.IsNullOrEmpty(NoteSite) ? "" : NoteSite.Trim(),
                        DateCreation = DateTime.Now,
                        IdUtilisateur = _idUtilisateur
                    });

                    await App.Current.MainPage.DisplayAlert("Alerte", "Le site est enregistré avec succès !", "Fermer");
                    await App.Current.MainPage.Navigation.PopAsync();
                }

                // Modification
                else
                {
                    // Valider s'il existe déjà un site avec le même nom
                    if (sites.Any(s => s.Id != _siteChoisi.Id && s.Nom.ToLower() == nomSiteSaisi))
                    {
                        await App.Current.MainPage.DisplayAlert("Alerte", "Il existe un site avec le même nom", "Fermer");
                        return;
                    }

                    // Valider si un site existe avec le même lien saisi
                    if (!string.IsNullOrEmpty(LienSite))
                    {
                        var lienSiteSaisi = LienSite.ToLower().Trim();
                        var siteMemeLienExiste = sites.Any(s => s.Id != _siteChoisi.Id &&
                                s.Lien != "" &&
                                s.Lien.ToLower() == lienSiteSaisi);

                        if (siteMemeLienExiste)
                        {
                            var confirmerAjout = await App.Current.MainPage.DisplayAlert("Alerte", "Il existe un site avec le même lien. " +
                                "Voulez-vous continuer ?", "Oui", "Non");

                            if (!confirmerAjout)
                            {
                                return;
                            }
                        }
                    }

                    // Enregistrer les modifications
                    _siteChoisi.Nom = NomSite.Trim();
                    _siteChoisi.Lien = string.IsNullOrEmpty(LienSite) ? "" : LienSite.Trim();
                    _siteChoisi.Note = string.IsNullOrEmpty(NoteSite) ? "" : NoteSite.Trim();

                    conn.Update(_siteChoisi);

                    await App.Current.MainPage.DisplayAlert("Alerte",
                        $"Les modifications du site sont enregistrées avec succès !", "Fermer");

                    await App.Current.MainPage.Navigation.PopAsync();
                }
            }
        }

        public async void Supprimer()
        {
            var confirmerSuppression = await App.Current.MainPage.DisplayAlert("Alerte",
                "Êtes-vous sûr de vouloir supprimer ce site ?", "Confirmer", "Annuler");

            if (confirmerSuppression)
            {
                if (_siteChoisi != null)
                {
                    using (var conn = new SQLiteConnection(App.CheminBD))
                    {
                        conn.Delete(_siteChoisi);
                    }

                    await App.Current.MainPage.DisplayAlert("Alerte", $"Le site `{_siteChoisi.Nom}` " +
                        $"est supprimé avec succès !", "Fermer");
                    await App.Current.MainPage.Navigation.PopAsync();
                }
            }
        }

        public void OnPropertyChanged(string nomProperty)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nomProperty));
        }
    }
}
