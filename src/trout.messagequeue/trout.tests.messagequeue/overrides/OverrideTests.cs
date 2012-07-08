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

namespace trout.tests.messagequeue.overrides
{
    [TestFixture]
    public class OverrideTests
    {
        private MailMessage GetTestingEmail()
        {
            var mailMessage = new MailMessage();

            mailMessage.To.Add("user1@example.com, user2@example.com");
            mailMessage.CC.Add("user1@example.com, user2@example.com");
            mailMessage.Bcc.Add("user1@example.com, user2@example.com");
            mailMessage.Subject = "Test Email Subject";
            mailMessage.Body = "Test Email Body";

            return mailMessage;
        }

        [Test]
        public void TestToOverride()
        {
            var filter = new ToOverride();
            
            filter.Override("user3@example.com");
            Assert.AreEqual(filter.ApplyOverride(GetTestingEmail()).To.ToString(), "user3@example.com", "ToOverride.Override()");

            filter = new ToOverride();
            filter.Clear();
            Assert.AreEqual(filter.ApplyOverride(GetTestingEmail()).To.ToString(), "", "ToOverride.Clear()");

            filter = new ToOverride();
            filter.Prepend("user3@example.com");
            Assert.AreEqual(filter.ApplyOverride(GetTestingEmail()).To.ToString(), "user3@example.com, user1@example.com, user2@example.com", "ToOverride.Preprend()");

            filter = new ToOverride();
            filter.Append("user3@example.com");
            Assert.AreEqual(filter.ApplyOverride(GetTestingEmail()).To.ToString(), "user1@example.com, user2@example.com, user3@example.com", "ToOverride.Append()");

            filter = new ToOverride();
            filter.Override("user3@example.com");
            filter.Append("user4@example.com");
            filter.Prepend("user5@example.com");
            Assert.AreEqual(filter.ApplyOverride(GetTestingEmail()).To.ToString(), "user5@example.com, user3@example.com, user4@example.com", "ToOverride.Override() -> Append -> Prepend");
        }

        [Test]
        public void TestCcOverride()
        {
            var filter = new CcOverride();

            filter.Override("user3@example.com");
            Assert.AreEqual(filter.ApplyOverride(GetTestingEmail()).CC.ToString(), "user3@example.com", "CcOverride.Override()");

            filter = new CcOverride();
            filter.Clear();
            Assert.AreEqual(filter.ApplyOverride(GetTestingEmail()).CC.ToString(), "", "CcOverride.Clear()");

            filter = new CcOverride();
            filter.Prepend("user3@example.com");
            Assert.AreEqual(filter.ApplyOverride(GetTestingEmail()).CC.ToString(), "user3@example.com, user1@example.com, user2@example.com", "CcOverride.Preprend()");

            filter = new CcOverride();
            filter.Append("user3@example.com");
            Assert.AreEqual(filter.ApplyOverride(GetTestingEmail()).CC.ToString(), "user1@example.com, user2@example.com, user3@example.com", "CcOverride.Append()");

            filter = new CcOverride();
            filter.Override("user3@example.com");
            filter.Append("user4@example.com");
            filter.Prepend("user5@example.com");
            Assert.AreEqual(filter.ApplyOverride(GetTestingEmail()).CC.ToString(), "user5@example.com, user3@example.com, user4@example.com", "CcOverride.Override() -> Append -> Prepend");
        }

        [Test]
        public void TestBccOverride()
        {
            var filter = new BccOverride();

            filter.Override("user3@example.com");
            Assert.AreEqual(filter.ApplyOverride(GetTestingEmail()).Bcc.ToString(), "user3@example.com", "BccOverride.Override()");

            filter = new BccOverride();
            filter.Clear();
            Assert.AreEqual(filter.ApplyOverride(GetTestingEmail()).Bcc.ToString(), "", "BccOverride.Clear()");

            filter = new BccOverride();
            filter.Prepend("user3@example.com");
            Assert.AreEqual(filter.ApplyOverride(GetTestingEmail()).Bcc.ToString(), "user3@example.com, user1@example.com, user2@example.com", "BccOverride.Preprend()");

            filter = new BccOverride();
            filter.Append("user3@example.com");
            Assert.AreEqual(filter.ApplyOverride(GetTestingEmail()).Bcc.ToString(), "user1@example.com, user2@example.com, user3@example.com", "BccOverride.Append()");

            filter = new BccOverride();
            filter.Override("user3@example.com");
            filter.Append("user4@example.com");
            filter.Prepend("user5@example.com");
            Assert.AreEqual(filter.ApplyOverride(GetTestingEmail()).Bcc.ToString(), "user5@example.com, user3@example.com, user4@example.com", "BccOverride.Override() -> Append -> Prepend");
        }

