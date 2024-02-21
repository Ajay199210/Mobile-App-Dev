using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Scores
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Main : TabbedPage
    {
        public Main()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Title = $"NHL {DateTime.Now.Year}";
        }
    }
}