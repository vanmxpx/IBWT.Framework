using System;
using System.Linq;
using IBWT.Framework.Abstractions;
using Microsoft.AspNetCore.Http;
using Telegram.Bot.Types.Enums;

namespace IBWT.Framework
{
    public static class When
    {
        public static bool Webhook(IUpdateContext context) => context.Items.ContainsKey(nameof(HttpContext));

        public static Predicate<IUpdateContext> State(string command) => 
                (IUpdateContext context) => context.Items.ContainsKey("State") && context.Items["State"].Equals(command);

        public static bool NewMessage(IUpdateContext context) =>
            context.Update.Message != null;

        public static bool NewTextMessage(IUpdateContext context) =>
            context.Update.Message?.Text != null;

        public static bool NewCommand(IUpdateContext context) =>
            context.Update.Message?.Entities?.FirstOrDefault()?.Type == MessageEntityType.BotCommand;

        public static bool MembersChanged(IUpdateContext context) =>
            context.Update.Message?.NewChatMembers != null
            || context.Update.Message?.LeftChatMember != null
            || context.Update.ChannelPost?.NewChatMembers != null
            || context.Update.ChannelPost?.LeftChatMember != null;

        public static bool LocationMessage(IUpdateContext context) =>
            context.Update.Message?.Location != null;

        public static bool StickerMessage(IUpdateContext context) =>
            context.Update.Message?.Sticker != null;

        public static bool CallbackQuery(IUpdateContext context) =>
            context.Update.CallbackQuery != null;
    }
}