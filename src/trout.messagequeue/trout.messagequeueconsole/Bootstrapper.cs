using StructureMap;
using trout.messagequeue.attachments;
using trout.messagequeue.config;
using trout.messagequeue.config.staticoverrides;
using trout.messagequeue.infrastrucure;
using trout.messagequeue.infrastrucure.dependencies;
using trout.messagequeue.model;
using trout.messagequeue.queue;
using trout.messagequeue.queue.overrides;
using trout.messagequeueconsole.commands;

namespace trout.messagequeueconsole
{
    public static class Bootstrapper
    {
        public static void Setup()
        {
            ObjectFactory.Configure(c =>
                                        {
                                            c.For<ISmtpClient>().Use<DotNetBuiltInSmtpClient>();
                                            c.For<IEmailQueueDbContext>().Singleton().Use(() => new EmailQueueDbContext());
                                            c.For<IMailMessageSenderConfig>().Use(MailMessageSenderConfig.GetMailMessageSenderConfig());
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
