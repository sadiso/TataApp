using System.Threading.Tasks;
using TataApp.Views;

namespace TataApp.Services
{
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
                default:
                    break;
            }
        }
    }
}
