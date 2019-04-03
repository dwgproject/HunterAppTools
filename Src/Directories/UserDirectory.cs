using System;
using System.Collections.Generic;
using HuntingAppSupport.Commands;
using HuntingAppSupport.Infrastructure;

namespace HuntingAppSupport.Directories{

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
            Directories = new Dictionary<string, IDirectory>();
        }

        public CommandResult Execute(ContextApplication context)
        {
            context.PushDirectory(this);
            return new CommandResult();
        }
    }
}