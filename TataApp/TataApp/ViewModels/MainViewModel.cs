namespace TataApp.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using Models;
    using Services;
    using Xamarin.Forms;
    using Interfaces;

    public class MainViewModel
    {
        #region Attributes
        NavigationService navigationService;
        #endregion

        #region Propierties
        public ObservableCollection<MenuItemViewModel> Menu { get; set; }
        public Employee Employee { get; set; }
        public LoginViewModel Login { get; set; }
        public TimesViewModel Times { get; set; }
        public NewTimeViewModel NewTime { get; set; }
        public LocationsViewModel Locations { get; set; }
        public EmployeesViewModel Employees { get; set; }
        public EmployeeDetailViewModel EmployeeDetail { get; set; }
        public ProfileViewModel Profile { get; set; }
        #endregion

        #region Constructors
        public MainViewModel()
        {
            instance = this;
            Menu = new ObservableCollection<MenuItemViewModel>();
            navigationService = new NavigationService();
            Login = new LoginViewModel();
            LoadMenu();
        }
        #endregion

        #region Singleton
        private static MainViewModel instance;

        public static MainViewModel GetInstance()
        {
            if (instance == null)
            {
                instance = new MainViewModel();
            }

            return instance;
        }
        #endregion

        #region Methods
        public void RegisterDevice()
        {
            var register = DependencyService.Get<IRegisterDevice>();
            register.RegisterDevice();
        }
        private void LoadMenu()
        {
            Menu = new ObservableCollection<MenuItemViewModel>();

            Menu.Add(new MenuItemViewModel
            {
                Title = "Regiter Time",
                Icon = "ic_access_alarms.png",
                PageName = "TimesPage",
            });

            Menu.Add(new MenuItemViewModel
            {
                Title = "Employees",
                Icon = "ic_stat_person_pin.png",
                PageName = "EmployeesPage",
            });

            Menu.Add(new MenuItemViewModel
            {
                Title = "Localizate Employees",
                Icon = "ic_location_on.png",
                PageName = "LocationsPage",
            });

            Menu.Add(new MenuItemViewModel
            {
                Title = "My Profile",
                Icon = "ic_profile.png",
                PageName = "ProfilePage",
            });

            Menu.Add(new MenuItemViewModel
            {
                Title = "Close Section",
                Icon = "ic_close.png",
                PageName = "LoginPage",
            });
        }
        #endregion

        #region Commands
        public ICommand NewTimeCommand
        {
            get { return new RelayCommand(GoNewTime); }
        }

        async void GoNewTime()
        {
            NewTime = new NewTimeViewModel();
            await navigationService.Navigate("NewTimePage");
        }
        #endregion
    }
}
