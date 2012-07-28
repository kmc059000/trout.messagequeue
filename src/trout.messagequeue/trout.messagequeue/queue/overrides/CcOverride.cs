using System.Net.Mail;

namespace trout.messagequeue.queue.overrides
{
    /// <summary>
    /// Override for cc field
    /// </summary>
    public sealed class CcOverride : MailMessageOverride
    {
        /// <summary>
        /// Will apply overrides first, then apply prepend and append strings, and finally clear if needed
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public override MailMessage ApplyOverride(MailMessage message)
        {
            this.ApplyMailAddressOverride(message.CC);

            return message;
        }
    }
}