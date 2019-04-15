using Gravityzero.Console.Utility.Context;
using Gravityzero.Console.Utility.Infrastructure;

namespace Gravityzero.Console.Utility.Commands
{

    public class BackCommand : ICommand
    {
        public string Description { get; set; }

        public CommandResult Execute(ConsoleContext context)
        {
            context.PopDirectory();
            return new CommandResult();
        }
    }


}