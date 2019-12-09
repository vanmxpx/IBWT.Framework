namespace IBWT.Framework.Abstractions
{
    /// <summary>
    /// Configurations for the bot
    /// </summary>
    public interface IBotOptions
    {
        string Username { get; }

        /// <summary>
        /// Optional if client not needed. Telegram API token
        /// </summary>
        string ApiToken { get; }
        string WebhookDomain { get; set; }

        string WebhookPath { get; }
        
   
    }
}