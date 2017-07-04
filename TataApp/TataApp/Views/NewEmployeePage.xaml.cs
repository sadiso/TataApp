using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TataApp.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TataApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewEmployeePage : ContentPage
    {
        #region Attributes
        //ApiService apiService;
        //DialogService dialogService;
        //DataService dataService;
        #endregion

        #region Constructors
        public NewEmployeePage()
        {
            //apiService = new ApiService();
            //dialogService = new DialogService();
            //dataService = new DataService();
            InitializeComponent();
        }
        #endregion

        #region Methods
        //async void GetTokenClicked(object sender, EventArgs args)
        //{
        //    if (string.IsNullOrEmpty(Password.Text))
        //    {
        //        await dialogService.ShowMessage("Error", "You must enter a password.");
        //        Password.Focus();
        //        return;
        //    }

        //    if (string.IsNullOrEmpty(Email.Text))
        //    {
        //        await dialogService.ShowMessage("Error", "You must enter the a email.");
        //        Email.Focus();
        //        return;
        //    }

        //    if (string.IsNullOrEmpty(ConfirmPassword.Text))
        //    {
        //        await dialogService.ShowMessage("Error", "You must enter the a password.");
        //        ConfirmPassword.Focus();
        //        return;
        //    }

        //    if (!Password.Text.Equals(ConfirmPassword.Text))
        //    {
        //        await dialogService.ShowMessage("Error", "Password and confirt password must be equals.");
        //        Password.Focus();
        //        return;
        //    }

        //    var checkConnetion = await apiService.CheckConnection();
        //    if (!checkConnetion.IsSuccess)
        //    {
        //        await dialogService.ShowMessage("Error", checkConnetion.Message);
        //        return;
        //    }

        //    var urlAPI = Application.Current.Resources["URLAPI"].ToString();

        //    var token = await apiService.GetToken(
        //        urlAPI,
        //        Email.Text,
        //        Password.Text);

        //    if (token == null)
        //    {
        //        await dialogService.ShowMessage("Error", "The user name or password in incorrect.");
        //        Password = null;
        //        return;
        //    }

        //    if (string.IsNullOrEmpty(token.AccessToken))
        //    {
        //        await dialogService.ShowMessage("Error", token.ErrorDescription);
        //        Password = null;
        //        return;
        //    }

        //    IsToken.IsVisible = false;
        //}
        #endregion
    }
}