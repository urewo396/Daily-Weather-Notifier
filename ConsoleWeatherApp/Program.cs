using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PushSharp.Apple;
using PushSharp.Core;
using System.Net.Http.Headers;
using System.Xml.Xsl;



class Program
{

    static async Task Main(string[] args)
    {


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
        string city = "YOUR_CITY";
        string url = $"https://weather.visualcrossing.com/VisualCrossingWebServices/rest/services/timeline/" + city + "?key=" + apiKey + "&lang=en&unitGroup=metric";

        HttpClient client = new HttpClient();
        HttpResponseMessage response = await client.GetAsync(url);

        response.EnsureSuccessStatusCode();

        var body = await response.Content.ReadAsStringAsync();

        dynamic weather = JsonConvert.DeserializeObject(body);

        string weatherDate = weather.days[0].datetime;
        string weather_desc = weather.days[0].description;
        string weatherMax = weather.days[0].tempmax;
        string weatherMin = weather.days[0].tempmin;
        float weatherTmax = float.Parse(weatherMax, System.Globalization.CultureInfo.InvariantCulture);
        float weatherTmin = float.Parse(weatherMin, System.Globalization.CultureInfo.InvariantCulture);

        Console.WriteLine("Forecast for today: ");
        Console.WriteLine("Max Temperature is going to be: " + weatherTmax + "°C");
        Console.WriteLine("Min Temperature is going to be: " + weatherTmin + "°C");

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
}