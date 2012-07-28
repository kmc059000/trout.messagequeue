using System.Collections.Generic;
using System.Net.Mail;

namespace trout.messagequeue.queue.overrides
{
    /// <summary>
    /// List of overrides to be applied to emails before they are sent.
    /// </summary>
    public sealed class OverrideList : List<MailMessageOverride>
    {
        /// <summary>
        /// Applies all overrides to the passed MailMessage
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public MailMessage ApplyOverrides(MailMessage message)
        {
            foreach (var overrideItem in this)
            {
                message = overrideItem.ApplyOverride(message);
            }

            return message;
        }
    }
}