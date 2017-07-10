namespace TataApp.Services
{
    using System.Threading.Tasks;
    using Views;
    public class NavigationService
    {
        public void SetMainPage(string pageName)
        {
            switch (pageName)
            {
                case "MasterPage":
                    App.Current.MainPage = new MasterPage();
                    break;
                case "LoginPage":
                    App.Current.MainPage = new LoginPage();
                    break;
                case "LoginFacebookPage":
                    App.Current.MainPage = new LoginFacebookPage();
                    break;
                case "NewEmployeePage":
                    App.Current.MainPage = new NewEmployeePage();
                    break;
                case "PasswordRecoveryPage":
                    App.Current.MainPage = new PasswordRecoveryPage();
                    break;
                default:
                    break;
            }
        }

        public async Task Navigate(string pageName)
        {
            App.Master.IsPresented = false;

            switch (pageName)
            {
                case "TimesPage":
                    await App.Navigator.PushAsync(new TimesPage());
                    break;
                case "NewTimePage":
                    await App.Navigator.PushAsync(new NewTimePage());
                    break;
                case "LocationsPage":
                    await App.Navigator.PushAsync(new LocationsPage());
                    break;
                case "EmployeesPage":
                    await App.Navigator.PushAsync(new EmployeesPage());
                    break;
                case "EmployeeDetailPage":
                    await App.Navigator.PushAsync(new EmployeeDetailPage());
                    break;
                case "ProfilePage":
                    await App.Navigator.PushAsync(new ProfilePage());
                    break;
                case "EditTimePage":
                    await App.Navigator.PushAsync(new EditTimePage());
                    break;
                default:
                    break;
            }
        }

        public async Task Back()
        {
            await App.Navigator.PopAsync();
        }
    }
}
