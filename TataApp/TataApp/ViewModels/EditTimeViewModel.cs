namespace TataApp.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Models;
    using Services;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class EditTimeViewModel : Time, INotifyPropertyChanged
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
        int projectIndex;
        int activityIndex;
        Time editTime;
        string urlAPI;
        MainViewModel mainViewModel;
        Employee employee;
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
        public int ProjectIndex
        {
            set
            {
                if (projectIndex != value)
                {
                    projectIndex = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ProjectIndex"));
                }
            }
            get
            {
                return projectIndex;
            }
        }
        public int ActivityIndex
        {
            set
            {
                if (activityIndex != value)
                {
                    activityIndex = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ActivityIndex"));
                }
            }
            get
            {
                return activityIndex;
            }
        }
        #endregion

        #region Constructors
        public EditTimeViewModel(Time time)
        {
            editTime = time;

            apiService = new ApiService();
            dialogService = new DialogService();
            navigationService = new NavigationService();
            geolocatorService = new GeolocatorService();
            Projects = new ObservableCollection<ProjectItemViewModel>();
            Activities = new ObservableCollection<ActivityItemViewModel>();

            urlAPI = Application.Current.Resources["URLAPI"].ToString();
            mainViewModel = MainViewModel.GetInstance();
            employee = mainViewModel.Employee;

            LoadPickers();
            //AsyncHelpers.RunSync(() => LoadPickers());

            //Activity = time.Activity;
            //ActivityId = time.ActivityId;
            //DateRegistered = time.DateRegistered;
            DateReported = time.DateReported;
            //EmployeeId = time.EmployeeId;
            FromString = time.From.ToString().Substring(10, 6);
            //Project = time.Project;
            //ProjectId = time.ProjectId;
            Remarks = time.Remarks;
            //TimeId = time.TimeId;
            ToString = time.To.ToString().Substring(10, 6);
            
            IsEnabled = true;
        }
        #endregion

        #region Methods
        public async void LoadPickers()
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
            var index = 0;
            Projects.Clear();
            foreach (var project in projects.OrderBy(p => p.Description))
            {
                
                Projects.Add(new ProjectItemViewModel
                {
                    Description = project.Description,
                    ProjectId = project.ProjectId,
                });

                if (project.ProjectId == editTime.ProjectId)
                {
                    ProjectIndex = index;
                }

                index++;
            }
        }

        public void ReloadActivities()
        {
            var index = 0;
            Activities.Clear();
            foreach (var activity in activities.OrderBy(a => a.Description))
            {
                Activities.Add(new ActivityItemViewModel
                {
                    Description = activity.Description,
                    ActivityId = activity.ActivityId,
                });

                if (activity.ActivityId == editTime.ActivityId)
                {
                    ActivityIndex = index;
                }

                index++;
            }
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

            await geolocatorService.GetLocation();

            var activitySelected = Activities.ElementAt(ActivityIndex);
            var projectSelected = Projects.ElementAt(ProjectIndex);

            var time = new Time
            {
                Activity = activitySelected,
                ActivityId = activitySelected.ActivityId,
                DateRegistered = DateTime.Today,
                DateReported = DateReported,
                EmployeeId = employee.EmployeeId,
                From = From,
                Project = projectSelected,
                ProjectId = projectSelected.ProjectId,
                Remarks = Remarks,
                TimeId = editTime.TimeId,
                To = To,
            };

            var response = await apiService.Put(
                urlAPI,
                "/api",
                "/Times",
                employee.TokenType,
                employee.AccessToken,
                time);

            IsEnabled = true;
            IsRunning = false;

            if (!response.IsSuccess)
            {
                await dialogService.ShowMessage("Error", response.Message);
                return;
            }

            await navigationService.Back();
        }

        public ICommand DeleteCommand
        {
            get { return new RelayCommand(Delete); }
        }

        async void Delete()
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

            var response = await apiService.Delete(
                urlAPI,
                "/api",
                "/Times",
                employee.TokenType,
                employee.AccessToken,
                editTime);

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
