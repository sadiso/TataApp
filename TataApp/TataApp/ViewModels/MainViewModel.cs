using System.Collections.ObjectModel;
using TataApp.Models;

namespace TataApp.ViewModels
{
    public class MainViewModel
    {
        #region Propierties
        public ObservableCollection<MenuItemViewModel> Menu { get; set; }
        public Employee Employee { get; set; }
        public LoginViewModel Login { get; set; }
        #endregion

        #region Constructors
        public MainViewModel()
        {
            instance = this;
            Menu = new ObservableCollection<MenuItemViewModel>();
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
        private void LoadMenu()
        {
            Menu = new ObservableCollection<MenuItemViewModel>();

            Menu.Add(new MenuItemViewModel
            {
                Title = "Regiter Time",
                Icon = "ic_access_alarms.png",
                PageName = "RegisterTimePage",
            });

            Menu.Add(new MenuItemViewModel
            {
                Title = "Sickleaves",
                Icon = "ic_sentiment_dissatisfied.png",
                PageName = "SickleavesPage",
            });

            Menu.Add(new MenuItemViewModel
            {
                Title = "Localizate Employees",
                Icon = "ic_location_on.png",
                PageName = "LocalizatePage",
            });

            Menu.Add(new MenuItemViewModel
            {
                Title = "Close Section",
                Icon = "ic_close.png",
                PageName = "LoginPage",
            });
        } 
        #endregion
    }
}
