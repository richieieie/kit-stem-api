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

                var distance = await GetDistanceAsync(shopLat, shopLong, (double)addressLat, (double)addressLong);
                if (distance == null)
                {
                    return serviceResponse
                       .SetSucceeded(false)
                       .AddDetail("message", "Lâý khoảng cách không thành công!")
                       .AddError("notFound", "Không thể xác định được địa chỉ cấn lấy khoảng cách!");
                }

                return serviceResponse
                        .AddDetail("message", "Lâý khoảng cách thành công!")
                        .AddDetail("distance", distance); // Returns distance in km
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
            var requestUri = $"{_configuration["Mapbox:GeocodingUrl"]}/forward?q={Uri.EscapeDataString(address)}&country=vn&access_token={_configuration["Mapbox:AccessToken"]}";

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

        private async Task<double?> GetDistanceAsync(double startLat, double startLon, double endLat, double endLon)
        {
            var startLongitude = Uri.EscapeDataString(startLon.ToString());
            var startLatitude = Uri.EscapeDataString(startLat.ToString());
            var endLongitude = Uri.EscapeDataString(endLon.ToString());
            var endLatitude = Uri.EscapeDataString(endLat.ToString());
            var requestUri = $"{_configuration["Mapbox:DirectionUrl"]}/driving/{startLongitude},{startLatitude};{endLongitude},{endLatitude}?access_token={_configuration["Mapbox:AccessToken"]}&geometries=geojson";
            try
            {
                var response = await _httpClient.GetAsync(requestUri);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var json = JObject.Parse(content);

                // Extract the distance from the response
                var distance = json["routes"]?[0]?["distance"];
                return distance != null ? Math.Ceiling((double)distance / 1000) : null;
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Error calling Mapbox Directions API: {ex.Message}", ex);
            }
        }

        public async Task<double?> GetDistanceBetweenAddressAndShop(string address)
        {
            try
            {
                var (shopLong, shopLat) = (_configuration.GetValue<double>("KitStemHub:Coordinates:Longitude"), _configuration.GetValue<double>("KitStemHub:Coordinates:Latitude"));
                var (addressLong, addressLat) = await GetCoordinatesAsync(address);
                if (addressLong == null || addressLat == null)
                {
                    return 0;
                }

                var distance = await GetDistanceAsync(shopLat, shopLong, (double)addressLat, (double)addressLong);
                if (distance == null)
                {
                    return 0;
                }

                return distance;
            }
            catch
            {
                return 0;
            }
        }
    }
}