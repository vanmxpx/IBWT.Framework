using Microsoft.Extensions.Options;

namespace IBWT.Framework
{
    public class TelegramBot : BotBase
    {
        public TelegramBot(IOptions<BotOptions> options)
            : base(options.Value)
        {
        }
    }
}
