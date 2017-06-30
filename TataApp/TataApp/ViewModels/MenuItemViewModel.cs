namespace TataApp.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using System.Windows.Input;
    using TataApp.Services;

    public class MenuItemViewModel
    {
        #region Attributes
        private NavigationService navigationService;
        #endregion

        #region Properties
        public string Icon { get; set; }
        public string Title { get; set; }
        public string PageName { get; set; }
        #endregion

        #region Constructors
        public MenuItemViewModel()
        {
            navigationService = new NavigationService();
        }
        #endregion

        #region Commands
        public ICommand NavigateCommand
        {
            get { return new RelayCommand(Navigate); }
        }

        private async void Navigate()
        {
            if (PageName == "LoginPage")
            {
                navigationService.SetMainPage("LoginPage");
            }
            else
            {
                var mainViewModel = MainViewModel.GetInstance();

                switch (PageName)
                {
                    case "TimesPage":
                        mainViewModel.Times = new TimesViewModel();
                        break;
                    case "LocationsPage":
                        mainViewModel.Locations = new LocationsViewModel();
                        break;
                    case "EmployeesPage":
                        mainViewModel.Employees = new EmployeesViewModel();
                        break;
                    default:
                        break;
                }

                await navigationService.Navigate(PageName);
            }
        }
        #endregion
    }
}
