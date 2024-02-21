﻿using System.IO;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;

namespace PlacesFavoris.Droid
{
    [Activity(Label = "PlacesFavoris", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            // Initialisation de la carte
            Xamarin.FormsMaps.Init(this, savedInstanceState);

            // Configurer le chemin de la BD
            var nomBD = "placesFavoris_db.sqlite";
            var repertoire = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var cheminBD = Path.Combine(repertoire, nomBD);

            LoadApplication(new App(cheminBD));
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}