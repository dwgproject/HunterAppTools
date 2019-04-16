using System.Collections.Generic;
using CommandLine;
using Gravityzero.Console.Utility.Context;
using Gravityzero.Console.Utility.Infrastructure;

namespace Gravityzero.Console.Utility.Commands
{
    public class HelpCommand : BaseCommand<HelpArguments>
    {
        public HelpCommand(IList<string> args) : base(args)
        {
        }
        public HelpCommand() : base(new List<string>())
        {
        }

        protected override CommandResult Execute(ConsoleContext context, HelpArguments arguments)
        {
            System.Console.WriteLine();
            if (string.IsNullOrEmpty(arguments.CommandName))
            {
                System.Console.WriteLine($"\tNarzędzie wspomagające testowanie i konfigurowanie usługi wspomagania polowania.");
                System.Console.WriteLine($"\tWpisz nazwę modułu który chcesz testować lub konfigurować (user, configuration),");
                System.Console.WriteLine($"\ta następnie użyj polecenia list, aby zapoznać się z listą komend danego modułu.");
            }
            else
            {
                
            }
            System.Console.WriteLine();
            return new CommandResult();
        }
    }

    public class HelpArguments
    {
        [Option('c', "command", Required = false, HelpText = "Nazwa komendy.")]
        public string CommandName { get; set; }
    }
}