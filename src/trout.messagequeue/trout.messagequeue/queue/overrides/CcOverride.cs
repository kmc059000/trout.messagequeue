using System.Net.Mail;

namespace trout.messagequeue.queue.overrides
{
    public class CcOverride : MailMessageOverride
    {
        public override MailMessage ApplyOverride(MailMessage message)
        {
            this.ApplyMailAddressOverride(message.CC);

            return message;
        }
    }
}