using System.Net.Mail;

namespace trout.messagequeue.queue.overrides
{
    public class ToOverride : MailMessageOverride
    {
        public override MailMessage ApplyOverride(MailMessage message)
        {
            this.ApplyMailAddressOverride(message.To);

            return message;
        }
    }
}