        [Test]
        public void TestSubjectOverride()
        {
            var filter = new SubjectOverride();

            filter.Override("SUBJECT");
            Assert.AreEqual(filter.ApplyOverride(GetTestingEmail()).Subject, "SUBJECT", "SubjectOverride.Override()");

            filter = new SubjectOverride();
            filter.Clear();
            Assert.AreEqual(filter.ApplyOverride(GetTestingEmail()).Subject, "", "SubjectOverride.Clear()");

            filter = new SubjectOverride();
            filter.Prepend("PREPEND - ");
            Assert.AreEqual(filter.ApplyOverride(GetTestingEmail()).Subject, "PREPEND - Test Email Subject", "SubjectOverride.Preprend()");

            filter = new SubjectOverride();
            filter.Append(" - APPEND");
            Assert.AreEqual(filter.ApplyOverride(GetTestingEmail()).Subject, "Test Email Subject - APPEND", "SubjectOverride.Append()");

            filter = new SubjectOverride();
            filter.Override("NEW SUBJECT");
            filter.Append("AP");
            filter.Prepend("PP");
            Assert.AreEqual(filter.ApplyOverride(GetTestingEmail()).Subject, "PPNEW SUBJECTAP", "SubjectOverride.Override() -> Append -> Prepend");
        }

        [Test]
        public void TestBodyOverride()
        {
            var filter = new BodyOverride();

            filter.Override("BODY");
            Assert.AreEqual(filter.ApplyOverride(GetTestingEmail()).Body, "BODY", "BodyOverride.Override()");

            filter = new BodyOverride();
            filter.Clear();
            Assert.AreEqual(filter.ApplyOverride(GetTestingEmail()).Body, "", "BodyOverride.Clear()");

            filter = new BodyOverride();
            filter.Prepend("PREPEND - ");
            Assert.AreEqual(filter.ApplyOverride(GetTestingEmail()).Body, "PREPEND - Test Email Body", "BodyOverride.Preprend()");

            filter = new BodyOverride();
            filter.Append(" - APPEND");
            Assert.AreEqual(filter.ApplyOverride(GetTestingEmail()).Body, "Test Email Body - APPEND", "BodyOverride.Append()");

            filter = new BodyOverride();
            filter.Override("NEW BODY");
            filter.Append("AP");
            filter.Prepend("PP");
            Assert.AreEqual(filter.ApplyOverride(GetTestingEmail()).Body, "PPNEW BODYAP", "BodyOverride.Override() -> Append -> Prepend");
        }

        [Test]
        public void StaticOverridesAreApplied()
        {
            var testingStaticOverrides = new OverrideList();
            testingStaticOverrides.Add(new ToOverride().Prepend("user@troutproject.info"));
            testingStaticOverrides.Add(new CcOverride().Prepend("usercc@troutproject.info"));
            testingStaticOverrides.Add(new BccOverride().Prepend("userbcc@troutproject.info"));
            testingStaticOverrides.Add(new SubjectOverride().Prepend("SubjectSpecialString"));
            testingStaticOverrides.Add(new BodyOverride().Prepend("BodySpecialString"));

            var staticOverridesProviderMock = new Mock<IStaticOverridesProvider>();
            staticOverridesProviderMock.Setup(m => m.StaticOverrides).Returns(testingStaticOverrides);

            MailMessageQueue queue = new MailMessageQueue(Context, AttachmentFileSystem);
            MailMessageDequeuer dequeuer = new MailMessageDequeuer(Config, SmtpClient, Context, AttachmentFileSystem, staticOverridesProviderMock.Object);

            var mm = new MailMessage();

            queue.EnqueueMessage(mm);

            var emailQueueItem = Context.EmailQueueItemRepo.Fetch().First();

            var fl = new DequeueFilterList();
            fl.And(new IdDequeueFilter(emailQueueItem.ID));

            var result = dequeuer.SendQueuedMessages(fl, new OverrideList());

            var sent = result.First().SentMailMessage;

            Assert.AreEqual("user@troutproject.info", sent.To.ToString(), "Static To Override was not applied");
            Assert.AreEqual("usercc@troutproject.info", sent.CC.ToString(), "Static Cc Override was not applied");
            Assert.AreEqual("userbcc@troutproject.info", sent.Bcc.ToString(), "Static Bcc Override was not applied");
            Assert.AreEqual("SubjectSpecialString", sent.Subject, "Static Subject Override was not applied");
            Assert.AreEqual("BodySpecialString", sent.Body, "Static Body Override was not applied");
        }


        private IEmailQueueDbContext Context;
        private IAttachmentFileSystem AttachmentFileSystem;
        private IMailMessageSenderConfig Config;
        private ISmtpClient SmtpClient;

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
