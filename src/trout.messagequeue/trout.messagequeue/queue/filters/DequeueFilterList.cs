using System.Collections.Generic;
using System.Linq;
using trout.messagequeue.model;
using trout.messagequeue.model.repository;

namespace trout.messagequeue.queue.filters
{
    /// <summary>
    /// List of Dequeue Filters which should be applied
    /// </summary>
    public sealed class DequeueFilterList
    {
        private enum FilterOperand
        {
            And,
        }

        private readonly Queue<KeyValuePair<FilterOperand, DequeueFilter>> Filters = new Queue<KeyValuePair<FilterOperand, DequeueFilter>>();

        /// <summary>
        /// Constructor
        /// </summary>
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


        /// <summary>
        /// Adds a new filter which is the union of all filters than have been added
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public DequeueFilterList And(DequeueFilter filter)
        {
            Filters.Enqueue(new KeyValuePair<FilterOperand, DequeueFilter>(FilterOperand.And, filter));
            return this;
        }

        /// <summary>
        /// returns EmailQueueItems from the provided context which are belong in the union of all filter cases
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <returns></returns>
        public IEnumerable<EmailQueueItem> Filter(IRepository<EmailQueueItem> repository)
        {
            if(Filters.Count == 0)
            {
                this.And(new SentStatusDequeueFilter(false)).And(new RetriesFilter(5));
            }

            int length = Filters.Count;

            var query = repository.Fetch();

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

        /// <summary>
        /// Returns a new instance of a DequeueFilterList which has the same filters as this item. It does not create new instances of the filters.
        /// </summary>
        /// <returns></returns>
        public DequeueFilterList Clone()
        {
            return new DequeueFilterList(this.Filters);
        }
    }
}