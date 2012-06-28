using System.Net.Mail;

namespace trout.emailservice.queue.overrides
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