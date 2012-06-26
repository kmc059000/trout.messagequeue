using System.Net.Mail;

namespace trout.emailservice.infrastrucure
{
    public interface ISmtpClient
    {
        void Send(MailMessage message);
    }
}