using Quickstart.AspNetCore.Configuration.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quickstart.AspNetCore.Configuration.Entities.Logging;
using Microsoft.AspNetCore.Hosting;
using IBWT.Framework;

namespace Quickstart.AspNetCore.Configuration
{
    public static class ConfigurationExtention
    {
        public static void AddConfigurationProvider(this IServiceCollection services, IConfiguration config, IHostingEnvironment env)
        {
            services.Configure<ConnectionStrings>(config.GetSection("ConnectionStrings"))
                .Configure<LoggingSettings>(config.GetSection("Logging"))
                .Configure<WeatherServiceConfig>(config.GetSection("WeatherServiceConfig"));


            if (env.IsDevelopment())
            {
                services.Configure<BotOptions>(config.GetSection("BotConfigTest"));
            }
            else 
            {
                services.Configure<BotOptions>(config.GetSection("BotConfig"));
            }
        }

        private static T GetConfiguration<T>(IConfiguration config, string Path) where T : class
        {
            return config.GetSection(Path).Get<T>();
        }

    }
}