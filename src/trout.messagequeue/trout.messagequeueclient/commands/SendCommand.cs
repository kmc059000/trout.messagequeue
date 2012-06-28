using System;
using NDesk.Options;
using trout.emailservice.config;
using trout.emailservice.infrastrucure;
using trout.emailservice.model;
using trout.emailservice.queue;
using trout.emailservice.queue.filters;

namespace trout.emailserviceclient.commands
{
    class SendCommand : Command
    {
        private static DequeueFilterList filterList;

        public override void Do(string[] args)
        {
            ParseArguments(args);

            var sender = new MailMessageDequeuer(new MailMessageSenderConfig(), new SmtpClient(), new EmailQueueDbContext());

            sender.SendQueuedMessages(filterList);
        }

        protected override void ParseArguments(string[] args)
        {
            filterList = new DequeueFilterList();

            bool dateRangeApplied = false;
            DateTime dateFrom = DateTime.MinValue, dateTo = DateTime.MaxValue;

            OptionSet optionSet = new OptionSet()
                .Add("tf=|tofilter=", v => filterList.And(new ToFilter(v)))
                .Add("bcf=|bodycontainsfilter=", v => filterList.And(new BodyContainsFilter(v)))
                .Add("bef=|bodyexactfilter=", v => filterList.And(new BodyExactFilter(v)))
                .Add("fd=|fromdate=", (DateTime v) => { dateRangeApplied = true; dateFrom = v; })
                .Add("td=|todate=", (DateTime v) => { dateRangeApplied = true; dateTo = v; })
                .Add("idf=|idfilter=|id=", (int v) => filterList.And(new IdDequeueFilter(v)))
                .Add("rtf=|retriesfilter=", (byte v) => filterList.And(new RetriesFilter(v)))
                .Add("sent=|sentfilter=", (bool v) => filterList.And(new SentStatusDequeueFilter(v)))
                .Add("scf=|subjectcontainsfilter=", v => filterList.And(new SubjectContainsFilter(v)))
                .Add("sef=|subjectexactfilter=", v => filterList.And(new SubjectExactFilter(v)))
                ;

            try
            {
                optionSet.Parse(args);
            }
            catch (OptionException)
            {
                Console.WriteLine("Error, usage is:", optionSet);
            }

            if (dateRangeApplied)
            {
                filterList.And(new DateRangeFilter(dateFrom, dateTo));
            }
        }
    }
}