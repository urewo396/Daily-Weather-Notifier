using System.Text.Json;
using ConsoleWeatherApp.Utilities;

namespace ConsoleWeatherApp.Services
{
    public class LocationService
    {
        public static async Task<string> GetLocation()
        {

            // Get public IP address using ipinfo.io
            string url = "https://ipinfo.io/json";
            HttpResponseMessage response = await HttpClientProvider.Client.GetAsync(url);
            
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("⚠️ API call wasnt successfull");
                return "ERROR WITH IPINFO API";
            }

            var body = await response.Content.ReadAsStringAsync();

            using JsonDocument doc = JsonDocument.Parse(body);
            JsonElement root = doc.RootElement;
            var ip = root.GetProperty("ip").GetString();

            //determine location on IPSTACK API, because ipinfo sometimes misplaces the location
            string locationAccessKey = "YOUR_ACCESS_KEY_FROM_IPSTACK";
            string locationUrl = "http://api.ipstack.com/" + ip + "?access_key=" + locationAccessKey;

            HttpResponseMessage locationResponse = await HttpClientProvider.Client.GetAsync(locationUrl);
            
            if (!locationResponse.IsSuccessStatusCode)
            {
                Console.WriteLine("⚠️ API call wasnt successfull");
                return "ERROR WITH IPSTACK API";
            }

            var locationBody = await locationResponse.Content.ReadAsStringAsync();

            using JsonDocument locationDoc = JsonDocument.Parse(locationBody);
            JsonElement locationRoot = locationDoc.RootElement;
            var city = locationRoot.GetProperty("city").GetString();

            return city;
        }
    }
}
