using System.Text.Json;
using WeatherAppUI.Utilities;

namespace WeatherAppUI.Services
{
    internal class LocationService
    {
        public static async Task<string> GetLocation()
        {

            // Get public IP address using ipinfo.io
            string url = "https://ipinfo.io/json";
            HttpResponseMessage response = await HttpClientProvider.Client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("⚠️ IPINFO API call wasnt successfull");
                return "ERROR WITH IPINFO API";
            }

            var body = await response.Content.ReadAsStringAsync();

            using JsonDocument doc = JsonDocument.Parse(body);
            JsonElement root = doc.RootElement;
            var ip = root.GetProperty("ip").GetString();

            //determine location on IPSTACK API, because ipinfo sometimes misplaces the location
            //string locationAPIKey = Environment.GetEnvironmentVariable("IPSTACK_API_KEY");
            string locationAPIKey = "19f34c0298230c5f4d5a7fd6273633c2";
            string locationUrl = "http://api.ipstack.com/" + ip + "?access_key=" + locationAPIKey;

            HttpResponseMessage locationResponse = await HttpClientProvider.Client.GetAsync(locationUrl);

            if (!locationResponse.IsSuccessStatusCode)
            {
                Console.WriteLine("⚠️ IPSTACK API call wasnt successfull");
                return "ERROR WITH IPSTACK API";
            }

            var locationBody = await locationResponse.Content.ReadAsStringAsync();

            using JsonDocument locationDoc = JsonDocument.Parse(locationBody);
            JsonElement locationRoot = locationDoc.RootElement;

            if (locationRoot.TryGetProperty("city", out JsonElement cityElement))
            {
                var city = cityElement.GetString();
                return city;
            }
            else
            {
                Console.WriteLine("❌ 'city' not found in IPStack API response.");
                return "ERROR: CITY NOT FOUND";
            }
        }
    }
}
