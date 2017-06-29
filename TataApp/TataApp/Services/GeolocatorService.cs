namespace TataApp.Services
{
    using Plugin.Geolocator;
    using System;
    using System.Threading.Tasks;
    public class GeolocatorService
    {
        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public async Task GetLocation()
        {
            try
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 50;
                var location = await locator.GetPositionAsync(timeoutMilliseconds: 30000);
                Latitude = location.Latitude;
                Longitude = location.Longitude;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
    }
}
