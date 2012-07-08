using System.Net.Mail;

namespace trout.messagequeue.queue.overrides
{
    public sealed class ToOverride : MailMessageOverride
    {
        public override MailMessage ApplyOverride(MailMessage message)
        {
            this.ApplyMailAddressOverride(message.To);

            return message;
        }
    }
}