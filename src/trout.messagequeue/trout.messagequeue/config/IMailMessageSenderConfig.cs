using System.Net.Mail;

namespace trout.emailservice.config
{
    public interface IMailMessageSenderConfig
    {
        int MaxTries { get; }
        MailAddress FromAddress { get; }
    }
}