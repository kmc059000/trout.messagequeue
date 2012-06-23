using System.Net.Mail;

namespace trout.emailservice
{
    public interface IMailMessageSenderConfig
    {
        int MaxTries { get; }
        MailAddress FromAddress { get; }
    }
}