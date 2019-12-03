using System;
using System.Threading;
using System.Threading.Tasks;
using IBWT.Framework.Abstractions;
using IBWT.Framework.Pagination;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Quickstart.AspNetCore.Handlers
{
    public class PaginationHandler : IUpdateHandler
    {
        private string[] data;
        public PaginationHandler()
        {
            data = new string[100];
            for(int i = 0; i < 100; i++)
            {
                data[i] = $"test data {i + 1}";
            }
        }
        public async Task HandleAsync(IUpdateContext context, UpdateDelegate next, CancellationToken cancellationToken)
        {
            CallbackQuery cq = context.Update.CallbackQuery;

            string page = context.Items["Data"].ToString();

            PaginatorData pd = new PaginatorBuilder<string>(5, 3, "pagination").Build(data, Int32.Parse(page));
            await context.Bot.Client.EditMessageTextAsync(
                cq.Message.Chat.Id,
                cq.Message.MessageId,
                pd.Message,
                replyMarkup: pd.ReplyMarkup,
                cancellationToken: cancellationToken
            );
        }




    }
}