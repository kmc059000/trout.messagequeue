using System;
using trout.messagequeue.queue;
using trout.messagequeue.queue.filters;
using trout.messagequeue.queue.overrides;
using trout.messagequeueconsole.arguments;

namespace trout.messagequeueconsole.commands
{
    class SendCommand : Command
    {
        private DequeueFilterList filterList;
        private OverrideList overrideList;
        private bool Audit = true;

        private readonly MailMessageDequeuer Dequeuer;

        public SendCommand(MailMessageDequeuer dequeuer)
        {
            Dequeuer = dequeuer;
        }

        public override void Do(string[] args)
        {
            if(ParseArguments(args))
            {
                Dequeuer.SendQueuedMessages(filterList, overrideList, Audit);
            }
        }

        protected override bool ParseArguments(string[] args)
        {
            filterList = new DequeueFilterList();
            overrideList = new OverrideList();

            bool dateRangeApplied = false;
            DateTime dateFrom = DateTime.MinValue, dateTo = DateTime.MaxValue;

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

                //overrides
                .Add("to=|tooverride=", v=> overrideList.Add(new ToOverride().Override(v)))
                .Add("cc=|ccoverride=", v => overrideList.Add(new CcOverride().Override(v)))
                .Add("bcc=|bccoverride=", v => overrideList.Add(new BccOverride().Override(v)))
                .Add("subject=|subjectoverride=", v => overrideList.Add(new SubjectOverride().Override(v)))
                .Add("body=|bodyoverride=", v => overrideList.Add(new BodyOverride().Override(v)))

                .Add("toa=|toappend=", v => overrideList.Add(new ToOverride().Append(v)))
                .Add("cca=|ccappend=", v => overrideList.Add(new CcOverride().Append(v)))
                .Add("bcca=|bccappend=", v => overrideList.Add(new BccOverride().Append(v)))
                .Add("subjecta=|subjectappend=", v => overrideList.Add(new SubjectOverride().Append(v)))
                .Add("bodya=|bodyappend=", v => overrideList.Add(new BodyOverride().Append(v)))

                .Add("top=|toprepend=", v => overrideList.Add(new ToOverride().Prepend(v)))
                .Add("ccp=|ccprepend=", v => overrideList.Add(new CcOverride().Prepend(v)))
                .Add("bccp=|bccprepend=", v => overrideList.Add(new BccOverride().Prepend(v)))
                .Add("subjectp=|subjectprepend=", v => overrideList.Add(new SubjectOverride().Prepend(v)))
                .Add("bodyp=|bodyprepend=", v => overrideList.Add(new BodyOverride().Prepend(v)))

                .Add("da|donotaudit", "whether to audit whether the email was sent (default=true)" , v => Audit = false)
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

            return true;
        }
    }
}