using System;
using System.IO;
using System.Linq;
using System.Net.Mail;
using Moq;
using NUnit.Framework;
using trout.messagequeue.attachments;
using trout.messagequeue.config;
using trout.messagequeue.infrastrucure;
using trout.messagequeue.model;
using trout.messagequeue.model.repository;
using trout.messagequeue.queue;
using trout.messagequeue.queue.filters;
using trout.messagequeue.queue.overrides;
using trout.tests.messagequeue.mocks;

namespace trout.tests.messagequeue.queue
{
    [TestFixture]
    public class QueueTests
    {
        private IEmailQueueDbContext Context;
        private IAttachmentFileSystem AttachmentFileSystem;
        private IMailMessageSenderConfig Config;
        private ISmtpClient SmtpClient;

        [Test]
        public void EmailsAreSentWithOriginalAttachments()
        {
            MailMessageQueue queue = new MailMessageQueue(Context, AttachmentFileSystem);
            MailMessageDequeuer dequeuer = new MailMessageDequeuer(Config, SmtpClient, Context, AttachmentFileSystem);

            var mm = new MailMessage();
            mm.Attachments.Add(new Attachment(Path.Combine(Environment.CurrentDirectory, @"testingfiles\sample.pdf")));

            queue.EnqueueMessage(mm);

            var emailQueueItem = Context.EmailQueueItemRepo.Fetch().First();

            var fl = new DequeueFilterList();
            fl.And(new IdDequeueFilter(emailQueueItem.ID));

            var result = dequeuer.SendQueuedMessages(fl, new OverrideList());

            Assert.IsNotNull(result, "Result is null");
            Assert.IsNotNull(result.FirstOrDefault(), "0 results were received");
            Assert.IsNotNull(result.FirstOrDefault().SentMailMessage, "Mail Message of result is null");
            Assert.AreEqual(1, result.FirstOrDefault().SentMailMessage.Attachments.Count, "Attachment count is incorrect");

            string originalFile;
            using (StreamReader sr = new StreamReader(Path.Combine(Environment.CurrentDirectory, @"testingfiles\sample.pdf")))
            {
                originalFile = sr.ReadToEnd();
            }

            string newFile;
            using (StreamReader sr = new StreamReader(result.FirstOrDefault().SentMailMessage.Attachments[0].ContentStream))
            {
                newFile = sr.ReadToEnd();
            }

            Assert.AreEqual(originalFile, newFile, "provided and sent attachments are not identical");
        }

        [Test]
        public void MultipleAttachmentsAreSent()
        {
            MailMessageQueue queue = new MailMessageQueue(Context, AttachmentFileSystem);
            MailMessageDequeuer dequeuer = new MailMessageDequeuer(Config, SmtpClient, Context, AttachmentFileSystem);

            var mm = new MailMessage();
            mm.Attachments.Add(new Attachment(Path.Combine(Environment.CurrentDirectory, @"testingfiles\sample.pdf")));
            mm.Attachments.Add(new Attachment(Path.Combine(Environment.CurrentDirectory, @"testingfiles\sample.txt")));

            queue.EnqueueMessage(mm);

            var emailQueueItem = Context.EmailQueueItemRepo.Fetch().First();

            var fl = new DequeueFilterList();
            fl.And(new IdDequeueFilter(emailQueueItem.ID));

            var result = dequeuer.SendQueuedMessages(fl, new OverrideList());

            Assert.IsNotNull(result, "Result is null");
            Assert.IsNotNull(result.FirstOrDefault(), "0 results were received");
            Assert.IsNotNull(result.FirstOrDefault().SentMailMessage, "Mail Message of result is null");
            Assert.AreEqual(2, result.FirstOrDefault().SentMailMessage.Attachments.Count, "Attachment count is incorrect");

            string originalFile;
            using (StreamReader sr = new StreamReader(Path.Combine(Environment.CurrentDirectory, @"testingfiles\sample.pdf")))
            {
                originalFile = sr.ReadToEnd();
            }

            string newFile;
            using (StreamReader sr = new StreamReader(result.FirstOrDefault().SentMailMessage.Attachments[0].ContentStream))
            {
                newFile = sr.ReadToEnd();
            }

            Assert.AreEqual(originalFile, newFile, "provided and sent attachments are not identical");


            using (StreamReader sr = new StreamReader(Path.Combine(Environment.CurrentDirectory, @"testingfiles\sample.txt")))
            {
                originalFile = sr.ReadToEnd();
            }

            using (StreamReader sr = new StreamReader(result.FirstOrDefault().SentMailMessage.Attachments[1].ContentStream))
            {
                newFile = sr.ReadToEnd();
            }

            Assert.AreEqual(originalFile, newFile, "provided and sent attachments are not identical");
        }

        [SetUp]
        public void Setup()
        {
            var contextMock = new Mock<IEmailQueueDbContext>();
            var repo = new InMemoryRepository<EmailQueueItem>();
            contextMock.Setup(c => c.EmailQueueItemRepo).Returns(repo);
            contextMock.Setup(c => c.SaveChanges());

            Context = contextMock.Object;
            AttachmentFileSystem = new InMemoryAttachmentFileSystem();

            var configMock = new Mock<IMailMessageSenderConfig>();
            configMock.Setup(c => c.MaxTries).Returns(5);
            configMock.Setup(c => c.FromAddress).Returns(new MailAddress("donotreply@troutproject.info"));

            Config = configMock.Object;

            var smtpClientMock = new Mock<ISmtpClient>();
            smtpClientMock.Setup(m => m.Send(It.IsAny<MailMessage>())).Returns(new SendResult(true, "mock success", 1));
            SmtpClient = smtpClientMock.Object;

        }

    }
}
