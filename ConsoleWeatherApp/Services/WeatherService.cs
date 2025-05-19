using Newtonsoft.Json;
using System.Text;
namespace ConsoleWeatherApp.Services
{
    public static class WeatherService
    {
        public static async Task RunWeatherCheck()
        {
            StringBuilder telegramMessage = new StringBuilder();

            string apiKey = Environment.GetEnvironmentVariable("WEATHER_API_KEY");
            string city = await LocationService.GetLocation();
            string url = $"https://weather.visualcrossing.com/VisualCrossingWebServices/rest/services/timeline/" + city + "?key=" + apiKey + "&lang=en&unitGroup=metric";

            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(url);

            // Small error handling
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("⚠️ Weather API call wasnt successfull");
                return;
            }

            var body = await response.Content.ReadAsStringAsync();

            dynamic weather = JsonConvert.DeserializeObject(body);

            // Get weather TMAX, TMIN, and description
            string weather_desc = weather.days[0].description;
            string weatherMax = weather.days[0].tempmax;
            string weatherMin = weather.days[0].tempmin;
            float weatherTmax = float.Parse(weatherMax, System.Globalization.CultureInfo.InvariantCulture);
            float weatherTmin = float.Parse(weatherMin, System.Globalization.CultureInfo.InvariantCulture);
            
            Console.WriteLine("Forecast for today: ");
            Console.WriteLine("Max Temperature is going to be: " + weatherTmax + "°C");
            Console.WriteLine("Min Temperature is going to be: " + weatherTmin + "°C");
            Console.WriteLine("Weather overall description: " + weather_desc + "\n");
            telegramMessage.AppendLine($"Forecast for today in {city}: ");
            telegramMessage.AppendLine($"Max Temperature: {weatherTmax}°C");
            telegramMessage.AppendLine($"Min Temperature: {weatherTmin}°C");
            telegramMessage.AppendLine($"Weather overall description: {weather_desc}");

            if (weatherTmax < -5)
            {
                Console.WriteLine("It's freezing AF 🥶. Wear a winter coat, scarf, gloves — the whole armor set.");
                telegramMessage.AppendLine("It's freezing AF 🥶. Wear a winter coat, scarf, gloves — the whole armor set.");
            }
            else if (weatherTmax >= -5 && weatherTmax < 5)
            {
                Console.WriteLine("It's hella cold ❄️. Don’t even think about going out without a jacket and hat.");
                telegramMessage.AppendLine("It's hella cold ❄️. Don’t even think about going out without a jacket and hat.");
            }
            else if (weatherTmax >= 5 && weatherTmax < 12)
            {
                Console.WriteLine("Kinda chilly 🧥. A jacket should do the trick.");
                telegramMessage.AppendLine("Kinda chilly 🧥. A jacket should do the trick.");
            }
            else if (weatherTmax >= 12 && weatherTmax < 18)
            {
                Console.WriteLine("Mild weather, kinda vibey 🌤️. Maybe layer up just in case.");
                telegramMessage.AppendLine("Mild weather, kinda vibey 🌤️. Maybe layer up just in case.");
            }
            else if (weatherTmax >= 18 && weatherTmax < 24)
            {
                Console.WriteLine("Pretty comfy weather 😌. T-shirt weather but maybe bring a hoodie.");
                telegramMessage.AppendLine("Pretty comfy weather 😌. T-shirt weather but maybe bring a hoodie.");
            }
            else if (weatherTmax >= 24 && weatherTmax < 30)
            {
                Console.WriteLine("It's getting hot 🔥. Stay hydrated, grab some shades and enjoy.");
                telegramMessage.AppendLine("It's getting hot 🔥. Stay hydrated, grab some shades and enjoy.");
            }
            else if (weatherTmax >= 30 && weatherTmax < 35)
            {
                Console.WriteLine("Hot as hell 🥵. Consider staying inside unless you wanna melt.");
                telegramMessage.AppendLine("Hot as hell. Consider staying inside unless you wanna melt.");
            }
            else if (weatherTmax >= 35)
            {
                Console.WriteLine("It's straight up desert levels 🌞💀. Don’t go outside unless you wanna become beef jerky.");
                telegramMessage.AppendLine("It's straight up desert levels 🌞💀. Don’t go outside unless you wanna become beef jerky.");
            }

            // Send the message to Telegram
            string msg = telegramMessage.ToString();
            SendTelegramBotMessage telegramBot = new SendTelegramBotMessage();
            await telegramBot.SendTelegramMessage(msg);
        }
    }
}
