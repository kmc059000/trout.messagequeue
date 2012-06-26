using System.Net.Mail;

namespace trout.emailservice.infrastrucure
{
    public class SmtpClient : ISmtpClient
    {
        readonly System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();

        public void Send(MailMessage message)
        {
            client.Send(message);
        }
    }
}