using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NDesk.Options;
using trout.emailservice.config;
using trout.emailservice.infrastrucure;
using trout.emailservice.infrastrucure.dependencies;
using trout.emailservice.model;
using trout.emailservice.queue;
using trout.emailservice.queue.filters;
using trout.emailservice.queue.overrides;

namespace trout.emailserviceclient.commands
{
    class ListCommand : Command
    {
        private static DequeueFilterList filterList;
        private static OverrideList overrideList;

        int skip = 0;

        public override void Do(string[] args)
        {
            ParseArguments(args);

            var sender = DependencyResolver.Resolve<MailMessageDequeuer>();

            var results = sender.GetQueuedMessages(filterList, overrideList).ToList();

            

            WriteSummary(results);

            while(true) if (!DoDialog(results)) return;
        }

        private bool DoDialog(List<DequeueListItem> results)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(string.Format("1 - print all simple [count=5 skip={0}]", skip));
            sb.AppendLine(string.Format("2 - print all extended [count=5 skip={0}]", skip));
            sb.AppendLine("3 - return to beginning");
            sb.AppendLine("4 - go to end");
            sb.AppendLine("5 - cancel");

            Console.WriteLine(sb.ToString());
            Console.Write(">> ");
            string[] input = Console.ReadLine().Split(' ');

            int command, count = 5, skipEntered = 0;
            bool commandSuccess = false;

            if (input.Length == 0 || input[0] == "") return true;
            if(input.Length > 0 && int.TryParse(input[0], out command))
            {
                if(input.Length > 1) int.TryParse(input[1], out count);
                bool skipIncluded = input.Length > 2 && int.TryParse(input[2], out skipEntered);


                if (skipIncluded) skip += skipEntered;

                switch (command)
                {
                    case 1:
                        PrintSimple(results, count, skip);
                        commandSuccess = true;
                        skip += count;
                        break;
                    case 2:
                        PrintExtended(results, count, skip);
                        commandSuccess = true;
                        skip += count;
                        break;
                    case 3:
                        skip = 0;
                        commandSuccess = true;
                        break;
                    case 4:
                        skip = results.Count;
                        commandSuccess = true;
                        break;
                    case 5:
                        return false;
                        break;
                    default:
                        commandSuccess = false;
                        break;
                }
            }

            if(!commandSuccess) Console.WriteLine("Invalid command entered");

            return true;
        }
        private const string header = "==========================";
        private const string footer = "==========================";

        private void PrintSimple(List<DequeueListItem> items, int count, int s)
        {
            foreach (var item in items.Skip(s).Take(count))
            {
                StringBuilder sb = new StringBuilder();

                sb.AppendLine(header);
                sb.AppendLine(string.Format("ID: {0}, IsSent: {1}, Tries: {2}", item.EmailQueueItem.ID, item.EmailQueueItem.IsSent, item.EmailQueueItem.NumberTries));
                sb.AppendLine(string.Format("To: {0}", item.Message.To));
                sb.AppendLine(string.Format("Subject: {0}", item.Message.Subject));
                sb.AppendLine(footer);

                Console.WriteLine(sb.ToString());
            }
        }

        private void PrintExtended(List<DequeueListItem> items, int count, int s)
        {
            foreach (var item in items.Skip(s).Take(count))
            {
                StringBuilder sb = new StringBuilder();

                sb.AppendLine(header);
                sb.AppendLine(string.Format("ID: {0}, IsSent: {1}, Tries: {2}", item.EmailQueueItem.ID, item.EmailQueueItem.IsSent, item.EmailQueueItem.NumberTries));
                sb.AppendLine(string.Format("To: {0}", item.Message.To));
                sb.AppendLine(string.Format("CC: {0}", item.Message.CC));
                sb.AppendLine(string.Format("Bcc: {0}", item.Message.Bcc));
                sb.AppendLine(string.Format("Subject: {0}", item.Message.Subject));
                sb.AppendLine(string.Format("Body: {0}", item.Message.Body));
                sb.AppendLine(footer);

                Console.WriteLine(sb.ToString());
            }
        }

        private void WriteSummary(List<DequeueListItem> results)
        {
            Console.WriteLine(string.Format("Result count: {0}", results.Count()));
        }

        protected override void ParseArguments(string[] args)
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