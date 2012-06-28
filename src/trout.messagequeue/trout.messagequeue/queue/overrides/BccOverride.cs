using System.Net.Mail;

namespace trout.emailservice.queue.overrides
{
    public class BccOverride : MailMessageOverride
    {
        public override MailMessage ApplyOverride(MailMessage message)
        {
            this.ApplyMailAddressOverride(message.Bcc);

            return message;
        }
    }
}