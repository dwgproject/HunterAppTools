using System;
using System.Collections.Generic;
using Gravityzero.Console.Utility.Commands;
using Gravityzero.Console.Utility.Context;
using Gravityzero.Console.Utility.Infrastructure;

namespace Gravityzero.Console.Utility.Directories
{

    public class UserDirectory : IDirectory, ICommand
    {
        public string Name => "User";

        public string Description { get; set ; } = "Wejście do modułu usera.";

        public IDictionary<string, Type> Commands { get; private set; }

        public IDictionary<string, IDirectory> Directories { get; private set; }

        public UserDirectory()
        {
            Commands = new Dictionary<string, Type>();
            Commands.Add("add", typeof(AddUserCommand));
            Commands.Add("addusers", typeof(AddUsersCommand));
            Commands.Add("delete", typeof(DeleteUserCommand));
            Commands.Add("get", typeof(GetUsersCommand));
            Commands.Add("update", typeof(UpdateUserCommand));
            Directories = new Dictionary<string, IDirectory>();
        }

        public CommandResult Execute(ConsoleContext context)
        {
            context.PushDirectory(this);
            return new CommandResult();
        }
    }
}