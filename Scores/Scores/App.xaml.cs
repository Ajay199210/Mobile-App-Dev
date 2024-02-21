using Scores.Models;
using SQLite;
using Xamarin.Forms;

namespace Scores
{
    public partial class App : Application
    {
        public static string CheminBD;

        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new Main());
        }

        public App(string cheminBD)
        {
            InitializeComponent();

            MainPage = new NavigationPage(new Main());

            CheminBD = cheminBD;

            using (var conn = new SQLiteConnection(cheminBD))
            {
                conn.CreateTable<Equipe>();
                conn.CreateTable<Match>();
            }
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