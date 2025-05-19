using ConsoleWeatherApp.Utilities;

namespace ConsoleWeatherApp.Services
{
    internal class SendTelegramBotMessage
    {
        public async Task SendTelegramMessage(string msg)
        {

            string botToken = Environment.GetEnvironmentVariable("BOT_TOKEN");
            string chatId = Environment.GetEnvironmentVariable("CHAT_ID");
            string url = $"https://api.telegram.org/bot{botToken}/sendMessage?chat_id={chatId}&text={msg}";

            HttpResponseMessage response = await HttpClientProvider.Client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Telegram message sent successfully!");
            }
            else
            {
                Console.WriteLine("Failed to send a message. Check your telegram bot credentials");
            }
        }
    }
}
