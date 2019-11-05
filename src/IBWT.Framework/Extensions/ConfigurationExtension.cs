
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IBWT.Framework.Extentions
{
    public static class ConfigurationExtension
    {
        public static IServiceCollection AddTelegramBot(this IServiceCollection services)
        {
            var sp = services.BuildServiceProvider();
            var config = sp.GetService<IConfiguration>();
            var env = sp.GetService<IHostingEnvironment>();
            
            if (env.IsDevelopment())
            {
                services.Configure<BotOptions>(config.GetSection("BotOptionsTest"));
            }
            else 
            {
                services.Configure<BotOptions>(config.GetSection("BotOptions"));
            }

            return services.AddTransient<TelegramBot>();
        }
    }
}