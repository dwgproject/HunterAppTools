using System.Collections.Generic;
using System.Linq;
using Gravityzero.Console.Utility.Context;
using Gravityzero.Console.Utility.Infrastructure;
using Gravityzero.Console.Utility.Model;
using Gravityzero.Console.Utility.Tools;

namespace Gravityzero.Console.Utility.Commands
{
    public class DeleteHuntingCommand : ICommand
    {
        public string Description => "Usunięcie polowania";

        public CommandResult Execute(ConsoleContext context)
        {
            return new CommandResult();
        }
    }
}