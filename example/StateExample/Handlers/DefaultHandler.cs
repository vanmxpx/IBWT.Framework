using System.Threading;
using System.Threading.Tasks;
using IBWT.Framework.Abstractions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Quickstart.AspNetCore.Handlers
{
    public class DefaultHandler : IUpdateHandler
    {
        public async Task HandleAsync(IUpdateContext context, UpdateDelegate next, CancellationToken cancellationToken)
        {
            Message msg = context.Update.Message ?? context.Update.CallbackQuery.Message;
            await context.Bot.Client.SendTextMessageAsync(
                msg.Chat,
                context.Items["History"].ToString() + " and last item = " +  context.Items["State"].ToString(),
                ParseMode.Markdown,
                replyToMessageId: msg.MessageId,
                replyMarkup: new InlineKeyboardMarkup(
                    new InlineKeyboardButton[]
                    {
                        InlineKeyboardButton.WithSwitchInlineQueryCurrentChat("switck inline", " "),
                        InlineKeyboardButton.WithCallbackData("menu1", "menu1::")
                    }
                    
                ),
                cancellationToken: cancellationToken
            );
        }
    }
}