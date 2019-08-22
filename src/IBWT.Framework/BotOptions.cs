using IBWT.Framework.Abstractions;

namespace IBWT.Framework
{
    /// <summary>
    /// Configurations for the bot
    /// </summary>
    public class BotOptions : IBotOptions
    {
        public string Username { get; set; }

        /// <summary>
        /// Optional if client not needed. Telegram API token
        /// </summary>
        public string ApiToken { get; set; }
        public string WebhookDomain { get; set; }
        public string WebhookPath { get; set; }

    }
}