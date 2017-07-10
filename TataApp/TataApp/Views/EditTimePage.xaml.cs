namespace TataApp.Views
{
    using System;
    using Services;
    using Models;
    using ViewModels;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditTimePage : ContentPage
    {
        #region Attributes
        ApiService apiService;
        DialogService dialogService;
        #endregion

        #region Constructors
        public EditTimePage()
        {
            InitializeComponent();

            apiService = new ApiService();
            dialogService = new DialogService();
        }
        #endregion

        #region Methods
        void NewProjectClicked(object sender, EventArgs args)
        {
            ProjectDescriptionEdit.Text = string.Empty;
            ProjectModalEdit.IsVisible = true;
            ProjectDescriptionEdit.Focus();
        }

        async void SaveProjectClicked(object sender, EventArgs args)
        {
            if (string.IsNullOrEmpty(ProjectDescriptionEdit.Text))
            {
                await dialogService.ShowMessage("Error", "You must enter a project description.");
                ProjectDescriptionEdit.Focus();
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
            var project = new Project { Description = ProjectDescriptionEdit.Text, };

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

            ProjectModalEdit.IsVisible = false;
        }

        void CancelProjectClicked(object sender, EventArgs args)
        {
            ProjectModalEdit.IsVisible = false;
        }

        void NewActivityClicked(object sender, EventArgs args)
        {
            ActivityDescriptionEdit.Text = string.Empty;
            ActivityModalEdit.IsVisible = true;
            ActivityDescriptionEdit.Focus();
        }

        async void SaveActivityClicked(object sender, EventArgs args)
        {
            if (string.IsNullOrEmpty(ActivityDescriptionEdit.Text))
            {
                await dialogService.ShowMessage("Error", "You must enter an activity description.");
                ActivityDescriptionEdit.Focus();
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
            var activity = new Activity { Description = ActivityDescriptionEdit.Text, };

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

            ActivityModalEdit.IsVisible = false;
        }

        void CancelActivityClicked(object sender, EventArgs args)
        {
            ActivityModalEdit.IsVisible = false;
        }
        #endregion
    }
}