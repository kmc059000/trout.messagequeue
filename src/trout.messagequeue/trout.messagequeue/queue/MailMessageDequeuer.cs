using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using trout.messagequeue.attachments;
using trout.messagequeue.config;
using trout.messagequeue.infrastrucure;
using trout.messagequeue.infrastrucure.logging;
using trout.messagequeue.model;
using trout.messagequeue.model.repository;
using trout.messagequeue.queue.filters;
using trout.messagequeue.queue.overrides;
using trout.messagequeue.smtp;

namespace trout.messagequeue.queue
{
    /// <summary>
    /// Dequeuer for Emails which attempts to send emails.
    /// </summary>
    public sealed class MailMessageDequeuer
    {
        private readonly IMailMessageSenderConfig Config;
        private readonly ISmtpClient SmtpClient;
        private readonly IRepository<EmailQueueItem> Repository;
        private readonly IAttachmentFileSystem AttachmentFileSystem;
        private readonly IStaticOverridesProvider StaticOverridesesProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="config">The config.</param>
        /// <param name="smtpClient">The SMTP client.</param>
        /// <param name="repository">The repository.</param>
        /// <param name="attachmentFileSystem">The attachment file system.</param>
        /// <param name="staticOverridesesProvider">The static overrideses provider.</param>
        public MailMessageDequeuer(IMailMessageSenderConfig config, ISmtpClient smtpClient, IRepository<EmailQueueItem> repository, IAttachmentFileSystem attachmentFileSystem, IStaticOverridesProvider staticOverridesesProvider)
        {
            Config = config;
            Repository = repository;
            StaticOverridesesProvider = staticOverridesesProvider;
            AttachmentFileSystem = attachmentFileSystem;
            SmtpClient = smtpClient;
            
        }

        /// <summary>
        /// Sends the messages which have not been sent yet. Static overrides are applied.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DequeueResultItem> SendQueuedMessages()
        {
            var filters = new DequeueFilterList().And(new SentStatusDequeueFilter(false));
            var overrides = new OverrideList();

            return SendQueuedMessages(filters, overrides, true);
        }

        /// <summary>
        /// Sends the messages which match the filters and applies the overrides prior to sending
        /// </summary>
        /// <param name="filters"></param>
        /// <param name="overrides"></param>
        /// <param name="audit">whether to change the sent status and number of tries for an email</param>
        /// <returns></returns>
        public IEnumerable<DequeueResultItem> SendQueuedMessages(DequeueFilterList filters, OverrideList overrides, bool audit = true)
        {
            if(filters == null) throw new ArgumentNullException("filters");
            if(overrides == null) throw new ArgumentNullException("overrides");

            TroutLog.Log.Info(string.Format("Beginning dequeuing with{0} auditing at", audit ? "" : "out"));

            List<DequeueResultItem> results = new List<DequeueResultItem>();
            var messages = filters.Filter(Repository);

            foreach (var message in messages.ToList())
            {
                MailMessage mailMessage = GetMailMessage(message, overrides);

                foreach (var attachment in AttachmentFileSystem.GetAttachments(message))
                {
                    mailMessage.Attachments.Add(attachment);
                } 

                var result = SmtpClient.Send(mailMessage);
                results.Add(new DequeueResultItem(message, result.IsSuccess, result.Message, mailMessage, result.Tries));

                TroutLog.Log.Info(string.Format("{0} was {1}sent with message - {2} after {3} tries.", message.ID, result.IsSuccess ? "" : "not ", result.Message, result.Tries));

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

            TroutLog.Log.Info(string.Format("Saving of dequeue results"));
            Repository.SaveChanges();


            return results;
        }

        /// <summary>
        /// Returns emails which match the filter with the provided overrides applied.
        /// </summary>
        /// <param name="filters"></param>
        /// <param name="overrides"></param>
        /// <returns></returns>
        public IEnumerable<DequeueListItem> GetQueuedMessages(DequeueFilterList filters, OverrideList overrides)
        {
            if (filters == null) throw new ArgumentNullException("filters");
            if (overrides == null) throw new ArgumentNullException("overrides");

            TroutLog.Log.Info(string.Format("Retrieving messages"));

            List<DequeueListItem> results = new List<DequeueListItem>();

            var messages = filters.Filter(Repository);

            foreach (var message in messages.ToList())
            {
                MailMessage mailMessage = GetMailMessage(message, overrides);

                results.Add(new DequeueListItem(message, mailMessage));
            }

            TroutLog.Log.Info(string.Format("Retrieved {0} messages", results.Count));

            return results;
        }

        private MailMessage GetMailMessage(EmailQueueItem message, OverrideList overrides)
        {
            var mailMessage = new MailMessage();
            mailMessage.From = Config.FromAddress;
            if (!string.IsNullOrWhiteSpace(message.To)) mailMessage.To.Add(message.To);
            if (!string.IsNullOrWhiteSpace(message.Cc)) mailMessage.CC.Add(message.Cc);
            if (!string.IsNullOrWhiteSpace(message.Bcc)) mailMessage.Bcc.Add(message.Bcc);
            mailMessage.Subject = message.Subject;
            mailMessage.Body = message.Body;
            mailMessage.IsBodyHtml = message.IsBodyHtml;

            mailMessage = StaticOverridesesProvider.StaticOverrides.ApplyOverrides(mailMessage);
            mailMessage = overrides.ApplyOverrides(mailMessage);
            return mailMessage;
        }
    }
}
