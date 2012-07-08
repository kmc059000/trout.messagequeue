using System.Net.Mail;

namespace trout.messagequeue.queue.overrides
{
    public sealed class SubjectOverride : MailMessageOverride
    {
        public override MailMessage ApplyOverride(MailMessage message)
        {
            message.Subject = ApplyStringOverride(message.Subject);

            return message;
        }
    }
}