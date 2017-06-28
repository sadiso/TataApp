namespace TataApp.Views
{
    using System;
    using Services;
    using Models;
    using ViewModels;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewTimeTimePage : ContentPage
    {
        #region Attributes
        ApiService apiService;
        DialogService dialogService;
        #endregion

        #region Constructors
        public NewTimeTimePage()
        {
            InitializeComponent();

            apiService = new ApiService();
            dialogService = new DialogService();
        }
        #endregion

        #region Methods
        void NewProjectClicked(object sender, EventArgs args)
        {
            ProjectDescription.Text = string.Empty;
            ProjectModal.IsVisible = true;
            ProjectDescription.Focus();
        }

        async void SaveProjectClicked(object sender, EventArgs args)
        {
            if (string.IsNullOrEmpty(ProjectDescription.Text))
            {
                await dialogService.ShowMessage("Error", "You must enter a project description.");
                ProjectDescription.Focus();
                return;
            }

            var checkConnetion = await apiService.CheckConnection();
            if (!checkConnetion.IsSuccess)
            {
                await dialogService.ShowMessage("Error", checkConnetion.Message);
                return;
            }

            var urlAPI = Application.Current.Resources["URLAPI"].ToString();
            var mainViewModel = MainViewModel.GetInstance();
            var employee = mainViewModel.Employee;
            var project = new Project { Description = ProjectDescription.Text, };

            var response = await apiService.Post(
                urlAPI,
                "/api",
                "/Projects",
                employee.TokenType,
                employee.AccessToken,
                project);

            if (!response.IsSuccess)
            {
                await dialogService.ShowMessage("Error", response.Message);
                return;
            }

            var newTimeViewModel = NewTimeViewModel.GetInstance();
            newTimeViewModel.projects.Add((Project)response.Result);
            newTimeViewModel.ReloadProjects();

            ProjectModal.IsVisible = false;
        }

        void CancelProjectClicked(object sender, EventArgs args)
        {
            ProjectModal.IsVisible = false;
        }

        void NewActivityClicked(object sender, EventArgs args)
        {
            ActivityDescription.Text = string.Empty;
            ActivityModal.IsVisible = true;
            ActivityDescription.Focus();
        }

        async void SaveActivityClicked(object sender, EventArgs args)
        {
            if (string.IsNullOrEmpty(ActivityDescription.Text))
            {
                await dialogService.ShowMessage("Error", "You must enter an activity description.");
                ActivityDescription.Focus();
                return;
            }

            var checkConnetion = await apiService.CheckConnection();
            if (!checkConnetion.IsSuccess)
            {
                await dialogService.ShowMessage("Error", checkConnetion.Message);
                return;
            }

            var urlAPI = Application.Current.Resources["URLAPI"].ToString();
            var mainViewModel = MainViewModel.GetInstance();
            var employee = mainViewModel.Employee;
            var activity = new Activity { Description = ActivityDescription.Text, };

            var response = await apiService.Post(
                urlAPI,
                "/api",
                "/Activities",
                employee.TokenType,
                employee.AccessToken,
                activity);

            if (!response.IsSuccess)
            {
                await dialogService.ShowMessage("Error", response.Message);
                return;
            }

            var newTimeViewModel = NewTimeViewModel.GetInstance();
            newTimeViewModel.activities.Add((Activity)response.Result);
            newTimeViewModel.ReloadActivities();

            ActivityModal.IsVisible = false;
        }

        void CancelActivityClicked(object sender, EventArgs args)
        {
            ActivityModal.IsVisible = false;
        }
        #endregion
    }
}