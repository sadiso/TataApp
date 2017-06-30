namespace TataApp.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using System.Windows.Input;
    using Models;
    using Services;
    public class EmployeeItemViewModel : Employee
    {
        #region Attributes
        NavigationService navigationService;
        #endregion

        #region Attributes
        public EmployeeItemViewModel()
        {
            navigationService = new NavigationService();
        }
        #endregion

        #region Commands
        public ICommand SelectEmployeeCommand
        {
            get { return new RelayCommand(SelectEmployee); }
        }

        async void SelectEmployee()
        {
            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.EmployeeDetail = new EmployeeDetailViewModel(this);
            await navigationService.Navigate("EmployeeDetailPage");
        }
        #endregion
    }
}
