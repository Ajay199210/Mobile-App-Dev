using SQLite;
using WebKeep.Models;
using Xamarin.Forms;

namespace WebKeep
{
    public partial class App : Application
    {
        public static string CheminBD;

        public App(string cheminBD)
        {
            InitializeComponent();

            CheminBD = cheminBD;

            using (var conn = new SQLiteConnection(cheminBD))
            {
                conn.CreateTable<Utilisateur>();
                conn.CreateTable<Site>();
            }

            MainPage = new NavigationPage(new PageConnexion());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
