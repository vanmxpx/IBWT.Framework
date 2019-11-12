using System;
using IBWT.Framework.Abstractions;
using Telegram.Bot.Types;

namespace IBWT.Framework
{
    public static class UpdateExtention
    {
        public static long GetChatId(this Update update)
        {
            if(update.Message != null)
                return update.Message.Chat.Id;
            if(update.CallbackQuery != null)
                return update.CallbackQuery.Message.Chat.Id;

            throw new InvalidOperationException($"Cannot handle chat id from operation {update.Type}");
        }
    }
}