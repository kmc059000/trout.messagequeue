using System.Net.Mail;

namespace trout.emailservice.queue.overrides
{
    public class SubjectOverride : MailMessageOverride
    {
        public override MailMessage ApplyOverride(MailMessage message)
        {
            message.Subject = ApplyStringOverride(message.Subject);

            return message;
        }
    }
}