namespace TataApp.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Models;
    using Services;
    using System.Windows.Input;

    public class TimeItemViewModel : Time
    {
        NavigationService navigationService;
        public TimeItemViewModel()
        {
            navigationService = new NavigationService();
        }

        public ICommand EditTimeCommand
        {
            get { return new RelayCommand(EditTime); }
        }

        public async void EditTime()
        {
            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.EditTime = new EditTimeViewModel(this);
            await navigationService.Navigate("EditTimePage");
        }
    }
}
