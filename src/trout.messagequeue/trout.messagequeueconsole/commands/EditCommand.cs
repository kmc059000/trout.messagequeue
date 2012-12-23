using System;
using System.IO;
using System.Text;
using trout.messagequeue.infrastrucure.logging;
using trout.messagequeue.model;
using trout.messagequeue.model.repository;
using trout.messagequeueconsole.arguments;

namespace trout.messagequeueconsole.commands
{
    class EditCommand : Command
    {
        private int EmailID;
        private EmailQueueItem Email;
        private readonly IRepository<EmailQueueItem> Repository;

        public EditCommand(IRepository<EmailQueueItem> repository)
        {
            Repository = repository;
        }

        public override void Do(string[] args)
        {
            if (ParseArguments(args))
            {
                EditFile();
            }
        }

        protected override bool ParseArguments(string[] args)
        {
            OptionSet optionSet = new OptionSet().Add("id=", (int v) => EmailID = v);

            try
            {
                optionSet.Parse(args);
            }
            catch (OptionException)
            {
                Console.WriteLine("Error");
                return false;
            }
            
            Email = Repository.First(e => e.ID == EmailID);

            return Email != null;
        }

        private void EditFile()
        {
            WriteFile();

            bool save = false;

            while(!save)
            {
                Console.WriteLine("  Options:\n  save - save contents of file to database\n  cancel - cancel edit");
                Console.Write(">>");
                var command = Console.ReadLine().Trim();

                TroutLog.Log.Info(string.Format("Executing edit command: {0}", command));

                switch (command)
                {
                    case "save":
                        save = true;
                        break;
                    case "cancel":
                        DeleteFile();
                        return;
                    default:
                        Console.WriteLine("Invalid Command.\nsave - save contents of file to database\ncancel - cancel edit");
                        break;
                }
            }

            ReadFile();

        }

        private string Filename
        {
            get { return EmailID + ".txt"; }
        }

        private void WriteFile()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("#To");
            sb.AppendLine(Email.To);

            sb.AppendLine("#Cc");
            sb.AppendLine(Email.Cc);

            sb.AppendLine("#Bcc");
            sb.AppendLine(Email.Bcc);

            sb.AppendLine("#Subject");
            sb.AppendLine(Email.Subject);

            sb.AppendLine("#Body");
            sb.AppendLine(Email.Body);

            File.WriteAllText(Filename, sb.ToString());

            System.Diagnostics.Process.Start(Filename);
        }

        private void ReadFile()
        {
            var contents = File.ReadAllText(Filename);

            int to = contents.IndexOf("#To\r\n");
            int cc = contents.IndexOf("#Cc\r\n");
            int bcc = contents.IndexOf("#Bcc\r\n");
            int subject = contents.IndexOf("#Subject\r\n");
            int body = contents.IndexOf("#Body\r\n");

            Email.To = contents.Substring(to + 5, cc - to - 7);
            Email.Cc = contents.Substring(cc + 5, bcc - cc - 7);
            Email.Bcc = contents.Substring(bcc + 6, subject - bcc - 8);
            Email.Subject = contents.Substring(subject + 10, body - subject - 12);
            Email.Body = contents.Substring(body + 7, contents.Length - body - 9);

            DeleteFile();

            Repository.SaveChanges();
        }

        private void DeleteFile()
        {
            File.Delete(Filename);
        }
    }
}