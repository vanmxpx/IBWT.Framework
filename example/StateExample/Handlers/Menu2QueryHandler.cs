using System.Threading;
using System.Threading.Tasks;
using IBWT.Framework.Abstractions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Quickstart.AspNetCore.Handlers
{
    public class Menu2QueryHandler : IUpdateHandler
    {
        public async Task HandleAsync(IUpdateContext context, UpdateDelegate next, CancellationToken cancellationToken)
        {
            Message mess; 
            if(context.Update.CallbackQuery != null)
            {
                mess = context.Update.CallbackQuery.Message;
            } 
            else
            {
                mess = context.Update.Message;
            }

            await context.Bot.Client.SendTextMessageAsync(
                mess.Chat,
                context.Items["History"].ToString() + " and last item = " +  context.Items["State"].ToString(),
                replyMarkup: new InlineKeyboardMarkup(
                    new InlineKeyboardButton[]
                    {
                        InlineKeyboardButton.WithCallbackData("menu3", "menu3::"),
                        InlineKeyboardButton.WithCallbackData("back", "back::")
                    }
                    
                ),
                cancellationToken: cancellationToken
            );
        }
    }
}