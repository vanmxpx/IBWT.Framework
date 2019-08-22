using System;
using IBWT.Framework;
using IBWT.Framework.Abstractions;
using IBWT.Framework.Middleware;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Microsoft.AspNetCore.Builder
{
    public static class WebhookMiddleware
    {
        /// <summary>
        /// Add Telegram bot webhook handling functionality to the pipeline
        /// </summary>
        /// <typeparam name="TBot">Type of bot</typeparam>
        /// <param name="app">Instance of IApplicationBuilder</param>
        /// <param name="botBuilder">Bot builder, implemented by framework user</param>
        /// <returns>Instance of IApplicationBuilder</returns>
        public static IApplicationBuilder UseTelegramBotWebhook<TBot>(
            this IApplicationBuilder app,
            IBotBuilder botBuilder
        )
        where TBot : BotBase
        {
            var updateDelegate = botBuilder.Build();

            var options = app.ApplicationServices.GetRequiredService<IOptions<BotOptions>>();
            app.Map(
                options.Value.WebhookPath,
                builder => builder.UseMiddleware<TelegramBotMiddleware<TBot>>(updateDelegate)
            );

            return app;
        }

        public static IApplicationBuilder EnsureWebhookSet<TBot>(this IApplicationBuilder app) where TBot : IBot
        {
            using(var scope = app.ApplicationServices.CreateScope())
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<IApplicationBuilder>>();
                var bot = scope.ServiceProvider.GetRequiredService<TBot>();
                var options = scope.ServiceProvider.GetRequiredService<IOptions<BotOptions>>();
                var url = new Uri(new Uri(options.Value.WebhookDomain), options.Value.WebhookPath);

                logger.LogInformation("Setting webhook for bot \"{0}\" to URL \"{1}\"", typeof(TBot).Name, url);

                bot.Client.SetWebhookAsync(url.AbsoluteUri)
                    .GetAwaiter().GetResult();
            }

            return app;
        }
    }
}