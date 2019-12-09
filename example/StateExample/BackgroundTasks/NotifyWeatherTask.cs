using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using IBWT.Framework;
using IBWT.Framework.Scheduler;
using Quickstart.AspNetCore.Data.Entities;
using Quickstart.AspNetCore.Data.Repository;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Quickstart.AspNetCore.BackgroundTasks
{
    public class NotifyWeatherTask : IScheduledTask
    {
        private readonly IServiceProvider services;

        public string Schedule => "0 8,12,15,19 * * *";

        public NotifyWeatherTask(
            ILogger<NotifyWeatherTask> logger,
            TelegramBot tgBot,
            IServiceProvider services
        )
        {
            this.services = services;
        }
        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            using(var scope = services.CreateScope())
            {
                IDataRepository<TGUser> tgUserRepository = (IDataRepository<TGUser>) scope.ServiceProvider.GetService(typeof(IDataRepository<TGUser>));

                List<TGUser> users = tgUserRepository.All().ToList();

            }
        }
    }
}