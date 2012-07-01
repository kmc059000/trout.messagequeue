using System.Net.Mail;

namespace trout.messagequeue.queue.overrides
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