using System;
using TataApp.Models;
using TataApp.Services;
using TataApp.ViewModels;
using TataApp.Views;
using Xamarin.Forms;

namespace TataApp
{
    public partial class App : Application
    {
        #region Attributes
        private DataService dataService;
        #endregion

        #region Properties
        public static NavigationPage Navigator { get; internal set; }
        public static MasterPage Master { get; internal set; }
        #endregion

        #region Constructors
        public App()
        {
            InitializeComponent();

            dataService = new DataService();

            var employee = dataService.First<Employee>(false);

            if (employee != null &&
                employee.IsRemembered &&
                employee.TokenExpires > DateTime.Now)
            {
                var mainViewModel = MainViewModel.GetInstance();
                mainViewModel.Employee = employee;
                MainPage = new MasterPage();
            }
            else
            {
                MainPage = new LoginPage();
            }
        } 
        #endregion

        #region Methods
        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        } 
        #endregion
    }
}
