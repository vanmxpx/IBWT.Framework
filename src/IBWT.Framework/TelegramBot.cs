using Microsoft.Extensions.Options;
using IBWT.Framework;

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
