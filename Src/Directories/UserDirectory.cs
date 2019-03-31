using System.Collections.Generic;
using HuntingAppSupport.Infrastructure;

namespace HuntingAppSupport.Directories{

    public class UserDirectory : IDirectory, ICommand
    {
        public string Name => "User";

        public IEnumerable<ICommand> Commands { get; private set; }

        public IEnumerable<IDirectory> Directories { get; private set; }
        public string Description { get; set ; } = "Wejście do modułu usera.";

        public UserDirectory()
        {
            Commands = new List<ICommand>();
            Directories = new List<IDirectory>();

        }

        public CommandResult Execute(ContextApplication context)
        {
            context.PushDirectory(this);
            return new CommandResult();
        }
    }
}