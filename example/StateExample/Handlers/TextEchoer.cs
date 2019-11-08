﻿using System.Threading;
using System.Threading.Tasks;
using IBWT.Framework.Abstractions;
using Telegram.Bot.Types;

namespace Quickstart.AspNetCore.Handlers
{
    public class Texthandler : IUpdateHandler
    {
        public async Task HandleAsync(IUpdateContext context, UpdateDelegate next, CancellationToken cancellationToken)
        {
            Message msg = context.Update.Message;

            await context.Bot.Client.SendTextMessageAsync(
                msg.Chat,
                "You said:\n" + msg.Text,
                cancellationToken: cancellationToken
            );

            await next(context, cancellationToken);
        }
    }
}