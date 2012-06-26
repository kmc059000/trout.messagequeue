using System.Net.Mail;

namespace trout.emailservice
{
    public class SmtpClient : ISmtpClient
    {
        System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();

        public void Send(MailMessage message)
        {
            client.Send(message);
        }
    }
}