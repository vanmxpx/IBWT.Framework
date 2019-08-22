using IBWT.Framework.Abstractions;

namespace Quickstart.AspNetCore.Configuration.Entities
{
    public class BotConfig : IBotOptions
    {
        public string Username { get; set; }
        public string ApiToken { get; set; }
        public string WebhookDomain { get; set; }
        public string WebhookPath { get; set; }
    }
}