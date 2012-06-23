using System;

namespace trout.emailserviceclient
{
    class Command
    {
        public string Name { get; private set; }
        public Action Action { get; private set; }

        public Command(string name, Action action)
        {
            Name = name;
            Action = action;
        }

        public void Do()
        {
            Action.Invoke();
        }
    }
}