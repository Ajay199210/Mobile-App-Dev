using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace UnitsConverter
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Main : ContentPage
    {
        public Main()
        {
            InitializeComponent();
        }

        private void DeleteKey_Clicked(object sender, EventArgs e)
        {
            mesureFrom.Text = "";
            valeurFrom.Text = "";
            mesureTo.Text = "";
            valeurTo.Text = "";
        }

        private void NumKey_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(valeurTo.Text))
            {
                if (valeurFrom.Text.Length > 10) // Cela limite le nombre que l'utilisateur peut saisir et
                                                 // va gérer le overflow en même temps
                {
                    DisplayAlert("Alerte", "Le nombre doit avoir moins que 10 chiffres", "Fermer");
                    return;
                }

                var btn = (Button)sender;

                valeurFrom.Text = valeurFrom.Text.Length > 1 && valeurFrom.Text.StartsWith("0") ?
                    valeurFrom.Text : valeurFrom.Text.TrimStart('0');

                valeurFrom.Text += btn.Text;
            }
        }

        // Cm -> Pouce
        private void ConversionKey_CM_POUCE_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(valeurFrom.Text) &&
                string.IsNullOrEmpty(valeurTo.Text))
            {
                mesureFrom.Text = "Cm";
                mesureTo.Text = "Pouce";
                valeurTo.Text = string.Format(
                    $"{Math.Round(double.Parse(valeurFrom.Text) / 2.54, 3)}"
                );
            }
        }

        // Pouce -> Cm
        private void ConversionKey_POUCE_CM_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(valeurFrom.Text) &&
                string.IsNullOrEmpty(valeurTo.Text))
            {
                mesureFrom.Text = "Pouce";
                mesureTo.Text = "Cm";
                valeurTo.Text = string.Format(
                    $"{Math.Round(double.Parse(valeurFrom.Text) * 2.54, 3)}"
                );
            }
        }

        // M -> Pied
        private void ConversionKey_M_PIED_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(valeurFrom.Text) &&
                string.IsNullOrEmpty(valeurTo.Text))
            {
                mesureFrom.Text = "M";
                mesureTo.Text = "Pied";
                valeurTo.Text = string.Format(
                    $"{Math.Round(double.Parse(valeurFrom.Text) * 3.281, 3)}"
                );
            }
        }

        // Pied -> M
        private void ConversionKey_PIED_M_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(valeurFrom.Text) &&
                string.IsNullOrEmpty(valeurTo.Text))
            {
                mesureFrom.Text = "Pied";
                mesureTo.Text = "M";
                valeurTo.Text = string.Format(
                    $"{Math.Round(double.Parse(valeurFrom.Text) / 3.281, 3)}"
                );
            }
        }

        // G -> Once
        private void ConversionKey_G_ONCE_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(valeurFrom.Text) &&
                string.IsNullOrEmpty(valeurTo.Text))
            {
                mesureFrom.Text = "G";
                mesureTo.Text = "Once";
                valeurTo.Text = string.Format(
                    $"{Math.Round(double.Parse(valeurFrom.Text) / 28.35, 3)}"
                );
            }
        }

        // Once -> G
        private void ConversionKey_ONCE_G_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(valeurFrom.Text) &&
                string.IsNullOrEmpty(valeurTo.Text))
            {
                mesureFrom.Text = "Once";
                mesureTo.Text = "G";
                valeurTo.Text = string.Format(
                    $"{Math.Round(double.Parse(valeurFrom.Text) * 28.35, 3)}"
                );
            }
        }

        // Kg -> Lb
        private void ConversionKey_KG_LB_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(valeurFrom.Text) &&
                string.IsNullOrEmpty(valeurTo.Text))
            {
                mesureFrom.Text = "Kg";
                mesureTo.Text = "Lb";
                valeurTo.Text = string.Format(
                    $"{Math.Round(double.Parse(valeurFrom.Text) * 2.205, 3)}"
                );
            }
        }

        // Lb -> Kg
        private void ConversionKey_LB_KG_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(valeurFrom.Text) &&
                string.IsNullOrEmpty(valeurTo.Text))
            {
                mesureFrom.Text = "Lb";
                mesureTo.Text = "Kg";
                valeurTo.Text = string.Format(
                    $"{Math.Round(double.Parse(valeurFrom.Text) / 2.205, 3)}"
                );
            }
        }
    }
}