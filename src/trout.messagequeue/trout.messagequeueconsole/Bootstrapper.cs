using StructureMap;
using trout.messagequeue.attachments;
using trout.messagequeue.config;
using trout.messagequeue.config.staticoverrides;
using trout.messagequeue.infrastrucure;
using trout.messagequeue.model;
using trout.messagequeue.model.repository;
using trout.messagequeue.queue;
using trout.messagequeue.queue.overrides;
using trout.messagequeue.smtp;
using trout.messagequeueconsole.commands;
using trout.messagequeueconsole.infrastrucure;
using trout.messagequeueconsole.infrastrucure.dependencies;

namespace trout.messagequeueconsole
{
    public static class Bootstrapper
    {
        public static void Setup()
        {
            ObjectFactory.Configure(c =>
                                        {
                                            c.For<ISmtpClient>().Use<DotNetBuiltInSmtpClient>();
                                            c.For<IRepository<EmailQueueItem>>().Singleton().Use(() => new DbSetRepository<EmailQueueItem>(new EmailQueueDbContext()));
                                            var config = TroutConfiguration.GetMailMessageSenderConfig();
                                            c.For<IMailMessageSenderConfig>().Use(config);
                                            c.For<IMailMessageSenderConfig>().Use(config);
                                            c.For<IFileSystemAttachmentHandlerConfig>().Use(config);
                                            c.For<IAttachmentFileSystem>().Use<AttachmentFileSystem>();
                                            c.For<IStaticOverridesProvider>().Use<ConfigFileStaticOverridesProvider>();
                                            c.ForConcreteType<MailMessageQueue>();
                                            c.ForConcreteType<MailMessageDequeuer>();
                                            c.ForConcreteType<AddCommand>();
                                            c.ForConcreteType<ExitCommand>();
                                            c.ForConcreteType<HelpCommand>();
                                            c.ForConcreteType<ListCommand>();
                                            c.ForConcreteType<SendCommand>();
                                            c.ForConcreteType<EditCommand>();
                                            c.ForConcreteType<AttachmentsCommand>();
                                        }


                );

            DependencyResolver.SetDependencyResolver(new StructureMapDependencyResolver(ObjectFactory.Container));
        }

    }
}
