namespace TataApp.Views
{
    using Models;
    using Services;
    using System;
    using System.Threading.Tasks;
    using ViewModels;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePage : ContentPage
    {
        #region Attributes
        ApiService apiService;
        DialogService dialogService;
        DataService dataService;
        Employee employee;
        MainViewModel mainViewModel;
        #endregion

        #region Constructors
        public ProfilePage()
        {
            apiService = new ApiService();
            dialogService = new DialogService();
            dataService = new DataService();

            InitializeComponent();
        } 
        #endregion

        #region Methods
        void ChangePassword(object sender, EventArgs args)
        {
            mainViewModel = MainViewModel.GetInstance();
            employee = mainViewModel.Employee;

            CurrentPassword.Text = string.Empty;
            NewPassword.Text = string.Empty;
            ConfirmPassword.Text = string.Empty;
            PasswordModal.IsVisible = true;
            CurrentPassword.Focus();
        }

        async void SavePasswordClicked(object sender, EventArgs args)
        {
            if (string.IsNullOrEmpty(CurrentPassword.Text))
            {
                await dialogService.ShowMessage("Error", "You must enter the current password.");
                CurrentPassword.Focus();
                return;
            }

            if (string.IsNullOrEmpty(NewPassword.Text))
            {
                await dialogService.ShowMessage("Error", "You must enter the new password.");
                NewPassword.Focus();
                return;
            }

            if (string.IsNullOrEmpty(ConfirmPassword.Text))
            {
                await dialogService.ShowMessage("Error", "You must enter the confirm password.");
                ConfirmPassword.Focus();
                return;
            }

            if (!employee.Password.Equals(CurrentPassword.Text))
            {
                await dialogService.ShowMessage("Error", "Current passwword incorrect");
                CurrentPassword.Focus();
                return;
            }

            if (!NewPassword.Text.Equals(ConfirmPassword.Text))
            {
                await dialogService.ShowMessage("Error", "New password and confirm password must be equals");
                NewPassword.Focus();
                return;
            }

            var checkConnetion = await apiService.CheckConnection();
            if (!checkConnetion.IsSuccess)
            {
                await dialogService.ShowMessage("Error", checkConnetion.Message);
                return;
            }

            var urlAPI = Application.Current.Resources["URLAPI"].ToString();

            var changePassword = new ChangePasswordRequest
            {
                Email = employee.Email,
                CurrentPassword = CurrentPassword.Text,
                NewPassword = NewPassword.Text,
            };

            var response = await apiService.PostPassword<ChangePasswordRequest>(
                urlAPI,
                "/api",
                "/Employees/ChangePassword",
                employee.TokenType,
                employee.AccessToken,
                changePassword);

            if (!response.IsSuccess)
            {
                await dialogService.ShowMessage("Error", response.Message);
                return;
            }

            mainViewModel.Employee.Password = changePassword.NewPassword;
            dataService.DeleteAllAndInsert(mainViewModel.Employee);
            await dialogService.ShowMessage("Accept", "Password has been successfully");
            PasswordModal.IsVisible = false;
        }

        void CancelPasswordClicked(object sender, EventArgs args)
        {
            PasswordModal.IsVisible = false;
        }
        #endregion
    }
}