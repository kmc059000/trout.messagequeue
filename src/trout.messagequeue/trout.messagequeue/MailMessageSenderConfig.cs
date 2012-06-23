using System;
using System.Net.Mail;

namespace trout.emailservice
{
    public class MailMessageSenderConfig : IMailMessageSenderConfig
    {
        private readonly int maxTries;
        private readonly MailAddress fromAddress;

        public MailMessageSenderConfig(int maxTries = 5, string fromAddress = "from@example.com")
        {
            this.maxTries = maxTries;
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
    }
}