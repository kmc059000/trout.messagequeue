using System.Collections.Generic;
using System.Net.Mail;

namespace trout.messagequeue.queue.overrides
{
    public class OverrideList : List<MailMessageOverride>
    {
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