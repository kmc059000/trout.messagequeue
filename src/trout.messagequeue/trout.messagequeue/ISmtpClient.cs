using System.Net.Mail;

namespace trout.emailservice
{
    public interface ISmtpClient
    {
        void Send(MailMessage message);
    }
}