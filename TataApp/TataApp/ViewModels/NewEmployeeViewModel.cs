namespace TataApp.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Helpers;
    using Models;
    using Newtonsoft.Json;
    using Plugin.Media;
    using Plugin.Media.Abstractions;
    using Services;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Net.Http;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class NewEmployeeViewModel : Employee , INotifyPropertyChanged
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
        string confirmPassword;
        ImageSource imageSource;
        MediaFile file;
        List<DocumentType> documentTypes;
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

        public string ConfirmPassword
        {
            set
            {
                if (confirmPassword != value)
                {
                    confirmPassword = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ConfirmPassword"));
                }
            }
            get
            {
                return confirmPassword;
            }
        }
        #endregion

        #region Constructors
        public NewEmployeeViewModel()
        {
            apiService = new ApiService();
            dialogService = new DialogService();
            navigationService = new NavigationService();
            DocumentTypes = new ObservableCollection<DocumentTypeItemViewModel>();

            IsEnabled = true;

            LoadPickers();
        }
        #endregion

        #region Methods
        private async void LoadPickers()
        {
            try
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
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlAPI);
                var url = "/api//DocumentTypes";
                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    await dialogService.ShowMessage("Error", response.RequestMessage.ToString());
                    IsRunning = false;
                    IsEnabled = true;
                    return;
                }

                var result = await response.Content.ReadAsStringAsync();
                documentTypes = JsonConvert.DeserializeObject<List<DocumentType>>(result);

                ReloadDocuments();

                IsEnabled = true;
                IsRunning = false;
            }
            catch (Exception ex)
            {
                await dialogService.ShowMessage("Error", ex.Message.ToString());
                IsRunning = false;
                IsEnabled = true;
                return;
            }
        }

        public void ReloadDocuments()
        {
            
            DocumentTypes.Clear();
            foreach (var document in documentTypes)
            {
                DocumentTypes.Add(new DocumentTypeItemViewModel
                {
                    Description = document.Description,
                    DocumentTypeId = document.DocumentTypeId,
                });
            }
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

            if (string.IsNullOrEmpty(Password))
            {
                await dialogService.ShowMessage("Error", "You must enter a password.");
                return;
            }

            if (string.IsNullOrEmpty(ConfirmPassword))
            {
                await dialogService.ShowMessage("Error", "You must enter a confirm password.");
                return;
            }

            if (!Password.Equals(ConfirmPassword))
            {
                await dialogService.ShowMessage("Error", "Password and confirm password must be equals");
                return;
            }

            if (SourceIndex <= 0)
            {
                await dialogService.ShowMessage("Error", "Select a document type");
                return;
            }

            IsRunning = true;
            IsEnabled = false;

            byte[] imageArray = null;

            if (file != null)
            {
                imageArray = FilesHelper.ReadFully(file.GetStream());
                file.Dispose();
            }

            var NewEmployee = new Employee
            {
                EmployeeId = 0,
                FirstName = FirstName,
                LastName = LastName,
                EmployeeCode = EmployeeCode,
                DocumentTypeId = DocumentTypeId,
                LoginTypeId = 1,
                Document = Document,
                Email = Email,
                Phone = Phone,
                Address = Address,
                ImageArray = imageArray,
                Password = Password,
                //AccessToken = employee.AccessToken,
                //TokenType = employee.TokenType,
                //TokenExpires = employee.TokenExpires,
                //IsRemembered = employee.IsRemembered,
            };

            var urlAPI = Application.Current.Resources["URLAPI"].ToString();

            var response = await apiService.Post<Employee>(
                urlAPI,
                "/api",
                "/Employees",
                NewEmployee);

            if (!response.IsSuccess)
            {
                await dialogService.ShowMessage("Error", response.Message);
                IsRunning = false;
                IsEnabled = true;
                return;
            }

            IsRunning = false;
            IsEnabled = true;
        }
        #endregion
    }
}
