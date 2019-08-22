﻿using System.Threading;
using System.Threading.Tasks;
using IBWT.Framework.Abstractions;
using Telegram.Bot.Types;

namespace Quickstart.AspNetCore.Handlers
{
    public class CallbackQueryHandler : IUpdateHandler
    {
        public async Task HandleAsync(IUpdateContext context, UpdateDelegate next, CancellationToken cancellationToken)
        {
            CallbackQuery cq = context.Update.CallbackQuery;

            await context.Bot.Client.AnswerCallbackQueryAsync(
                cq.Id,
                "PONG",
                showAlert: true,
                cancellationToken: cancellationToken
            );

            await next(context, cancellationToken);
        }
    }
}