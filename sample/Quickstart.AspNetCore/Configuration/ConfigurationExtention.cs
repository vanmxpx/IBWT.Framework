using IBWTWeather.Configuration.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quickstart.AspNetCore.Configuration.Entities;
using Quickstart.AspNetCore.Configuration.Entities.Logging;

namespace Quickstart.AspNetCore.Configuration
{
    public static class ConfigurationExtention
    {
        public static void AddConfigurationProvider(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<ConnectionStrings>(config.GetSection("ConnectionStrings"))
                .Configure<LoggingSettings>(config.GetSection("Logging"))
                .Configure<BotConfig>(config.GetSection("BotConfig"))
                .Configure<ValeoApiConfig>(config.GetSection("WeatherApi"));

        }

        private static T GetConfiguration<T>(IConfiguration config, string Path) where T : class
        {
            return config.GetSection(Path).Get<T>();
        }

    }
}