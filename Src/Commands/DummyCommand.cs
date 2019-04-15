using Gravityzero.Console.Utility.Context;
using Gravityzero.Console.Utility.Infrastructure;

namespace Gravityzero.Console.Utility.Commands
{
    public class DummyCommand : ICommand
    {
        public string Description { get; set; }

        public CommandResult Execute(ConsoleContext context)
        {
            return new CommandResult("Unknown command.");
        }
    }
}