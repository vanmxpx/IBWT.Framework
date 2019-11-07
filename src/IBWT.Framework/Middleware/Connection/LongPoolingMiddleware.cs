using System;
using System.Threading;
using System.Threading.Tasks;
using IBWT.Framework;
using IBWT.Framework.Abstractions;
using IBWT.Framework.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// Extenstion methods for adding Telegram Bot framework to the ASP.NET Core middleware
    /// </summary>
    public static class LongPoolingMiddleware
    {
        /// <summary>
        /// Removes and disables webhooks for bot. Using default Telegram Bot Instance
        /// </summary>
        /// <param name="app">Instance of IApplicationBuilder</param>
        /// <param name="botBuilder">Bot builder, implemented by framework user</param>
        /// <param name="startAfter">Timeout for starting update manager</param>
        /// <param name="cancellationToken">Standart threading cancellation token to stop process</param>
        /// <returns>Instance of IApplicationBuilder</returns>
        public static IApplicationBuilder UseTelegramBotLongPolling(this IApplicationBuilder app,
            IBotBuilder botBuilder,
            TimeSpan startAfter = default,
            CancellationToken cancellationToken = default)
        {
            return UseTelegramBotLongPolling<TelegramBot>(app, botBuilder, startAfter, cancellationToken);
        }

        /// <summary>
        /// Removes and disables webhooks for bot
        /// </summary>
        /// <typeparam name="TBot">Type of bot</typeparam>
        /// <param name="app">Instance of IApplicationBuilder</param>
        /// <param name="botBuilder">Bot builder, implemented by framework user</param>
        /// <param name="startAfter">Timeout for starting update manager</param>
        /// <param name="cancellationToken">Standart threading cancellation token to stop process</param>
        /// <returns>Instance of IApplicationBuilder</returns>
        public static IApplicationBuilder UseTelegramBotLongPolling<TBot>(this IApplicationBuilder app,
            IBotBuilder botBuilder,
            TimeSpan startAfter = default,
            CancellationToken cancellationToken = default) where TBot : IBot
        {
            if (startAfter == default)
            {
                startAfter = TimeSpan.FromSeconds(2);
            }

            using (var scope = app.ApplicationServices.CreateScope())
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<IApplicationBuilder>>();
                var updateManager = new UpdatePollingManager<TBot>(botBuilder, new BotServiceProvider(app));

                Task.Run(async () =>
                    {
                        await Task.Delay(startAfter, cancellationToken);
                        await updateManager.RunAsync(cancellationToken: cancellationToken);
                    }, cancellationToken)
                    .ContinueWith(t =>
                    {
                        logger.LogError(t.Exception, "Error on updating the bot by LongPooling method.");
                        throw t.Exception;
                    }, TaskContinuationOptions.OnlyOnFaulted);
            }
            return app;
        }

        //private static IBotManager<TBot> FindBotManager<TBot>(IApplicationBuilder app)
        //    where TBot : BotBase<TBot>
        //{
        //    IBotManager<TBot> botManager;
        //    try
        //    {
        //        botManager = app.ApplicationServices.GetRequiredService<IBotManager<TBot>>();
        //        if (botManager == null)
        //        {
        //            throw new NullReferenceException();
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw new ConfigurationException(
        //            "Bot Manager service is not available", string.Format("Use services.{0}<{1}>()",
        //                nameof(TelegramBotFrameworkIServiceCollectionExtensions.AddTelegramBot), typeof(TBot).Name));
        //    }
        //    return botManager;
        //}
    }
}