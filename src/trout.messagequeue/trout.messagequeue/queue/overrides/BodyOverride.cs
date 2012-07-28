using System.Net.Mail;

namespace trout.messagequeue.queue.overrides
{
    /// <summary>
    /// Override for body field
    /// </summary>
    public sealed class BodyOverride : MailMessageOverride
    {
        /// <summary>
        /// Will apply overrides first, then apply prepend and append strings, and finally clear if needed
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public override MailMessage ApplyOverride(MailMessage message)
        {
            message.Body = ApplyStringOverride(message.Body);

            return message;
        }
    }
}