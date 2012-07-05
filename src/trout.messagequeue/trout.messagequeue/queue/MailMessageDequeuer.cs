using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using trout.messagequeue.attachments;
using trout.messagequeue.config;
using trout.messagequeue.infrastrucure;
using trout.messagequeue.model;
using trout.messagequeue.queue.filters;
using trout.messagequeue.queue.overrides;

namespace trout.messagequeue.queue
{
    public class MailMessageDequeuer
    {
        private readonly IMailMessageSenderConfig Config;
        private readonly ISmtpClient SmtpClient;
        private readonly IEmailQueueDbContext Context;
        private readonly IAttachmentFileSystem AttachmentFileSystem;

        public MailMessageDequeuer(IMailMessageSenderConfig config, ISmtpClient smtpClient, IEmailQueueDbContext context, IAttachmentFileSystem attachmentFileSystem)
        {
            Config = config;
            AttachmentFileSystem = attachmentFileSystem;
            Context = context;
            SmtpClient = smtpClient;
        }

        public IEnumerable<DequeueResultItem> SendQueuedMessages(DequeueFilterList filters, OverrideList overrides, bool audit = true)
        {
            List<DequeueResultItem> results = new List<DequeueResultItem>();
            var staticOverrideList = StaticOverrideList.GetStaticOverrideList();
            var messages = filters.Filter(Context);

            foreach (var message in messages.ToList())
            {
                MailMessage mailMessage = GetMailMessage(message, staticOverrideList, overrides);

                foreach (var attachment in AttachmentFileSystem.GetAttachments(message))
                {
                    mailMessage.Attachments.Add(attachment);
                } 

                var result = SmtpClient.Send(mailMessage);
                results.Add(new DequeueResultItem(message, result.IsSuccess, result.Message, mailMessage));

                if (audit)
                {
                    if (result.IsSuccess)
                    {
                        message.IsSent = true;
                        message.SendDate = DateTime.Now;
                    }
                    else
                    {
                        message.IsSent = false;
                        message.SendDate = null;
                    }

                    message.NumberTries++;
                    message.LastTryDate = DateTime.Now;
                }
            }

            Context.SaveChanges();


            return results;
        }

        public IEnumerable<DequeueListItem> GetQueuedMessages(DequeueFilterList filters, OverrideList overrides)
        {
            List<DequeueListItem> results = new List<DequeueListItem>();
            var staticOverrideList = StaticOverrideList.GetStaticOverrideList();

            var messages = filters.Filter(Context);

            foreach (var message in messages.ToList())
            {
                MailMessage mailMessage = GetMailMessage(message, staticOverrideList, overrides);

                results.Add(new DequeueListItem(message, mailMessage));
            }

            return results;
        }

        private MailMessage GetMailMessage(EmailQueueItem message, OverrideList staticOverrideList, OverrideList overrides)
        {
            var mailMessage = new MailMessage();
            mailMessage.From = Config.FromAddress;
            if (!string.IsNullOrWhiteSpace(message.To)) mailMessage.To.Add(message.To);
            if (!string.IsNullOrWhiteSpace(message.Cc)) mailMessage.CC.Add(message.Cc);
            if (!string.IsNullOrWhiteSpace(message.Bcc)) mailMessage.Bcc.Add(message.Bcc);
            mailMessage.Subject = message.Subject;
            mailMessage.Body = message.Body;
            mailMessage.IsBodyHtml = message.IsBodyHtml;

            mailMessage = staticOverrideList.ApplyOverrides(mailMessage);
            mailMessage = overrides.ApplyOverrides(mailMessage);
            return mailMessage;
        }
    }
}
