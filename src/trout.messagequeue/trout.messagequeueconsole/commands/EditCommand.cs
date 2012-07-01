using System;
using System.IO;
using System.Text;
using trout.messagequeue.model;
using trout.messagequeueconsole.arguments;

namespace trout.messagequeueconsole.commands
{
    class EditCommand : Command
    {
        private int EmailID;
        private EmailQueueItem Email;
        private readonly IEmailQueueDbContext Repository;

        public EditCommand(IEmailQueueDbContext repository)
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
            
            Email = Repository.EmailQueueItemRepo.First(e => e.ID == EmailID);

            return Email != null;
        }

        private void EditFile()
        {
            WriteFile();

            //wait until they finish?
            bool save = false;

            while(!save)
            {
                Console.WriteLine("  Options:\n  save - save contents of file to database\n  cancel - cancel edit");
                Console.Write(">>");
                var command = Console.ReadLine().Trim();

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
            bool toProcessed = false,
                 startTo = false,
                 ccProcessed = false,
                 startCc = false,
                 bccProcessed = false,
                 startBcc = false,
                 subjectProcessed = false,
                 startSubject = false,
                 startBody = false;



            var contents = File.ReadAllLines(Filename);
            var joinedString = "";

            for (int i = 0; i < contents.Length; i++)
            {
                var line = contents[i];

                if (!toProcessed)
                {
                    if (!startTo)
                    {
                        if (line.StartsWith("#To"))
                        {
                            startTo = true;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        if (line.StartsWith("#Cc"))
                        {
                            Email.To = joinedString;
                            joinedString = "";
                            toProcessed = true;
                            i--;
                        }
                        else
                        {
                            joinedString += line;
                        }
                    }
                }
                else if (!ccProcessed)
                {

                    if (!startCc)
                    {
                        if (line.StartsWith("#Cc"))
                        {
                            startCc = true;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        if (line.StartsWith("#Bcc"))
                        {
                            Email.Cc = joinedString;
                            joinedString = "";
                            ccProcessed = true;
                            i--;
                        }
                        else
                        {
                            joinedString += line;
                        }
                    }
                }
                else if (!bccProcessed)
                {

                    if (!startBcc)
                    {
                        if (line.StartsWith("#Bcc"))
                        {
                            startBcc = true;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        if (line.StartsWith("#Subject"))
                        {
                            Email.Bcc = joinedString;
                            joinedString = "";
                            bccProcessed = true;
                            i--;
                        }
                        else
                        {
                            joinedString += line;
                        }
                    }
                }
                else if (!subjectProcessed)
                {
                    if (!startSubject)
                    {
                        if (line.StartsWith("#Body"))
                        {
                            startSubject = true;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        if (line.StartsWith("#Body"))
                        {
                            Email.Subject = joinedString.Substring(0, joinedString.Length - 1);
                            joinedString = "";
                            subjectProcessed = true;
                            i--;
                        }
                        else
                        {
                            joinedString += line;
                            joinedString += "\n";
                        }
                    }
                }
                else
                {
                    if (!startBody)
                    {
                        if (line.StartsWith("#Body"))
                        {
                            startBody = true;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        joinedString += line;
                        joinedString += "\n";
                    }
                }
            }

            Email.Body = joinedString.Substring(0, joinedString.Length-1);

            DeleteFile();

            Repository.SaveChanges();
        }

        private void DeleteFile()
        {
            File.Delete(Filename);
        }
    }
}