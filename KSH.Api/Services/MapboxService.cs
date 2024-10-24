using KSH.Api.Services.IServices;
using Newtonsoft.Json.Linq;

namespace KSH.Api.Services
{
    public class MapboxService : IMapboxService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        public MapboxService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<ServiceResponse> GetDistanceBetweenAnAddressAndShop(string address)
        {
            var serviceResponse = new ServiceResponse();
            try
            {
                var (shopLong, shopLat) = (_configuration.GetValue<double>("KitStemHub:Coordinates:Longitude"), _configuration.GetValue<double>("KitStemHub:Coordinates:Latitude"));
                var (addressLong, addressLat) = await GetCoordinatesAsync(address);
                if (addressLong == null || addressLat == null)
                {
                    return serviceResponse
                       .SetSucceeded(false)
                       .AddDetail("message", "Lâý khoảng cách không thành công!")
                       .AddError("notFound", "Không thể xác định được địa chỉ cấn lấy khoảng cách!");
                }

                const double R = 6371e3; // Earth's radius in meters
                double phi1 = (double)(addressLat * Math.PI / 180); // Convert latitude to radians
                double phi2 = shopLat * Math.PI / 180; // Convert latitude to radians
                double deltaPhi = (double)((shopLat - addressLat) * Math.PI / 180); // Difference in latitude
                double deltaLambda = (double)((shopLong - addressLong) * Math.PI / 180); // Difference in longitude

                // Haversine formula
                double a = Math.Sin(deltaPhi / 2) * Math.Sin(deltaPhi / 2) +
                           Math.Cos(phi1) * Math.Cos(phi2) *
                           Math.Sin(deltaLambda / 2) * Math.Sin(deltaLambda / 2);
                double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

                double distance = R * c; // Distance in meters
                return serviceResponse
                        .AddDetail("message", "Lâý khoảng cách thành công!")
                        .AddDetail("distance", Math.Ceiling(distance / 1000)); // Returns distance in km
            }
            catch
            {
                return serviceResponse
                        .SetSucceeded(false)
                        .AddDetail("message", "Lâý khoảng cách không thành công!")
                        .AddError("outOfService", "Không thể lấy được địa chỉ ngay lúc này!");
            }
        }

        private async Task<(double?, double?)> GetCoordinatesAsync(string address)
        {
            if (string.IsNullOrEmpty(address))
            {
                return (null, null);
            }
            var requestUri = $"{_configuration["Mapbox:GeocodingUrl"]}/forward?q={Uri.EscapeDataString(address)}&access_token={_configuration["Mapbox:AccessToken"]}";

            try
            {
                var response = await _httpClient.GetAsync(requestUri);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var json = JObject.Parse(content);

                var coordinates = json["features"]?[0]?["geometry"]?["coordinates"];
                if (coordinates == null)
                {
                    return (null, null);
                }

                var longitude = (double)coordinates[0]!;
                var latitude = (double)coordinates[1]!;

                return (longitude, latitude);
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Error calling Mapbox API: {ex.Message}", ex);
            }
        }
    }
}