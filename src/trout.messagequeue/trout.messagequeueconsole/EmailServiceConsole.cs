using trout.emailserviceconsole.commands;

namespace trout.emailserviceconsole
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
