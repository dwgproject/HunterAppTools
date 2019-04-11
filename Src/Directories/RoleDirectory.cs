using System;
using System.Collections.Generic;
using HuntingAppSupport;
using HuntingAppSupport.Infrastructure;
using Src.Commands;

namespace HuntingAppSupport.Directories
{
    public class RoleDirectory : IDirectory, ICommand
    {
        public string Name => "Role";

        public IDictionary<string, Type> Commands {get; private set;}

        public IDictionary<string, IDirectory> Directories {get; private set;}

        public string Description {get; private set;} = "Wejscie do modułu roli";

        public RoleDirectory()
        {
            Commands = new Dictionary<string, Type>();
            Commands.Add("add",typeof(AddRoleCommand));
            Directories = new Dictionary<string, IDirectory>();
        }

        public CommandResult Execute(ContextApplication context)
        {
            context.PushDirectory(this);
            return new CommandResult();
        }
    }
}