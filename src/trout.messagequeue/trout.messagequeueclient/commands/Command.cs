using NDesk.Options;

namespace trout.emailserviceclient.commands
{
    abstract class Command
    {
        protected OptionSet OptionSet { get; private set; }

        public abstract void Do(string[] args);
        protected abstract void ParseArguments(string[] args);
    }
}