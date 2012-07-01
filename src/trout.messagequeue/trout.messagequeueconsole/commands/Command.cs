using trout.messagequeueconsole.arguments;

namespace trout.messagequeueconsole.commands
{
    abstract class Command
    {
        protected OptionSet OptionSet { get; private set; }

        public abstract void Do(string[] args);
        protected abstract void ParseArguments(string[] args);
    }
}