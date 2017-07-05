using GalaSoft.MvvmLight.Command;
using System.ComponentModel;
using System.Windows.Input;
using TataApp.Models;
using TataApp.Services;
using Xamarin.Forms;

namespace TataApp.ViewModels
{
    public class PasswordRecoveryViewModel : INotifyPropertyChanged
    {
        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Attributes
        ApiService apiService;
        DialogService dialogService;
        NavigationService navigationService;
        bool isRunning;
        bool isEnabled;
        string email;
        #endregion

        #region Properties
        public bool IsRunning
        {
            set
            {
                if (isRunning != value)
                {
                    isRunning = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsRunning"));
                }
            }
            get
            {
                return isRunning;
            }
        }

        public bool IsEnabled
        {
            set
            {
                if (isEnabled != value)
                {
                    isEnabled = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsEnabled"));
                }
            }
            get
            {
                return isEnabled;
            }
        }
        
        public string Email
        {
            set
            {
                if (email != value)
                {
                    email = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Email"));
                }
            }
            get
            {
                return email;
            }
        }
        #endregion

        #region Constructors
        public PasswordRecoveryViewModel()
        {
            apiService = new ApiService();
            dialogService = new DialogService();
            navigationService = new NavigationService();

            IsEnabled = true;
        }
        #endregion

        #region Commands
        public ICommand SendPasswordCommand
        {
            get { return new RelayCommand(SendPassword); }
        }

        private async void SendPassword()
        {
            if (string.IsNullOrEmpty(Email))
            {
                await dialogService.ShowMessage("Error", "You must enter an email.");
                return;
            }

            IsRunning = true;
            IsEnabled = false;

            var passwordRecovery = new PasswordRecovery
            {
                Email = Email,
            };
           var urlAPI = Application.Current.Resources["URLAPI"].ToString();

            var response = await apiService.PostPasswordRecoverybyEmail(
                urlAPI,
                "/api",
                "/Employees/PasswordRecovery",
                passwordRecovery);

            if (!response.IsSuccess)
            {
                await dialogService.ShowMessage("Error", response.Message);
                IsRunning = false;
                IsEnabled = true;
                return;
            }

            await dialogService.ShowMessage("Accept", "Your new password has been send, check email.");

            IsRunning = false;
            IsEnabled = true;
        }
        #endregion
    }
}
