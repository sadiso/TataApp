namespace TataApp.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using System.ComponentModel;
    using System.Windows.Input;
    using Models;
    using Services;
    public class EmployeeDetailViewModel : Employee, INotifyPropertyChanged
    {
        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Attributes
        ApiService apiService;
        DialogService dialogService;
        bool isRunning;
        bool isEnabled;
        string message;
        Employee employee;
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

        public string Message
        {
            set
            {
                if (message != value)
                {
                    message = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Message"));
                }
            }
            get
            {
                return message;
            }
        }
        #endregion

        #region Constructors
        public EmployeeDetailViewModel(Employee employee)
        {
            this.employee = employee;

            EmployeeId = employee.EmployeeId;
            FirstName = employee.FirstName;
            LastName = employee.LastName;
            EmployeeCode = employee.EmployeeCode;
            DocumentTypeId = employee.DocumentTypeId;
            LoginTypeId = employee.LoginTypeId;
            Document = employee.Document;
            Picture = employee.Picture;
            Email = employee.Email;
            Phone = employee.Phone;
            Address = employee.Address;

            apiService = new ApiService();
            dialogService = new DialogService();

            IsEnabled = true;
        }
        #endregion

        #region Commands
        public ICommand SendMessageCommand
        {
            get { return new RelayCommand(SendMessage); }
        }

        async void SendMessage()
        {
            var checkConnetion = await apiService.CheckConnection();
            if (!checkConnetion.IsSuccess)
            {
                await dialogService.ShowMessage("Error", checkConnetion.Message);
                return;
            }

            if (string.IsNullOrEmpty(Message))
            {
                await dialogService.ShowMessage("Error", "You must enter a message to send.");
                return;
            }
        }
        #endregion
    }
}
