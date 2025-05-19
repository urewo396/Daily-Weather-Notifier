using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherAppUI.Utilities
{
    internal class HttpClientProvider
    {
        public static readonly HttpClient Client = new HttpClient();
    }
}
