using System;
using trout.messagequeue.attachments;
using trout.messagequeue.queue;
using trout.messagequeue.queue.filters;
using trout.messagequeue.queue.overrides;
using trout.messagequeueconsole.arguments;
using System.Linq;

namespace trout.messagequeueconsole.commands
{
    class AttachmentsCommand : Command
    {
        private DequeueFilterList filterList;

        private readonly MailMessageDequeuer Dequeuer;
        private readonly IAttachmentFileSystem AttachmentFileSystem;

        private bool Purge = false;

        public AttachmentsCommand(MailMessageDequeuer dequeuer, IAttachmentFileSystem attachmentFileSystem)
        {
            Dequeuer = dequeuer;
            AttachmentFileSystem = attachmentFileSystem;
        }

        public override void Do(string[] args)
        {
            if(ParseArguments(args))
            {
                if(Purge)
                {
                    AttachmentFileSystem.PurgeAttachments(Dequeuer.GetQueuedMessages(filterList, new OverrideList()).Select(e => e.EmailQueueItem));
                }
            }
        }

        protected override bool ParseArguments(string[] args)
        {
            filterList = new DequeueFilterList();
            bool dateRangeApplied = false;
            DateTime dateFrom = DateTime.MinValue, dateTo = DateTime.MaxValue;
            bool idRangeApplied = false;
            int idMin = 0, idMax = int.MaxValue;

            OptionSet optionSet = new OptionSet()
                //filters
                .Add("tf=|tofilter=", v => filterList.And(new ToFilter(v)))
                .Add("bcf=|bodycontainsfilter=", v => filterList.And(new BodyContainsFilter(v)))
                .Add("bef=|bodyexactfilter=", v => filterList.And(new BodyExactFilter(v)))
                .Add("fd=|fromdate=", (DateTime v) => { dateRangeApplied = true; dateFrom = v; })
                .Add("td=|todate=", (DateTime v) => { dateRangeApplied = true; dateTo = v; })
                .Add("idf=|idfilter=|id=", (int v) => filterList.And(new IdDequeueFilter(v)))
                .Add("rtf=|retriesfilter=", (byte v) => filterList.And(new RetriesFilter(v)))
                .Add("sent=|sentfilter=|issent=", (bool v) => filterList.And(new SentStatusDequeueFilter(v)))
                .Add("scf=|subjectcontainsfilter=", v => filterList.And(new SubjectContainsFilter(v)))
                .Add("sef=|subjectexactfilter=", v => filterList.And(new SubjectExactFilter(v)))
                .Add("idrf=|idfrom=", (int v) => { idRangeApplied = true; idMin = v; })
                .Add("idrt=|idto=", (int v) => { idRangeApplied = true; idMax = v; })
                .Add("haf|hasattachmentsfilter", "filter on whether an email has attachments", v => filterList.And(new HasAttachmentsFilter(true)))

                .Add("p|purge", "purges attachments from storage (default=false)" , v => Purge = true)
                ;

            try
            {
                optionSet.Parse(args);
            }
            catch (OptionException)
            {
                Console.WriteLine("Error");
                return false;
            }

            if (dateRangeApplied)
            {
                filterList.And(new DateRangeFilter(dateFrom, dateTo));
            }

            if (idRangeApplied)
            {
                filterList.And(new IdRangeDequeueFilter(idMin, idMax));
            }

            return true;
        }
    }
}