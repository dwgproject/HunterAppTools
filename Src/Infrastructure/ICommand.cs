using Gravityzero.Console.Utility.Context;

namespace Gravityzero.Console.Utility.Infrastructure
{
    public interface ICommand{
        string Description {get;}
        CommandResult Execute(ConsoleContext context);
    }
}