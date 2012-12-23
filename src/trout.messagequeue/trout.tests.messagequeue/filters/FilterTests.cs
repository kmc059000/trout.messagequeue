using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using Moq;
using NUnit.Framework;
using trout.messagequeue.model;
using trout.messagequeue.model.repository;
using trout.messagequeue.queue.filters;

namespace trout.tests.messagequeue.filters
{
    [TestFixture]
    public class FilterTests
    {
        private IRepository<EmailQueueItem> Repository;

        public FilterTests()
        {
            Setup();
        }

        [Test]
        public void DefaultResults()
        {
            var filters  = new DequeueFilterList();

            var result = filters.Filter(Repository).Select(r => r.ID);
            var expected = new int[] {3};

            Assert.That(AreArraysEqual(result, expected));
        }

        [Test]
        public void IdDequeueFilterResults()
        {
            var filters = new DequeueFilterList();

            var result = filters.And(new IdDequeueFilter(1)).Filter(Repository).Select(r => r.ID);
            var expected = new int[] { 1 };

            Assert.That(AreArraysEqual(result, expected));
        }

        [Test]
        public void CanFilterTwice()
        {
            var filters = new DequeueFilterList();

            var result = filters.And(new IdDequeueFilter(1)).Filter(Repository).Select(r => r.ID);
            var expected = new int[] { 1 };

            Assert.That(AreArraysEqual(result, expected));

            //do it again with the same filterset
            result = filters.Filter(Repository).Select(r => r.ID);
            expected = new int[] { 1 };

            Assert.That(AreArraysEqual(result, expected));
        }

        [Test]
        public void BodyContainsFilterResults()
        {
            var filters = new DequeueFilterList();

            var result = filters.And(new BodyContainsFilter("SEARCHSTRING")).Filter(Repository).Select(r => r.ID);
            var expected = new int[] { 2 };

            Assert.That(AreArraysEqual(result, expected));
        }

        [Test]
        public void BodyExactFilterResults()
        {
            var filters = new DequeueFilterList();

            var result = filters.And(new BodyExactFilter("Test Message - EXACTSEARCH")).Filter(Repository).Select(r => r.ID);
            var expected = new int[] { 3 };

            Assert.That(AreArraysEqual(result, expected));
        }

        [Test]
        public void DateRangeFilterResults()
        {
            var filters = new DequeueFilterList();

            var result = filters.And(new DateRangeFilter(DateTime.Now.AddDays(1), DateTime.Now.AddDays(2))).Filter(Repository).Select(r => r.ID);
            var expected = new int[] { 3 };

            Assert.That(AreArraysEqual(result, expected));
        }

        [Test]
        public void RetriesFilterResults()
        {
            var filters = new DequeueFilterList();

            var result = filters.And(new RetriesFilter(0)).Filter(Repository).Select(r => r.ID);
            var expected = new int[] {  };

            Assert.That(AreArraysEqual(result, expected));

            filters = new DequeueFilterList();

            result = filters.And(new RetriesFilter(1)).Filter(Repository).Select(r => r.ID);
            expected = new int[] { 3 };

            Assert.That(AreArraysEqual(result, expected));

            filters = new DequeueFilterList();

            result = filters.And(new RetriesFilter(6)).Filter(Repository).Select(r => r.ID);
            expected = new int[] {1, 2, 3};

            Assert.That(AreArraysEqual(result, expected));
        }

        [Test]
        public void SentStatusFilterResults()
        {
            var filters = new DequeueFilterList();

            var result = filters.And(new SentStatusDequeueFilter(false)).Filter(Repository).Select(r => r.ID);
            var expected = new int[] {2, 3};

            Assert.That(AreArraysEqual(result, expected));

            filters = new DequeueFilterList();

            result = filters.And(new SentStatusDequeueFilter(true)).Filter(Repository).Select(r => r.ID);
            expected = new int[] { 1 };

            Assert.That(AreArraysEqual(result, expected));
        }

        [Test]
        public void SubjectContainsFilterResults()
        {
            var filters = new DequeueFilterList();

            var result = filters.And(new SubjectContainsFilter("SOMESUBJECTSTRING")).Filter(Repository).Select(r => r.ID);
            var expected = new int[] { 2 };

            Assert.That(AreArraysEqual(result, expected));
        }

        [Test]
        public void SubjectExactFilterResults()
        {
            var filters = new DequeueFilterList();

            var result = filters.And(new SubjectExactFilter("EXACTSUBJECTSTRING")).Filter(Repository).Select(r => r.ID);
            var expected = new int[] { 3 };

            Assert.That(AreArraysEqual(result, expected));
        }

        [Test]
        public void ToFilterResults()
        {
            var filters = new DequeueFilterList();

            var result = filters.And(new ToFilter("user1@example.com")).Filter(Repository).Select(r => r.ID);
            var expected = new int[] { 1 };

            Assert.That(AreArraysEqual(result, expected));
        }

        private bool AreArraysEqual(IEnumerable<int> result, IEnumerable<int> expected)
        {
            var list1Groups = result.ToLookup(i => i);
            var list2Groups = expected.ToLookup(i => i);
            return list1Groups.Count == list2Groups.Count
                           && list1Groups.All(g => g.Count() == list2Groups[g.Key].Count());
        }

        private void Setup()
        {
            var contextMock = new Mock<IRepository<EmailQueueItem>>();

            var lst = new List<EmailQueueItem>();
            lst.Add(new EmailQueueItem()
            {
                ID = 1,
                To = "user1@example.com",
                Cc = "",
                Bcc = "",
                Subject = "EXACTSUBJECTSTRING - blah blah blah",
                Body = "Test Message",
                CreateDate = DateTime.Now,
                IsFailed = false,
                IsSent = true,
                LastTryDate = null,
                NumberTries = 1
            });
            lst.Add(new EmailQueueItem()
            {
                ID = 2,
                To = "user2@example.com",
                Cc = "",
                Bcc = "",
                Subject = "SOMESUBJECTSTRING - Blah Blah Blah",
                Body = "Test Message - SEARCHSTRING",
                CreateDate = DateTime.Now,
                IsFailed = false,
                IsSent = false,
                LastTryDate = null,
                NumberTries = 5
            });
            lst.Add(new EmailQueueItem()
            {
                ID = 3,
                To = "user3@example.com",
                Cc = "",
                Bcc = "",
                Subject = "EXACTSUBJECTSTRING",
                Body = "Test Message - EXACTSEARCH",
                CreateDate = DateTime.Now.AddDays(1.5),
                IsFailed = false,
                IsSent = false,
                LastTryDate = null,
                NumberTries = 0
            });


            contextMock.Setup(context => context.Fetch()).Returns(lst.AsQueryable());

            Repository = contextMock.Object;
        }
    }
}
