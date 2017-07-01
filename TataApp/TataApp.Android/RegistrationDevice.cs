using TataApp.Interfaces;
using Xamarin.Forms;
using Gcm.Client;
using Android.Util;

[assembly: Dependency(typeof(TataApp.Droid.RegistrationDevice))]
namespace TataApp.Droid
{
    public class RegistrationDevice : IRegisterDevice
    {
        #region Methods
        public void RegisterDevice()
        {
            var mainActivity = MainActivity.GetInstance();
            GcmClient.CheckDevice(mainActivity);
            GcmClient.CheckManifest(mainActivity);

            Log.Info("MainActivity", "Registering...");
            GcmClient.Register(mainActivity, Constants.SenderID);
        }
        #endregion
    }
}