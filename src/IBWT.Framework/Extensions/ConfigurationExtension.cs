
using IBWT.Framework.Abstractions;
using IBWT.Framework.Services.State;
using IBWT.Framework.State.Providers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IBWT.Framework.Extentions
{
    public static class ConfigurationExtension
    {

        public static IServiceCollection AddTelegramBot(
            this IServiceCollection services
        )
        {
            return  services.AddTransient<TelegramBot>();
        }

        public static IServiceCollection AddTelegramBotOptions(
            this IServiceCollection services
        )
        {
            
            var sp = services.BuildServiceProvider();
            var config = sp.GetService<IConfiguration>();
            var env = sp.GetService<IHostEnvironment>();

            if (env.IsDevelopment())
            {
                services.Configure<BotOptions>(config.GetSection("BotOptionsTest"));
            }
            else 
            {
                services.Configure<BotOptions>(config.GetSection("BotOptions"));
            }
            
            return  services;
        }

        public static IServiceCollection AddBotStateCache<TStateCache>(
            this IServiceCollection services,
            IBotBuilder botBuilder
        )
        where TStateCache: IStateProvider, new() 
        => services.AddSingleton<IStateCacheService>(new StateCacheService<TStateCache>(botBuilder));
        
    }
}