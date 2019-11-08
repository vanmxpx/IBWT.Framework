using System.Threading;
using System.Threading.Tasks;
using IBWT.Framework.Abstractions;
using Microsoft.Extensions.Logging;
using Quickstart.AspNetCore.Data.Entities;
using Quickstart.AspNetCore.Data.Repository;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Quickstart.AspNetCore.Handlers
{
    public class StartCommand : CommandBase
    {
        private readonly IDataRepository<TGUser> userRepository;
        private readonly ILogger<StartCommand> logger;

        public StartCommand(
            IDataRepository<TGUser> userRepository,
            ILogger<StartCommand> logger
        )
        {
            this.userRepository = userRepository;
            this.logger = logger;
        }
        public override async Task HandleAsync(
            IUpdateContext context,
            UpdateDelegate next,
            string[] args,
            CancellationToken cancellationToken
        )
        {
            var msg = context.Update.Message;
            if (userRepository.Get(msg.Chat.Id) == null)
            {
                logger.LogInformation($"User created {0}, {1}", msg.Chat.Id, msg.Chat.Username);
                userRepository.Add(new TGUser()
                {
                    Id = msg.Chat.Id,
                    FirstName = msg.Chat.FirstName,
                    LastName = msg.Chat.LastName,
                    Nickname = msg.Chat.Username
                });
            }

            await context.Bot.Client.SendTextMessageAsync(
                msg.Chat,
                context.Items["History"].ToString() + " and last item = " +  context.Items["State"].ToString(),
                ParseMode.Markdown,
                replyToMessageId: msg.MessageId,
                replyMarkup: new InlineKeyboardMarkup(
                    new InlineKeyboardButton[]
                    {
                        InlineKeyboardButton.WithCallbackData("test1", "test1::")
                    }
                    
                ),
                cancellationToken: cancellationToken
            );

            await next(context, cancellationToken);
        }
    }
}