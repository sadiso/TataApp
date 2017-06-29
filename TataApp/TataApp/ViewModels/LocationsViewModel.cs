namespace TataApp.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using Models;
    using Services;
    using Xamarin.Forms;
    using Xamarin.Forms.Maps;

    public class LocationsViewModel
    {
        #region Attributes
        ApiService apiService;
        DialogService dialogService;
        #endregion

        #region Properties
        public ObservableCollection<Pin> Pins { get; set; }
        #endregion

        #region Constructors
        public LocationsViewModel()
        {
            instance = this;

            apiService = new ApiService();
            dialogService = new DialogService();

            Pins = new ObservableCollection<Pin>();
        }
        #endregion

        #region Singleton
        private static LocationsViewModel instance;

        public static LocationsViewModel GetInstance()
        {
            if (instance == null)
            {
                instance = new LocationsViewModel();
            }

            return instance;
        }
        #endregion

        #region Methods
        public async Task LoadPins()
        {
            var checkConnetion = await apiService.CheckConnection();
            if (!checkConnetion.IsSuccess)
            {
                await dialogService.ShowMessage("Error", checkConnetion.Message);
                return;
            }

            var urlAPI = Application.Current.Resources["URLAPI"].ToString();
            var mainViewModel = MainViewModel.GetInstance();
            var employee = mainViewModel.Employee;

            var response = await apiService.GetList<Location>(
                urlAPI,
                "/api",
                "/Locations",
                employee.TokenType,
                employee.AccessToken);

            if (!response.IsSuccess)
            {
                await dialogService.ShowMessage("Error", "Problem ocurred retrieving the locations, try latter.");
                return;
            }

            ReloadPins((List<Location>)response.Result);
        }

        void ReloadPins(List<Location> locations)
        {
            Pins.Clear();
            foreach (var location in locations)
            {
                var position = new Position(location.Latitude, location.Longitude);
                Pins.Add(new Pin
                {
                    Type = PinType.Generic,
                    Position = position,
                    Address = location.Address,
                    Label = location.Description,
                });
            }
        }
        #endregion
    }
}
