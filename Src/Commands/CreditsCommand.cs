using Gravityzero.Console.Utility.Context;
using Gravityzero.Console.Utility.Infrastructure;

namespace Gravityzero.Console.Utility.Commands
{
    public class CreditsCommand : ICommand
    {
        public string Description => "Wyświetla twórców programu.";

        public CommandResult Execute(ConsoleContext context)
        {
            System.Console.WriteLine("Created by: Garion, Api, Majami. 2019");
            return new CommandResult();
        }
    }
}