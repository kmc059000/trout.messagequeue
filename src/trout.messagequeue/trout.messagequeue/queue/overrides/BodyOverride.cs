﻿using System.Net.Mail;

namespace trout.messagequeue.queue.overrides
{
    public sealed class BodyOverride : MailMessageOverride
    {
        public override MailMessage ApplyOverride(MailMessage message)
        {
            message.Body = ApplyStringOverride(message.Body);

            return message;
        }
    }
}