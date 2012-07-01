using trout.messagequeueconsole.commands;

namespace trout.messagequeueconsole
{
    class EmailServiceConsole
    {
        static void Main(string[] args)
        {
            Bootstrapper.Setup();
            new RootCommand().PresentRootCommandToUser(args);
        }
    }
}
