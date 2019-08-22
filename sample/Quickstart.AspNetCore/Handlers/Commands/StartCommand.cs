using System.Threading;
using System.Threading.Tasks;
using IBWT.Framework.Abstractions;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Quickstart.AspNetCore.Handlers
{
    class StartCommand : CommandBase
    {
        public override async Task HandleAsync(
            IUpdateContext context,
            UpdateDelegate next,
            string[] args,
            CancellationToken cancellationToken
        )
        {
            var msg = context.Update.Message;
            await context.Bot.Client.SendTextMessageAsync(
                msg.Chat,
                "*Hello, World!*",
                ParseMode.Markdown,
                replyToMessageId : msg.MessageId,
                replyMarkup : new InlineKeyboardMarkup(
                    InlineKeyboardButton.WithCallbackData("Tap to start", "Start")
                ),
                cancellationToken : cancellationToken
            );

            await next(context, cancellationToken);
        }
    }
}