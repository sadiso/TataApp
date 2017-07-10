namespace TataApp.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Plugin.Media;
    using Plugin.Media.Abstractions;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Helpers;
    using Models;
    using Services;
    using Xamarin.Forms;
    public class ProfileViewModel : Employee, INotifyPropertyChanged
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
        int sourceIndex = -1;
        ImageSource imageSource;
        MediaFile file;
        Employee employee;
        List<DocumentType> documentTypes;
        Employee EditEmployee;
        byte[] imageArray = null;
        MainViewModel mainViewModel;
        #endregion

        #region Properties
        public ObservableCollection<DocumentTypeItemViewModel> DocumentTypes
        {
            get;
            set;
        }
        
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
        public ImageSource ImageSource
        {
            set
            {
                if (imageSource != value)
                {
                    imageSource = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ImageSource"));
                }
            }
            get
            {
                return imageSource;
            }
        }

        public int SourceIndex
        {
            set
            {
                if (sourceIndex != value)
                {
                    sourceIndex = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SourceIndex"));
                }
            }
            get
            {
                return sourceIndex;
            }
        }
        #endregion

        #region Constructors
        public ProfileViewModel()
        {
            apiService = new ApiService();
            dialogService = new DialogService();
            navigationService = new NavigationService();
            DocumentTypes = new ObservableCollection<DocumentTypeItemViewModel>();
            mainViewModel = MainViewModel.GetInstance();

            employee = mainViewModel.Employee;

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

            IsEnabled = true;

            LoadPickers();
        }
        #endregion

        #region Methods
        private async void LoadPickers()
        {
            IsEnabled = false;
            IsRunning = true;

            var checkConnetion = await apiService.CheckConnection();
            if (!checkConnetion.IsSuccess)
            {
                IsRunning = false;
                IsEnabled = true;
                await dialogService.ShowMessage("Error", checkConnetion.Message);
                return;
            }

            var urlAPI = Application.Current.Resources["URLAPI"].ToString();

            var DocumentTypesResponse = await apiService.GetList<DocumentType>(
                urlAPI,
                "/api",
                "/DocumentTypes",
                employee.TokenType,
                employee.AccessToken);

            if (DocumentTypesResponse.IsSuccess)
            {
                documentTypes = (List<DocumentType>)DocumentTypesResponse.Result;
                ReloadDocuments();
            }
            
            IsEnabled = true;
            IsRunning = false;
        }

        public void ReloadDocuments()
        {
            int index = 0;
            DocumentTypes.Clear();
            foreach (var document in documentTypes)
            {
                DocumentTypes.Add(new DocumentTypeItemViewModel
                {
                    Description = document.Description,
                    DocumentTypeId = document.DocumentTypeId,
                });

                if (document.DocumentTypeId == employee.DocumentTypeId)
                {
                    SourceIndex = index;
                }

                index += 1;
            }
        }
        public async Task GetUser()
        {
            var urlAPI = Application.Current.Resources["URLAPI"].ToString();

            var response = await apiService.GetEmployeeByEmailOrCode(
                urlAPI,
                "/api",
                "/Employees/GetGetEmployeeByEmailOrCode",
                employee.TokenType,
                employee.AccessToken,
                employee.Email);

            if (!response.IsSuccess)
            {
                IsRunning = false;
                IsEnabled = true;
                await dialogService.ShowMessage("Error", "Problem ocurred retrieving user information, try latter.");
                return;
            }

            var editEmployee = (Employee)response.Result;
            EditEmployee.Picture = editEmployee.Picture;
        }

        public void CreateUser()
        {
            
            if (file != null)
            {
                imageArray = FilesHelper.ReadFully(file.GetStream());
            }

            employee = mainViewModel.Employee;

            EditEmployee = new Employee
            {
                EmployeeId = employee.EmployeeId,
                FirstName = FirstName,
                LastName = LastName,
                EmployeeCode = EmployeeCode,
                DocumentTypeId = DocumentTypes.ElementAt(SourceIndex).DocumentTypeId,
                LoginTypeId = employee.LoginTypeId,
                Document = Document,
                Email = Email,
                Phone = Phone,
                Address = Address,
                ImageArray = imageArray,
                Password = employee.Password,
                AccessToken = employee.AccessToken,
                TokenType = employee.TokenType,
                TokenExpires = employee.TokenExpires,
                IsRemembered = employee.IsRemembered,
            };
        }
        #endregion

        #region Commands
        public ICommand TakePictureCommand
        {
            get { return new RelayCommand(TakePicture); }
        }

        private async void TakePicture()
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable ||
                !CrossMedia.Current.IsTakePhotoSupported)
            {
                await dialogService.ShowMessage("No Camera", "No camera available.");
                return;
            }

            file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                Directory = "Sample",
                Name = "test.jpg",
                PhotoSize = PhotoSize.Small,
            });

            IsRunning = true;

            if (file != null)
            {
                ImageSource = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    return stream;
                });
            }

            IsRunning = false;
        }

        public ICommand GalleryCommand
        {
            get { return new RelayCommand(Gallery); }
        }

        private async void Gallery()
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await dialogService.ShowMessage("No Gallery", "No gallery Supported.");
                return;
            }

            file = await CrossMedia.Current.PickPhotoAsync();

            IsRunning = true;

            if (file != null)
            {
                ImageSource = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    return stream;
                });
            }

            IsRunning = false;
        }

        public ICommand SaveCommand
        {
            get { return new RelayCommand(Save); }
        }

        private async void Save()
        {
            if (string.IsNullOrEmpty(FirstName))
            {
                await dialogService.ShowMessage("Error", "You must enter a first name.");
                return;
            }

            if (string.IsNullOrEmpty(LastName))
            {
                await dialogService.ShowMessage("Error", "You must enter a last name.");
                return;
            }
            if (EmployeeCode <= 0)
            {
                await dialogService.ShowMessage("Error", "You must enter an employee code.");
                return;
            }

            if (string.IsNullOrEmpty(Document))
            {
                await dialogService.ShowMessage("Error", "You must enter a document.");
                return;
            }

            if (string.IsNullOrEmpty(Email))
            {
                await dialogService.ShowMessage("Error", "You must enter an email.");
                return;
            }

            if (string.IsNullOrEmpty(Phone))
            {
                await dialogService.ShowMessage("Error", "You must enter a phone.");
                return;
            }

            if (string.IsNullOrEmpty(Address))
            {
                await dialogService.ShowMessage("Error", "You must enter an address.");
                return;
            }

            IsRunning = true;
            IsEnabled = false;

            CreateUser();

            var urlAPI = Application.Current.Resources["URLAPI"].ToString();

            var response = await apiService.Put<Employee>(
                urlAPI,
                "/api",
                "/Employees",
                employee.TokenType,
                employee.AccessToken,
                EditEmployee);

            if (!response.IsSuccess)
            {
                await dialogService.ShowMessage("Error", response.Message);
                IsRunning = false;
                IsEnabled = true;
                return;
            }

            await GetUser();

            if (file != null)
            {
                file.Dispose();
            }

            IsRunning = false;
            IsEnabled = true;

            mainViewModel.Employee = EditEmployee;
            navigationService.SetMainPage("MasterPage");
        }

        public ICommand ChangePasswordCommand
        {
            get { return new RelayCommand(ChangePassword); }
        }

        private void ChangePassword()
        {
            CreateUser();
            mainViewModel.Employee = employee; //EditEmployee;
        }
        #endregion
    }
}
