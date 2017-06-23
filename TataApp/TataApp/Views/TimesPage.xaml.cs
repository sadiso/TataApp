using System;
using TataApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TataApp.Views
{
    //[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TimesPage : ContentPage
    {
        public TimesPage()
        {
            InitializeComponent();

            var timesViewModel = TimesViewModel.GetInstance();
            base.Appearing += (object sender, EventArgs e) =>
            {
                timesViewModel.RefreshCommand.Execute(this);
            };
        }
    }
}