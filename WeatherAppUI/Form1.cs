using WeatherAppUI.Services;
using DotNetEnv;

namespace WeatherAppUI
{
    public partial class WeatherApp : Form
    {
        private string city;
        private string weatherData;

        public WeatherApp()
        {
            InitializeComponent();
            Env.Load();
        }
        private async void button1_Click(object sender, EventArgs e)
        {
            city = textBox1.Text;
            if (string.IsNullOrWhiteSpace(city))
            {
                city = await LocationService.GetLocation();
            }

            weatherData = await WeatherService.RunWeatherCheck(city);
            richTextBox1.Text = weatherData;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            Env.Load();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
