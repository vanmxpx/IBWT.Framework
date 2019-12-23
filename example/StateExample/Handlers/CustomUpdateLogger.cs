using System.IO;
using System.Threading;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using IBWT.Framework.Abstractions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;

namespace Quickstart.AspNetCore.Handlers
{
    class CustomUpdateLogger : IUpdateHandler
    {
        private readonly ILogger<CustomUpdateLogger> _logger;

        public CustomUpdateLogger(
            ILogger<CustomUpdateLogger> logger
        )
        {
            _logger = logger;
        }

        public async Task HandleAsync(IUpdateContext context, UpdateDelegate next, CancellationToken cancellationToken)
        {
            InlineQuery iq = context.Update.InlineQuery;
            if(iq == null)
                { next(context, cancellationToken); return; }

                

            await Task.Delay(500);  
            

            InlineQueryResultBase[] results = {
                new InlineQueryResultLocation(
                    id: "1",
                    latitude: 40.7058316f,
                    longitude: -74.2581888f,
                    title: "New York")   // displayed result
                    {
                        InputMessageContent = new InputLocationMessageContent(
                            latitude: 40.7058316f,
                            longitude: -74.2581888f)    // message if result is selected
                    },
                new InlineQueryResultLocation(
                    id: "2",
                    latitude: 13.1449577f,
                    longitude: 52.507629f,
                    title: "Berlin") // displayed result
                    {
                        InputMessageContent = new InputLocationMessageContent(
                            latitude: 13.1449577f,
                            longitude: 52.507629f)   // message if result is selected
                    }
            };

            await context.Bot.Client.AnswerInlineQueryAsync(
                context.Update.InlineQuery.Id,
                results,
                isPersonal: true,
                cacheTime: 0);

            return;
        }
    }
}