using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace UnitsConverter
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StartupPage : ContentPage
    {
        public StartupPage()
        {
            InitializeComponent();
        }

        private void BtnStartup_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Main());
        }
    }
}
