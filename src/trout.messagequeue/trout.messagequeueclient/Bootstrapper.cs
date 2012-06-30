﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using StructureMap;
using trout.emailservice.config;
using trout.emailservice.infrastrucure;
using trout.emailservice.infrastrucure.dependencies;
using trout.emailservice.model;
using trout.emailservice.queue;
using trout.emailserviceclient.commands;

namespace trout.emailserviceclient
{
    public static class Bootstrapper
    {
        public static void Setup()
        {
            ObjectFactory.Configure(c =>
                                        {
                                            c.For<ISmtpClient>().Use<DotNetBuiltInSmtpClient>();
                                            c.For<IEmailQueueDbContext>().Singleton().Use(() => new EmailQueueDbContext());
                                            c.For<IMailMessageSenderConfig>().Use(new MailMessageSenderConfig());
                                            c.ForConcreteType<MailMessageQueue>();
                                            c.ForConcreteType<MailMessageDequeuer>();
                                            c.ForConcreteType<AddCommand>();
                                            c.ForConcreteType<ExitCommand>();
                                            c.ForConcreteType<HelpCommand>();
                                            c.ForConcreteType<ListCommand>();
                                            c.ForConcreteType<SendCommand>();
                                        }


                );

            DependencyResolver.SetDependencyResolver(new StructureMapDependencyResolver(ObjectFactory.Container));
        }

    }
}
