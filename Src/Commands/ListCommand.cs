using System.Collections.Generic;
using System.Linq;
using CommandLine;
using Gravityzero.Console.Utility.Context;
using Gravityzero.Console.Utility.Directories;
using Gravityzero.Console.Utility.Infrastructure;

namespace Gravityzero.Console.Utility.Commands
{
    public class ListCommand : BaseCommand<ListArguments>
    {
        public ListCommand(IList<string> args) : base(args)
        {
        }
        public ListCommand() : base(new List<string>())
        {
            Description = "Wyświetla listę komend w ramach danego modułu.";
        }

        protected override CommandResult Execute(ConsoleContext context, ListArguments arguments)
        {
            IDirectory current = context.GetDirectory();
            IList<string> commands = new List<string>();
            if (current is RootDirectory)
            {
                foreach(string command in context.GetGeneralCommands())
                    commands.Add(command);
                foreach(string command in current.Commands.Keys)
                    commands.Add(command);
                commands = commands.OrderBy(ix => ix).ToList();
            }
            else
            {
                commands = current.Commands.Keys.OrderBy(ix => ix).ToList();;
            }

            System.Console.WriteLine("Command list:");
            System.Console.WriteLine(string.Empty);
            foreach (string name in commands)
                System.Console.WriteLine($"\t{name}");

            System.Console.WriteLine(string.Empty);
            return new CommandResult();
        }
    }

    public class ListArguments{
     
    }

}