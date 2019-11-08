using Quickstart.AspNetCore.Configuration.Entities;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Quickstart.AspNetCore.Services
{
    class WeatherService : IWeatherService
    {
        private readonly HttpClient _client;
        private readonly WeatherServiceConfig config;

        public WeatherService(IOptions<WeatherServiceConfig> options)
        {
            this.config = options.Value;
            _client = new HttpClient
            {
                BaseAddress = new Uri(config.BaseUrl)
            };
        }

        public async Task<CurrentWeather> GetWeatherAsync(float lat, float lon)
        {
            string location = await FindLocationIdAsync(lat, lon)
                .ConfigureAwait(false);

            DateTime today = DateTime.Today;

            string json = await _client.GetStringAsync(string.Format(config.GetWeatherUrl, location, today.Year, today.Month, today.Day))
                .ConfigureAwait(false);

            dynamic arr = JsonConvert.DeserializeObject(json);

            return new CurrentWeather
            {
                Status = arr[0].weather_state_name,
                Temp = arr[0].the_temp,
                MinTemp = arr[0].min_temp,
                MaxTemp = arr[0].max_temp,
            };
        }

        private async Task<string> FindLocationIdAsync(float lat, float lon)
        {
            string json = await _client.GetStringAsync(string.Format(config.GetLocationUrl, lat, lon))
                .ConfigureAwait(false);
            dynamic arr = JsonConvert.DeserializeObject(json);
            return arr[0].woeid;
        }
    }
}
