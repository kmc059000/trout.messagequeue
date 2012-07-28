using System.Net.Mail;

namespace trout.messagequeue.config
{
    /// <summary>
    /// Configuration values for Trout
    /// </summary>
    public interface IMailMessageSenderConfig
    {
        /// <summary>
        /// Number of times an email will be attempted to be sent
        /// </summary>
        int MaxTries { get; }

        /// <summary>
        /// Address the emails should be sent from
        /// </summary>
        MailAddress FromAddress { get; }

        
    }
}