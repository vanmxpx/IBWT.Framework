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
                    //options.UseSqlServer(Configuration.GetConnectionString("LocalDatabase"))
                    options.UseInMemoryDatabase("TestDB")
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
            // Save history of telegram user movements throw the bots' menus
            services.AddBotStateCache<InMemoryStateProvider>(ConfigureBot());

            services.AddTelegramBot()
                .AddScoped<TextHandler>()
                .AddScoped<StartCommand>()
                .AddScoped<UpdateLogger>()
                .AddScoped<CustomUpdateLogger>()
                .AddScoped<StickerHandler>()
                .AddScoped<WeatherReporter>()
                .AddScoped<ExceptionHandler>()
                .AddScoped<UpdateMembersList>()
                .AddScoped<DefaultHandler>()
                .AddScoped<Menu1QueryHandler>()
                .AddScoped<Menu2QueryHandler>()
                .AddScoped<Menu3QueryHandler>()
                .AddScoped<PaginationHandler>();

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
                .Use<CustomUpdateLogger>()
                .UseWhen<UpdateMembersList>(When.MembersChanged)
                .MapWhen(When.State("default"), cmdBranch => cmdBranch
                    .UseWhen(When.NewMessage, msgBranch => msgBranch
                        .UseWhen(When.NewTextMessage, txtBranch => txtBranch
                            .UseWhen(When.NewCommand, cmdBranch => cmdBranch
                                .UseCommand<StartCommand>("start")
                            )
                            //.Use<NLP>()
                        )
                        .MapWhen<StickerHandler>(When.StickerMessage)
                        .MapWhen<WeatherReporter>(When.LocationMessage)
                    )
                    .Use<DefaultHandler>()
                )
                .MapWhen(When.State("menu1"), defaultBranch => defaultBranch
                    .MapWhen<Menu1QueryHandler>(When.CallbackQuery)
                )
                .MapWhen(When.State("menu2"), defaultBranch => defaultBranch
                    .Use<Menu2QueryHandler>()
                )
                .MapWhen(When.State("menu3"), defaultBranch => defaultBranch
                    .MapWhen<Menu3QueryHandler>(When.CallbackQuery)
                    .UseWhen(When.NewMessage, msgBranch => msgBranch
                        .UseWhen(When.NewTextMessage, txtBranch => txtBranch
                            .Use<TextHandler>()
                            //.Use<NLP>()
                        )
                        .MapWhen<StickerHandler>(When.StickerMessage)
                        .MapWhen<WeatherReporter>(When.LocationMessage)
                    )   
                )
                .MapWhen(When.State("pagination"), defaultBranch => defaultBranch
                    .MapWhen<PaginationHandler>(When.CallbackQuery)
                )
            // .Use<UnhandledUpdateReporter>()
            ;
        }
    }
}