using System.Collections.Generic;
using HuntingAppSupport.Commands;
using HuntingAppSupport.Infrastructure;

namespace HuntingAppSupport.Directories{

    public class UserDirectory : IDirectory, ICommand
    {
        public string Name => "User";

        public string Description { get; set ; } = "Wejście do modułu usera.";

        public IDictionary<string, ICommand> Commands { get; private set; }

        public IDictionary<string, IDirectory> Directories { get; private set; }

        public UserDirectory()
        {
            Commands = new Dictionary<string, ICommand>();
            Commands.Add("add", new AddUserCommand());
            Directories = new Dictionary<string, IDirectory>();
        }

        public CommandResult Execute(ContextApplication context)
        {
            context.PushDirectory(this);
            return new CommandResult();
        }
    }
}