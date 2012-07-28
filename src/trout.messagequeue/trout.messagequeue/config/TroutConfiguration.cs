using System;
using System.Configuration;
using System.Net.Mail;

namespace trout.messagequeue.config
{
    /// <summary>
    /// Trout Configuration taken from configuration file
    /// </summary>
    public sealed class TroutConfiguration : ConfigurationSection, IMailMessageSenderConfig, IFileSystemAttachmentHandlerConfig
    {
        /// <summary>
        /// Loads a configuration instance from the configuration file
        /// </summary>
        /// <param name="sectionName"></param>
        /// <returns></returns>
        public static TroutConfiguration GetMailMessageSenderConfig(string sectionName = "trout")
        {
            TroutConfiguration section = (TroutConfiguration)ConfigurationManager.GetSection(sectionName);

            return section ?? new TroutConfiguration();
        }

        private TroutConfiguration()
        {
        }

        /// <summary>
        /// Number of times an email will be attempted to be sent
        /// </summary>
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

        /// <summary>
        /// Path for storage
        /// </summary>
        [ConfigurationProperty("storagePath", DefaultValue = "C:\\ProgramData\\trout\\", IsKey = false, IsRequired = false)]
        public string StoragePath
        {
            get { return (string)base["storagePath"]; }
            set { base["storagePath"] = value; }
        }


        /// <summary>
        /// Path for storing attachments
        /// </summary>
        public string AttachmentPath
        {
            get { return StoragePath + "attachments"; }
        }

        /// <summary>
        /// Address the emails should be sent from
        /// </summary>
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