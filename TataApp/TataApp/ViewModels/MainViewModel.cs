using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TataApp.ViewModels
{
    public class MainViewModel
    {
        #region Propierties
        public ObservableCollection<MenuItemViewModel> Menu { get; set; }

        public LoginViewModel Login { get; set; }
        #endregion

        public MainViewModel()
        {
            Menu = new ObservableCollection<MenuItemViewModel>();
            Login = new LoginViewModel();
            LoadMenu();
        }

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
    }
}
