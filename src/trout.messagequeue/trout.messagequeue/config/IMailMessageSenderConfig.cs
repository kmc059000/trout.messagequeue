using System.Net.Mail;

namespace trout.messagequeue.config
{
    public interface IMailMessageSenderConfig
    {
        int MaxTries { get; }
        MailAddress FromAddress { get; }
    }
}