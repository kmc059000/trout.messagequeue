using System.Collections.Generic;
using System.Linq;
using trout.messagequeue.model;

namespace trout.messagequeue.queue.filters
{
    public class DequeueFilterList
    {
        private enum FilterOperand
        {
            And,
        }

        private readonly Queue<KeyValuePair<FilterOperand, DequeueFilter>> Filters = new Queue<KeyValuePair<FilterOperand, DequeueFilter>>();

        public DequeueFilterList()
        {
            
        }

        /// <summary>
        /// Creates a new FilterList with a initial set of filters
        /// </summary>
        /// <param name="filterItems"></param>
        private DequeueFilterList(Queue<KeyValuePair<FilterOperand, DequeueFilter>> filterItems)
        {
            Filters = filterItems;
        }


        public DequeueFilterList And(DequeueFilter filter)
        {
            Filters.Enqueue(new KeyValuePair<FilterOperand, DequeueFilter>(FilterOperand.And, filter));
            return this;
        }

        public IEnumerable<EmailQueueItem> Filter(IEmailQueueDbContext context)
        {
            if(Filters.Count == 0)
            {
                this.And(new SentStatusDequeueFilter(false)).And(new RetriesFilter(5));
            }

            int length = Filters.Count;

            var query = context.EmailQueueItemRepo.Fetch();

            while (length > 0)
            {
                var filter = Filters.Dequeue();

                if(filter.Key == FilterOperand.And)
                {
                    query = filter.Value.Filter(query);
                }
                
                length--;

                //add filter back at the end of the queue so we can use this FilterList over and over. 
                //Essentially we iterate in place except that it isn't in place.
                //Should always be an O(1) operation because I don't think the capacity of the queue ever decreases. 
                //If O(1), who cares about this then, this function is bounded by O(n)
                Filters.Enqueue(filter);
            }

            return query.ToArray();
            
        }

        public DequeueFilterList Clone()
        {
            return new DequeueFilterList(this.Filters);
        }
    }
}