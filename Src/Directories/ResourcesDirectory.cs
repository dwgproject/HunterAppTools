using System;
using System.Collections.Generic;
using Gravityzero.Console.Utility.Commands;
using Gravityzero.Console.Utility.Context;
using Gravityzero.Console.Utility.Infrastructure;
using Gravityzero.Console.Utility.Model;

namespace Gravityzero.Console.Utility.Directories
{
    public class ResourcesDirectory : IDirectory, ICommand
    {
        public ResourcesDirectory()
        {
            Commands.Add("user",typeof(CreateCsvCommand<User>));
            Commands.Add("role", typeof(CreateCsvCommand<Role>));
            Commands.Add("hunting", typeof(CreateCsvCommand<Hunting>));
            Commands.Add("animal",typeof(CreateCsvCommand<Animal>));
        }
        public string Name => "Resources";

        public IDictionary<string, Type> Commands { get; private set;} = new Dictionary<string,Type>();

        public IDictionary<string, IDirectory> Directories { get; private set;} = new Dictionary<string,IDirectory>();

        public string Description { get; private set;} = "Komendy pobierające surowe dane do pliku";

        public CommandResult Execute(ConsoleContext context)
        {
            context.PushDirectory(this);
            return new CommandResult();
        }
    }
}