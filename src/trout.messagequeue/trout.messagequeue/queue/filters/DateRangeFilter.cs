﻿using System;
using System.Linq;
using trout.emailservice.model;

namespace trout.emailservice.queue.filters
{
    public class DateRangeFilter : DequeueFilter
    {
        private readonly DateTime FromDate, ToDate;

        public DateRangeFilter(DateTime fromDate, DateTime toDate)
        {
            FromDate = fromDate;
            ToDate = toDate;
        }

        public override IQueryable<EmailQueueItem> Filter(IQueryable<EmailQueueItem> source)
        {
            return source.Where(e => e.CreateDate >= FromDate && e.CreateDate <= ToDate);
        }
    }
}