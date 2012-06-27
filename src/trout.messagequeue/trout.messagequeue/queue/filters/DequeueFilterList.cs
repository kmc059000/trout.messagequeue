using System;
using System.Collections.Generic;
using System.Linq;
using trout.emailservice.model;

namespace trout.emailservice.queue.filters
{
    public class DequeueFilterList
    {
        private readonly IEmailQueueDbContext Context;

        public DequeueFilterList(IEmailQueueDbContext context)
        {
            Context = context;
        }

        private enum FilterOperand
        {
            And,
        }

        private Queue<KeyValuePair<FilterOperand, DequeueFilter>> Filters = new Queue<KeyValuePair<FilterOperand, DequeueFilter>>();

        public DequeueFilterList And(DequeueFilter filter)
        {
            Filters.Enqueue(new KeyValuePair<FilterOperand, DequeueFilter>(FilterOperand.And, filter));
            return this;
        }

        public IEnumerable<EmailQueueItem> Filter()
        {
            if(Filters.Count == 0)
            {
                this.And(new SentStatusDequeueFilter(false)).And(new RetriesFilter(5));
            }

            int length = Filters.Count;

            var query = Context.FetchEmailQueueItems().AsQueryable();

            while (length > 0)
            {
                var filter = Filters.Dequeue();

                if(filter.Key == FilterOperand.And)
                {
                    query = filter.Value.Filter(query);
                }
                
                length--;
            }

            return query.ToArray();
            
        }
    }
}