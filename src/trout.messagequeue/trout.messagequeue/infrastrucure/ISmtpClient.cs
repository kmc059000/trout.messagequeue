using System.Net.Mail;

namespace trout.emailservice.infrastrucure
{
    public interface ISmtpClient
    {
        SendResult Send(MailMessage mailMessage);
    }
}