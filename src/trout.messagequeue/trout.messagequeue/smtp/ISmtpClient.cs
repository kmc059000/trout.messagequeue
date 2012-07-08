using System.Net.Mail;

namespace trout.messagequeue.smtp
{
    public interface ISmtpClient
    {
        SendResult Send(MailMessage mailMessage);
    }
}