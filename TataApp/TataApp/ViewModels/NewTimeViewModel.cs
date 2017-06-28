namespace TataApp.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using Models;
    using Services;
    using Xamarin.Forms;
    using System.Linq;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;

    public class NewTimeViewModel : Time, INotifyPropertyChanged
    {
        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Attributes
        ApiService apiService;
        DialogService dialogService;
        NavigationService navigationService;
        GeolocatorService geolocatorService;
        bool isRunning;
        bool isEnabled;
        bool isRepeated;
        bool isRepeatMonday;
        bool isRepeatTuesday;
        bool isRepeatWednesday;
        bool isRepeatThursday;
        bool isRepeatFriday;
        bool isRepeatSaturday;
        bool isRepeatSunday;
        public List<Project> projects;
        public List<Activity> activities;
        #endregion

        #region Properties
        public ObservableCollection<ProjectItemViewModel> Projects
        {
            get;
            set;
        }

        public ObservableCollection<ActivityItemViewModel> Activities
        {
            get;
            set;
        }

        public string FromString
        {
            get;
            set;
        }

        public string ToString
        {
            get;
            set;
        }

        public DateTime Until
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

        public bool IsRepeated
        {
            set
            {
                if (isRepeated != value)
                {
                    isRepeated = value;
                    if (!isRepeated)
                    {
                        TurnOffDays();
                    }
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsRepeated"));
                }
            }
            get
            {
                return isRepeated;
            }
        }

        public bool IsRepeatMonday
        {
            set
            {
                if (isRepeatMonday != value)
                {
                    isRepeatMonday = value;
                    if (value)
                    {
                        IsRepeated = true;
                    }
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsRepeatMonday"));
                }
            }
            get
            {
                return isRepeatMonday;
            }
        }

        public bool IsRepeatTuesday
        {
            set
            {
                if (isRepeatTuesday != value)
                {
                    isRepeatTuesday = value;
                    if (value)
                    {
                        IsRepeated = true;
                    }
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsRepeatTuesday"));
                }
            }
            get
            {
                return isRepeatTuesday;
            }
        }

        public bool IsRepeatWednesday
        {
            set
            {
                if (isRepeatWednesday != value)
                {
                    isRepeatWednesday = value;
                    if (value)
                    {
                        IsRepeated = true;
                    }
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsRepeatWednesday"));
                }
            }
            get
            {
                return isRepeatWednesday;
            }
        }

        public bool IsRepeatThursday
        {
            set
            {
                if (isRepeatThursday != value)
                {
                    isRepeatThursday = value;
                    if (value)
                    {
                        IsRepeated = true;
                    }
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsRepeatThursday"));
                }
            }
            get
            {
                return isRepeatThursday;
            }
        }

        public bool IsRepeatFriday
        {
            set
            {
                if (isRepeatFriday != value)
                {
                    isRepeatFriday = value;
                    if (value)
                    {
                        IsRepeated = true;
                    }
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsRepeatFriday"));
                }
            }
            get
            {
                return isRepeatFriday;
            }
        }

        public bool IsRepeatSaturday
        {
            set
            {
                if (isRepeatSaturday != value)
                {
                    isRepeatSaturday = value;
                    if (value)
                    {
                        IsRepeated = true;
                    }
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsRepeatSaturday"));
                }
            }
            get
            {
                return isRepeatSaturday;
            }
        }

        public bool IsRepeatSunday
        {
            set
            {
                if (isRepeatSunday != value)
                {
                    isRepeatSunday = value;
                    if (value)
                    {
                        IsRepeated = true;
                    }
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsRepeatSunday"));
                }
            }
            get
            {
                return isRepeatSunday;
            }
        }
        #endregion

        #region Constructor
        public NewTimeViewModel()
        {
            instance = this;

            apiService = new ApiService();
            dialogService = new DialogService();
            navigationService = new NavigationService();
            geolocatorService = new GeolocatorService();

            IsEnabled = true;
            Until = DateTime.Today;
            DateReported = DateTime.Today;

            Projects = new ObservableCollection<ProjectItemViewModel>();
            Activities = new ObservableCollection<ActivityItemViewModel>();

            LoadPickers();
        }
        #endregion

        #region Singleton
        private static NewTimeViewModel instance;

        public static NewTimeViewModel GetInstance()
        {
            if (instance == null)
            {
                instance = new NewTimeViewModel();
            }

            return instance;
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
            var mainViewModel = MainViewModel.GetInstance();
            var employee = mainViewModel.Employee;

            var projectsResponse = await apiService.GetList<Project>(
                urlAPI,
                "/api",
                "/Projects",
                employee.TokenType,
                employee.AccessToken);

            if (projectsResponse.IsSuccess)
            {
                projects = (List<Project>)projectsResponse.Result;
                ReloadProjects();
            }

            var activitiesResponse = await apiService.GetList<Activity>(
                urlAPI,
                "/api",
                "/Activities",
                employee.TokenType,
                employee.AccessToken);

            if (activitiesResponse.IsSuccess)
            {
                activities = (List<Activity>)activitiesResponse.Result;
                ReloadActivities();
            }

            IsEnabled = true;
            IsRunning = false;
        }

        public void ReloadProjects()
        {
            Projects.Clear();
            foreach (var project in projects.OrderBy(p => p.Description))
            {
                Projects.Add(new ProjectItemViewModel
                {
                    Description = project.Description,
                    ProjectId = project.ProjectId,
                });
            }
        }

        public void ReloadActivities()
        {
            Activities.Clear();
            foreach (var activity in activities.OrderBy(a => a.Description))
            {
                Activities.Add(new ActivityItemViewModel
                {
                    Description = activity.Description,
                    ActivityId = activity.ActivityId,
                });
            }
        }

        private void TurnOffDays()
        {
            IsRepeatMonday = false;
            IsRepeatTuesday = false;
            IsRepeatWednesday = false;
            IsRepeatThursday = false;
            IsRepeatFriday = false;
            IsRepeatSaturday = false;
            IsRepeatSunday = false;
        }
        private void ConvertHours()
        {
            int posTo = ToString.IndexOf(':');
            int posFrom = FromString.IndexOf(':');
            int toHour = 0, toMinute = 0, fromHour = 0, fromMinute = 0;

            if (posTo == -1)
            {
                int.TryParse(ToString, out toHour);
            }
            else
            {
                int.TryParse(ToString.Substring(0, posTo), out toHour);
                int.TryParse(ToString.Substring(posTo + 1), out toMinute);
            }

            if (posFrom == -1)
            {
                int.TryParse(FromString, out fromHour);
            }
            else
            {
                int.TryParse(FromString.Substring(0, posFrom), out fromHour);
                int.TryParse(FromString.Substring(posFrom + 1), out fromMinute);
            }

            To = new DateTime(1900, 1, 1, toHour, toMinute, 0);
            From = new DateTime(1900, 1, 1, fromHour, fromMinute, 0);
        }
        #endregion

        #region Commands
        public ICommand SaveCommand
        {
            get { return new RelayCommand(Save); }
        }

        async void Save()
        {
            if (ProjectId == 0)
            {
                await dialogService.ShowMessage("Error", "You must select a project.");
                return;
            }

            if (ActivityId == 0)
            {
                await dialogService.ShowMessage("Error", "You must select an activity.");
                return;
            }

            ConvertHours();

            if (To <= From)
            {
                await dialogService.ShowMessage("Error", "The hour 'To' must be greather hour 'From'.");
                return;
            }

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
            var mainViewModel = MainViewModel.GetInstance();
            var employee = mainViewModel.Employee;

            await geolocatorService.GetLocation();

            var newTimeRequest = new NewTimeRequest
            {
                ActivityId = ActivityId,
                DateReported = DateReported,
                EmployeeId = employee.EmployeeId,
                From = From,
                Latitude = geolocatorService.Latitude,
                Longitude = geolocatorService.Longitude,
                IsRepeated = IsRepeated,
                IsRepeatFriday = IsRepeatFriday,
                IsRepeatMonday = IsRepeatMonday,
                IsRepeatSaturday = IsRepeatSaturday,
                IsRepeatSunday = IsRepeatSunday,
                IsRepeatThursday = IsRepeatThursday,
                IsRepeatTuesday = IsRepeatTuesday,
                IsRepeatWednesday = IsRepeatWednesday,
                ProjectId = ProjectId,
                Remarks = Remarks,
                To = To,
                Until = Until,
            };

            var response = await apiService.Post(
                urlAPI,
                "/api",
                "/Times",
                employee.TokenType,
                employee.AccessToken,
                newTimeRequest);

            IsEnabled = true;
            IsRunning = false;

            if (!response.IsSuccess)
            {
                await dialogService.ShowMessage("Error", response.Message);
                return;
            }

            await navigationService.Back();
        }
        #endregion
    }
}
