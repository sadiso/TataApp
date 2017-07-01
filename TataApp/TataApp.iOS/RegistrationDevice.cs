using TataApp.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(TataApp.iOS.RegistrationDevice))]
namespace TataApp.iOS
{
    public class RegistrationDevice : IRegisterDevice
    {
        public void RegisterDevice()
        {
        }
    }
}