using Newtonsoft.Json;
using System.Text.Json;


class Program
{


    static async Task Main(string[] args)
    {
        await RunWeatherCheck();

        while (true)
        {
            DateTime now = DateTime.Now;
            DateTime nextMidnight = now.Date.AddDays(1);

            TimeSpan timeUntilMidnight = nextMidnight - now;

            Console.WriteLine($"[INFO] Waiting until midnight... ({timeUntilMidnight.TotalMinutes:F1} min left)");
            await Task.Delay(timeUntilMidnight); // Wait until 00:00

            await RunWeatherCheck();

            Console.WriteLine("[INFO] Done. Waiting for next day...\n");

            await Task.Delay(TimeSpan.FromDays(1)); //Wait another 24h for nexy weather call
        }
    }

    static async Task RunWeatherCheck()
    {
        string apiKey = "YOUR_API_KEY";
        string city = await GetLocation();
        string url = $"https://weather.visualcrossing.com/VisualCrossingWebServices/rest/services/timeline/" + city + "?key=" + apiKey + "&lang=en&unitGroup=metric";

        HttpClient client = new HttpClient();
        HttpResponseMessage response = await client.GetAsync(url);

        response.EnsureSuccessStatusCode();

        var body = await response.Content.ReadAsStringAsync();

        dynamic weather = JsonConvert.DeserializeObject(body);

        // Get weather TMAX, TMIN, and description
        string weatherDate = weather.days[0].datetime;
        string weather_desc = weather.days[0].description;
        string weatherMax = weather.days[0].tempmax;
        string weatherMin = weather.days[0].tempmin;
        float weatherTmax = float.Parse(weatherMax, System.Globalization.CultureInfo.InvariantCulture);
        float weatherTmin = float.Parse(weatherMin, System.Globalization.CultureInfo.InvariantCulture);

        Console.WriteLine("Forecast for today: ");
        Console.WriteLine("Max Temperature is going to be: " + weatherTmax + "°C");
        Console.WriteLine("Min Temperature is going to be: " + weatherTmin + "°C");
        Console.WriteLine("Weather overall description: " + weather_desc + "\n");

        if (weatherTmax < 12 && weatherTmax > 5)
        {
            Console.WriteLine("Consider wearing a jacket, its not gonna be warm today.");
        }
        if (weatherTmax > 25)
        {
            Console.WriteLine("Consider grabbing some water, its gonna be hot today!");
        }
        if (weatherTmax < 5)
        {
            Console.WriteLine("Consider wearing a coat, it is going to be very cold today. Don't forget to wear gloves and a hat!");
        }
    }

    static async Task<string> GetLocation()
    {
        // Get public IP address using ipinfo.io
        HttpClient client = new HttpClient();
        string url = "https://ipinfo.io/json";
        HttpResponseMessage response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();

        using JsonDocument doc = JsonDocument.Parse(body);
        JsonElement root = doc.RootElement;
        var ip = root.GetProperty("ip").GetString();

        //determine location on IPSTACK API, because ipinfo sometimes misplaces the location
        string locationAccessKey = "YOUR_ACCESS_KEY_FROM_IPSTACK";
        string locationUrl = "http://api.ipstack.com/" + ip + "?access_key=" + locationAccessKey;
        HttpClient locationClient = new HttpClient();
        HttpResponseMessage locationResponse = await locationClient.GetAsync(locationUrl);
        locationResponse.EnsureSuccessStatusCode();
        var locationBody = await locationResponse.Content.ReadAsStringAsync();
        using JsonDocument locationDoc = JsonDocument.Parse(locationBody);
        JsonElement locationRoot = locationDoc.RootElement;
        var city = locationRoot.GetProperty("city").GetString();

        return city;
    }
}