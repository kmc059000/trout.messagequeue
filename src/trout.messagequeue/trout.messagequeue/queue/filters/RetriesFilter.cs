﻿using System.Linq;
using trout.emailservice.model;

namespace trout.emailservice.queue.filters
{
    public class RetriesFilter : DequeueFilter
    {
        private readonly byte MaximumRetries;

        public RetriesFilter(byte maximumRetries)
        {
            MaximumRetries = maximumRetries;
        }

        public override IQueryable<EmailQueueItem> Filter(IQueryable<EmailQueueItem> source)
        {
            return source.Where(e => e.NumberTries < MaximumRetries);
        }
    }
}