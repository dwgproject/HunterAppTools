using System;
using System.Collections.Generic;
using Gravityzero.Console.Utility.Commands;
using Gravityzero.Console.Utility.Context;
using Gravityzero.Console.Utility.Infrastructure;

namespace Gravityzero.Console.Utility.Directories
{
    public class HuntingDirectory : IDirectory, ICommand
    {
        public string Name => "Hunting";

        public IDictionary<string, Type> Commands {get;private set;}

        public IDictionary<string, IDirectory> Directories {get; private set;}

        public string Description {get;set;}="Wejście do modułu polowania";

        public HuntingDirectory()
        {
            Commands = new Dictionary<string,Type>();
            Commands.Add("addhunting",typeof(AddHuntingCommand));
            Directories= new Dictionary<string, IDirectory>();
        }

        public CommandResult Execute(ConsoleContext context)
        {
            context.PushDirectory(this);
            return new CommandResult();
        }
    }


}