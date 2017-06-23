using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TataApp.Models;
using TataApp.Services;
using Xamarin.Forms;

namespace TataApp.ViewModels
{
    public class TimesViewModel : INotifyPropertyChanged
    {
        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Attributes
        private ApiService apiService;
        private DialogService dialogService;
        private bool isRefreshing = false;
        private string filter;
        private List<Time> times;
        #endregion

        #region Properties
        public ObservableCollection<TimeItemViewModel> MyTimes
        {
            get;
            set;
        }

        public bool IsRefreshing
        {
            set
            {
                if (isRefreshing != value)
                {
                    isRefreshing = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsRefreshing"));
                }
            }
            get
            {
                return isRefreshing;
            }
        }

        public string Filter
        {
            set
            {
                if (filter != value)
                {
                    filter = value;
                    if (string.IsNullOrEmpty(filter))
                    {
                        ReloadTimes();
                    }
                    else
                    {
                        Search();
                    }

                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Filter"));
                }
            }
            get
            {
                return filter;
            }
        }
        #endregion

        #region Constructors
        public TimesViewModel()
        {
            instance = this;

            apiService = new ApiService();
            dialogService = new DialogService();

            MyTimes = new ObservableCollection<TimeItemViewModel>();
        }
        #endregion

        #region Singleton
        private static TimesViewModel instance;

        public static TimesViewModel GetInstance()
        {
            if (instance == null)
            {
                instance = new TimesViewModel();
            }

            return instance;
        }
        #endregion

        #region Methods
        private async Task LoadTimes()
        {
            IsRefreshing = true;
            var urlAPI = Application.Current.Resources["URLAPI"].ToString();
            var mainViewModel = MainViewModel.GetInstance();
            var employee = mainViewModel.Employee;
            var response = await apiService.GetList<Time>(
                urlAPI,
                "/api",
                "/Times",
                employee.TokenType,
                employee.AccessToken,
                employee.EmployeeId);

            if (!response.IsSuccess)
            {
                IsRefreshing = false;
                await dialogService.ShowMessage("Error", response.Message);
                return;
            }

            times = (List<Time>)response.Result;
            ReloadTimes();
            IsRefreshing = false;
        }

        private void ReloadTimes()
        {
            MyTimes.Clear();
            foreach (var time in times)
            {
                MyTimes.Add(new TimeItemViewModel
                {
                    Activity = time.Activity,
                    ActivityId = time.ActivityId,
                    DateRegistered = time.DateRegistered,
                    DateReported = time.DateReported,
                    EmployeeId = time.EmployeeId,
                    From = time.From,
                    Project = time.Project,
                    ProjectId = time.ProjectId,
                    Remarks = time.Remarks,
                    TimeId = time.TimeId,
                    To = time.To,
                });
            }
        }
        #endregion

        #region Commands
        public ICommand RefreshCommand { get { return new RelayCommand(Refresh); } }

        public async void Refresh()
        {
            await LoadTimes();
        }

        public ICommand SearchCommand { get { return new RelayCommand(Search); } }

        private void Search()
        {
            MyTimes.Clear();
            foreach (var time in times
                     .Where(t => t.Project.Description.ToLower().Contains(Filter.ToLower()) ||
                                 t.Activity.Description.ToLower().Contains(Filter.ToLower())))
            {
                MyTimes.Add(new TimeItemViewModel
                {
                    Activity = time.Activity,
                    ActivityId = time.ActivityId,
                    DateRegistered = time.DateRegistered,
                    DateReported = time.DateReported,
                    EmployeeId = time.EmployeeId,
                    From = time.From,
                    Project = time.Project,
                    ProjectId = time.ProjectId,
                    Remarks = time.Remarks,
                    TimeId = time.TimeId,
                    To = time.To,
                });
            }
        }
        #endregion
    }
}
