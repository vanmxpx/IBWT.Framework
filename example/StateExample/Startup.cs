using System;
using IBWT.Framework;
using IBWT.Framework.Abstractions;
using IBWT.Framework.Extentions;
using IBWT.Framework.Middleware;
using IBWT.Framework.Scheduler;
using IBWT.Framework.State.Providers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quickstart.AspNetCore.BackgroundTasks;
using Quickstart.AspNetCore.Configuration;
using Quickstart.AspNetCore.Data.Entities;
using Quickstart.AspNetCore.Data.Repository;
using Quickstart.AspNetCore.Handlers;
using Quickstart.AspNetCore.Services;

namespace Quickstart.AspNetCore
{
    public class Startup
    {
        private readonly IHostingEnvironment env;

        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            this.env = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddConfigurationProvider(Configuration, env);

            services.AddScoped<IDataRepository<Order>, OrderRepository>();
            services.AddScoped<IDataRepository<TGUser>, TGUserReposiroty>();

            if (env.IsDevelopment())
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    // options.UseSqlServer(Configuration.GetConnectionString("LocalDatabase"))
                    options.UseInMemoryDatabase("Test")
                );
            }
            else
            {
                services.AddHttpsRedirection(options =>
                {
                    options.RedirectStatusCode = StatusCodes.Status308PermanentRedirect;
                    options.HttpsPort = 443;
                });

                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("RemoteDatabase")));
            }

            services.AddBotStateCache<InMemoryStateProvider>();
            services.AddTelegramBot()
                .AddScoped<Texthandler>()
                .AddScoped<StartCommand>()
                .AddScoped<UpdateLogger>()
                .AddScoped<StickerHandler>()
                .AddScoped<WeatherReporter>()
                .AddScoped<ExceptionHandler>()
                .AddScoped<UpdateMembersList>()
                .AddScoped<Callback1QueryHandler>()
                 .AddScoped<Callback2QueryHandler>()
                  .AddScoped<Callback3QueryHandler>();

            services.AddScoped<IWeatherService, WeatherService>();

            services.AddSingleton<IScheduledTask, NotifyWeatherTask>();
            services.AddScheduler((sender, args) =>
            {
                Console.Write(args.Exception.Message);
                args.SetObserved();
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseHttpsRedirection();
            // app.UseStaticFiles();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                // app.UseMvc();
                app.UseTelegramBotLongPolling(ConfigureBot(), startAfter: TimeSpan.FromSeconds(2));
            }
            else
            {
                // app.UseMvc();
                app.UseTelegramBotWebhook(ConfigureBot());
                app.EnsureWebhookSet();
            }

        }

        private IBotBuilder ConfigureBot()
        {
            return new BotBuilder()
                .Use<ExceptionHandler>()
                .Use<UpdateLogger>()
                .UseWhen(When.State("default"), cmdBranch => cmdBranch
                    .UseCommand<StartCommand>("start")
                )
                .UseWhen(When.State("test1"), defaultBranch => defaultBranch
                    .UseWhen<Callback1QueryHandler>(When.CallbackQuery)
                )
                .UseWhen(When.State("test2"), defaultBranch => defaultBranch
                    .UseWhen<Callback2QueryHandler>(When.CallbackQuery)
                )
                .UseWhen(When.State("test3"), defaultBranch => defaultBranch
                    .UseWhen<Callback3QueryHandler>(When.CallbackQuery)
                )
                // .Use<CustomUpdateLogger>()
                // .UseWhen<UpdateMembersList>(When.MembersChanged)
                // .UseWhen(When.NewMessage, msgBranch => msgBranch
                //     .UseWhen(When.NewTextMessage, txtBranch => txtBranch
                //         .Use<Texthandler>()
                //         .UseWhen(When.NewCommand, cmdBranch => cmdBranch
                //             .UseCommand<StartCommand>("start")
                //         )
                //     //.Use<NLP>()
                //     )
                //     .UseWhen<StickerHandler>(When.StickerMessage)
                //     .UseWhen<WeatherReporter>(When.LocationMessage)
                // )
                // .UseWhen<CallbackQueryHandler>(When.CallbackQuery)

            // .Use<UnhandledUpdateReporter>()
            ;
        }
    }
}