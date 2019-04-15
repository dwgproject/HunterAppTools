using Gravityzero.Console.Utility.Context;
using Gravityzero.Console.Utility.Infrastructure;

namespace Gravityzero.Console.Utility.Commands
{
    public class ExitCommand : ICommand
    {
        public ExitCommand()
        {
            
        }

        public string Description { get ; set; } = "Komenda zamyka aplikację.";

        public CommandResult Execute(ConsoleContext context)
        {
            context.ShouldWork = false;
            return new CommandResult();
        }
    }
}