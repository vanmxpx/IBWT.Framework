using Telegram.Bot.Types.ReplyMarkups;

namespace IBWT.Framework.Pagination
{
    public class PaginatorData
    {    
        public string Message { get; set; }
        public InlineKeyboardMarkup ReplyMarkup { get; set; }
    }
}