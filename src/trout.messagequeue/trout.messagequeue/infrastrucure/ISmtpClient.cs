using System.Net.Mail;

namespace trout.messagequeue.infrastrucure
{
    public interface ISmtpClient
    {
        SendResult Send(MailMessage mailMessage);
    }
}