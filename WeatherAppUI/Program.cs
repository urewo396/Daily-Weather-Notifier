using DotNetEnv;

namespace WeatherAppUI
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Env.Load();
            ApplicationConfiguration.Initialize();
            Application.Run(new WeatherApp());
        }
    }
}