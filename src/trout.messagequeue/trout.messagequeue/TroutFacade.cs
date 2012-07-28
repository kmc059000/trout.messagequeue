using trout.messagequeue.attachments;
using trout.messagequeue.config;
using trout.messagequeue.config.staticoverrides;
using trout.messagequeue.model;
using trout.messagequeue.queue;
using trout.messagequeue.smtp;

namespace trout.messagequeue
{
    /// <summary>
    /// Facade for easily getting an instance of a MailMessageQueue and MailMessageDequeuer. 
    /// </summary>
    public class TroutFacade
    {
        protected IAttachmentFileSystem AttachmentFileSystem { get; set; }
        protected IFileSystemAttachmentHandlerConfig FileSystemAttachmentHandlerConfig { get; set; }
        protected IMailMessageSenderConfig MailMessageSenderConfig { get; set; }
        protected ConfigFileStaticOverridesProvider StaticOverridesProvider { get; set; }
        protected IEmailQueueDbContext EmailQueueDbContext { get; set; }
        protected ISmtpClient SmtpClient { get; set; }

        /// <summary>
        /// An instance of MailMessageQueue which remains constant for the life of the TroutFacade
        /// </summary>
        public MailMessageQueue MailMessageQueue { get; protected set; }
        
        /// <summary>
        /// An instance of MailMessageDequeuer which remains constant for the life of the TroutFacade
        /// </summary>
        public MailMessageDequeuer MailMessageDequeuer { get; protected set; }

        /// <summary>
        /// Initializes MailMessageQueue and MailMessageDequeuer properties
        /// </summary>
        public TroutFacade()
        {
            var config = TroutConfiguration.GetMailMessageSenderConfig();
            this.FileSystemAttachmentHandlerConfig = config;
            this.MailMessageSenderConfig = config;

            this.AttachmentFileSystem = new AttachmentFileSystem(this.FileSystemAttachmentHandlerConfig);
            this.StaticOverridesProvider = new ConfigFileStaticOverridesProvider();

            this.EmailQueueDbContext = new EmailQueueDbContext();

            this.MailMessageQueue = new MailMessageQueue(this.EmailQueueDbContext, this.AttachmentFileSystem);
            this.SmtpClient = new DotNetBuiltInSmtpClient();
            this.MailMessageDequeuer = new MailMessageDequeuer(MailMessageSenderConfig, SmtpClient, EmailQueueDbContext, AttachmentFileSystem, StaticOverridesProvider);
    
        }
    }
}
