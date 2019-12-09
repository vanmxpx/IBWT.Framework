using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using IBWT.Framework.Abstractions;
using IBWT.Framework.Services.State;
using Telegram.Bot.Types;

namespace Quickstart.AspNetCore.Handlers
{
    public class TextHandler : IUpdateHandler
    {
        private readonly IStateCacheService stateCache;

        public TextHandler(
            IStateCacheService stateCache
        ) 
        {
            this.stateCache = stateCache;
        }
        public async Task HandleAsync(IUpdateContext context, UpdateDelegate next, CancellationToken cancellationToken)
        {
            Message msg = context.Update.Message;

            await context.Bot.Client.SendTextMessageAsync(
                msg.Chat,
                "You said:\n" + msg.Text,
                cancellationToken: cancellationToken
            );
            await stateCache.UpdateState(context, "back");
            // await next(context, cancellationToken);
        }
    }
}