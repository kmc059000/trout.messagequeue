using System;
using System.Net.Mail;

namespace trout.messagequeue.config
{
    public class MailMessageSenderConfig : IMailMessageSenderConfig
    {
        private readonly int maxTries;
        private readonly MailAddress fromAddress;
        private readonly String attachmentPath;

        public MailMessageSenderConfig(int maxTries = 5, string fromAddress = "from@example.com", string attachmentPath = "C:\\ProgramData\\trout\\attachments")
        {
            this.maxTries = maxTries;
            this.attachmentPath = attachmentPath;
            this.fromAddress = new MailAddress(fromAddress);
        }

        public int MaxTries
        {
            get { return maxTries; }
        }

        public MailAddress FromAddress
        {
            get { return fromAddress; }
        }

        public string AttachmentPath
        {
            get { return attachmentPath; }
        }
    }
}