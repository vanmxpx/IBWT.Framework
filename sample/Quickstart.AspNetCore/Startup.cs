using System;
using IBWT.Framework;
using IBWT.Framework.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quickstart.AspNetCore.Handlers;
using Quickstart.AspNetCore.Services;

namespace Quickstart.AspNetCore
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<EchoBot>()
                .Configure<BotOptions>(Configuration.GetSection("EchoBot"))
                .AddScoped<Texthandler>()
                .AddScoped<StartCommand>()
                .AddScoped<UpdateLogger>()
                .AddScoped<StickerHandler>()
                .AddScoped<WeatherReporter>()
                .AddScoped<ExceptionHandler>()
                .AddScoped<UpdateMembersList>()
                .AddScoped<CallbackQueryHandler>();
            services.AddScoped<IWeatherService, WeatherService>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                // get bot updates from Telegram via long-polling approach during development
                // this will disable Telegram webhooks
                app.UseTelegramBotLongPolling<EchoBot>(ConfigureBot(), startAfter : TimeSpan.FromSeconds(2));
            }
            else
            {
                // use Telegram bot webhook middleware in higher environments
                app.UseTelegramBotWebhook<EchoBot>(ConfigureBot());
                // and make sure webhook is enabled
                app.EnsureWebhookSet<EchoBot>();
            }

        }

        private IBotBuilder ConfigureBot()
        {
            return new BotBuilder()
                .Use<ExceptionHandler>()
                .Use<UpdateLogger>()

                // .Use<CustomUpdateLogger>()
                .UseWhen<UpdateMembersList>(When.MembersChanged)
                .UseWhen(When.NewMessage, msgBranch => msgBranch
                    .UseWhen(When.NewTextMessage, txtBranch => txtBranch
                        .Use<Texthandler>()
                        .UseWhen(When.NewCommand, cmdBranch => cmdBranch
                            .UseCommand<StartCommand>("start")
                        )
                        //.Use<NLP>()
                    )
                    .UseWhen<StickerHandler>(When.StickerMessage)
                    .UseWhen<WeatherReporter>(When.LocationMessage)
                )
                .UseWhen<CallbackQueryHandler>(When.CallbackQuery)

            // .Use<UnhandledUpdateReporter>()
            ;
        }
    }
}