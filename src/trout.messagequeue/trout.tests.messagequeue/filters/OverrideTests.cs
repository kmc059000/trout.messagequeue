using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Mail;
using System.Text;
using Moq;
using NUnit.Framework;
using trout.emailservice.model;
using trout.emailservice.queue.filters;
using trout.emailservice.queue.overrides;

namespace trout.tests.messagequeue.filters
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
    }
}
