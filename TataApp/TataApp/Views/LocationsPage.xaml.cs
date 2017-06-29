

namespace TataApp.Views
{
    using System;
    using System.Threading.Tasks;
    using Services;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;
    using Xamarin.Forms.Maps;
    using ViewModels;
    using Models;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LocationsPage : ContentPage
    {
        #region Attributes
        GeolocatorService geolocatorService;
        ApiService apiService;
        DialogService dialogService;
        #endregion

        #region Constructors
        public LocationsPage()
        {
            InitializeComponent();

            geolocatorService = new GeolocatorService();
            apiService = new ApiService();
            dialogService = new DialogService();

            MoveToCurrentLocation();
        }
        #endregion

        #region Methods
        async void MoveToCurrentLocation()
        {
            await geolocatorService.GetLocation();
            if (geolocatorService.Latitude != 0 && geolocatorService.Longitude != 0)
            {
                var position = new Position(geolocatorService.Latitude, geolocatorService.Longitude);
                MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(position, Distance.FromMiles(.3)));
            }

            await ShowPoins();
        }

        async Task ShowPoins()
        {
            var locationsViewModel = LocationsViewModel.GetInstance();
            await locationsViewModel.LoadPins();
            foreach (var pin in locationsViewModel.Pins)
            {
                MyMap.Pins.Add(pin);
            }
        }

        void NewPinClicked(object sender, EventArgs args)
        {
            PinDescription.Text = string.Empty;
            PinAddress.Text = string.Empty;
            NewPinModal.IsVisible = true;
            PinDescription.Focus();
        }

        async void SavePinClicked(object sender, EventArgs args)
        {
            if (string.IsNullOrEmpty(PinDescription.Text))
            {
                await dialogService.ShowMessage("Error", "You must enter a Pin description.");
                return;
            }

            if (string.IsNullOrEmpty(PinAddress.Text))
            {
                await dialogService.ShowMessage("Error", "You must enter a Pin address.");
                return;
            }

            await geolocatorService.GetLocation();
            if (geolocatorService.Latitude == 0 && geolocatorService.Longitude == 0)
            {
                await dialogService.ShowMessage("Error", "The localization in not available.");
                return;
            }

            var location = new Location
            {
                Address = PinAddress.Text,
                Description = PinDescription.Text,
                Latitude = geolocatorService.Latitude,
                Longitude = geolocatorService.Longitude,
            };

            var urlAPI = Application.Current.Resources["URLAPI"].ToString();
            var mainViewModel = MainViewModel.GetInstance();
            var employee = mainViewModel.Employee;

            var response = await apiService.Post(
                urlAPI,
                "/api",
                "/Locations",
                employee.TokenType,
                employee.AccessToken,
                location);

            if (!response.IsSuccess)
            {
                await dialogService.ShowMessage("Error", "Problem ocurred saving the new location.");
                return;
            }

            var position = new Position(geolocatorService.Latitude, geolocatorService.Longitude);

            var pin = new Pin
            {
                Address = PinAddress.Text,
                Label = PinDescription.Text,
                Position = position,
                Type = PinType.Place,
            };

            var locationsViewModel = LocationsViewModel.GetInstance();
            locationsViewModel.Pins.Add(pin);
            MyMap.Pins.Add(pin);

            NewPinModal.IsVisible = false;
        }

        void CancelPinClicked(object sender, EventArgs args)
        {
            NewPinModal.IsVisible = false;
        }
        #endregion
    }
}