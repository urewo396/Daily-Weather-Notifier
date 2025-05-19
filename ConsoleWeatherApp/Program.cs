using Newtonsoft.Json;
using System.Text.Json;
using ConsoleWeatherApp.Services;

class Program
{


    static async Task Main(string[] args)
    {

        while (true)
        {
            DateTime now = DateTime.Now;
            DateTime nextMidnight = now.Date.AddDays(1);
            TimeSpan timeUntilMidnight = nextMidnight - now;

            await WeatherService.RunWeatherCheck();
            Console.WriteLine($"[INFO] Waiting until midnight... ({timeUntilMidnight.TotalMinutes:F1} min left)");
            await Task.Delay(timeUntilMidnight); // Wait until 00:00

            Console.WriteLine("[INFO] Done. Waiting for next day...\n");

            await Task.Delay(TimeSpan.FromDays(1)); //Wait another 24h for nexy weather call
        }
    }
}