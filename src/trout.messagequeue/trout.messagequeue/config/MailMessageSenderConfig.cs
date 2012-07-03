﻿using System;
using System.Configuration;
using System.Net.Mail;

namespace trout.messagequeue.config
{
    public class MailMessageSenderConfig : ConfigurationSection, IMailMessageSenderConfig
    {
        public static MailMessageSenderConfig GetMailMessageSenderConfig(string sectionName = "trout")
        {
            MailMessageSenderConfig section = (MailMessageSenderConfig)ConfigurationManager.GetSection(sectionName);

            return section ?? new MailMessageSenderConfig();
        }

        private MailMessageSenderConfig()
        {
        }

        [ConfigurationProperty("maxTries", DefaultValue = 5, IsKey = false, IsRequired = false)]
        public int MaxTries
        {
            get { return (int)base["maxTries"]; }
            set { base["maxTries"] = value; }
        }

        [ConfigurationProperty("fromAddress", DefaultValue = "trout@example.com", IsKey = false, IsRequired = false)]
        protected string fromAddress
        {
            get { return (string)base["fromAddress"]; }
            set { base["fromAddress"] = value; }
        }

        [ConfigurationProperty("fromName", DefaultValue = "Trout", IsKey = false, IsRequired = false)]
        protected string fromName
        {
            get { return (string)base["fromName"]; }
            set { base["fromName"] = value; }
        }

        [ConfigurationProperty("attachmentPath", DefaultValue = "C:\\ProgramData\\trout\\attachments", IsKey = false, IsRequired = false)]
        public string AttachmentPath
        {
            get { return (string)base["attachmentPath"]; }
            set { base["attachmentPath"] = value; }
        }

        public MailAddress FromAddress
        {
            get { return new MailAddress(fromAddress, fromName); }
        }
    }
}