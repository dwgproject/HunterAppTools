using System;
using System.Collections.Generic;
using Gravityzero.Console.Utility.Commands;
using Gravityzero.Console.Utility.Context;
using Gravityzero.Console.Utility.Infrastructure;

namespace Gravityzero.Console.Utility.Directories
{
    public class ConfigurationDirectory : IDirectory, ICommand
    {
        public ConfigurationDirectory()
        {
            Commands.Add("addrole", typeof(AddRoleCommand));
        }

        public string Name { get; private set;} = "Configuration";

        public IDictionary<string, Type> Commands { get; private set;} = new Dictionary<string, Type>();

        public IDictionary<string, IDirectory> Directories { get; private set;} = new Dictionary<string, IDirectory>();

        public string Description { get; private set;} = "Udostępnia komendy konfigurujące system.";

        public CommandResult Execute(ConsoleContext context)
        {
            context.PushDirectory(this);
            return new CommandResult();
        }
    }


}