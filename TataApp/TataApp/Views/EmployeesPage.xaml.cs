
namespace TataApp.Views
{
    using System;
    using ViewModels;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EmployeesPage : ContentPage
    {
        public EmployeesPage()
        {
            InitializeComponent();

            var employeesViewModel = EmployeesViewModel.GetInstance();
            base.Appearing += (object sender, EventArgs e) =>
            {
                employeesViewModel.RefreshCommand.Execute(this);
            };
        }
    }
}