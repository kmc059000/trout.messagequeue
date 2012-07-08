using System;
using System.Configuration;
using System.Net.Mail;

namespace trout.messagequeue.config
{
    public sealed class MailMessageSenderConfig : ConfigurationSection, IMailMessageSenderConfig
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
        private string fromAddress
        {
            get { return (string)base["fromAddress"]; }
            set { base["fromAddress"] = value; }
        }

        [ConfigurationProperty("fromName", DefaultValue = "Trout", IsKey = false, IsRequired = false)]
        private string fromName
        {
            get { return (string)base["fromName"]; }
            set { base["fromName"] = value; }
        }

        [ConfigurationProperty("storagePath", DefaultValue = "C:\\ProgramData\\trout\\", IsKey = false, IsRequired = false)]
        public string StoragePath
        {
            get { return (string)base["storagePath"]; }
            set { base["storagePath"] = value; }
        }

        
        public string AttachmentPath
        {
            get { return StoragePath + "attachments"; }
        }

        public MailAddress FromAddress
        {
            get
            {
                return new MailAddress(fromAddress);

                //this screws up with the Antix client. Not sure why and not sure if
                //it will do the same with a real smtp client.
                //so continue to just put the address and no name for the From address.
                //return new MailAddress(fromAddress, fromName);
            }
        }
    }
}