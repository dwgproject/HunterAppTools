using System.Collections.Generic;
using Gravityzero.Console.Utility.Context;
using Gravityzero.Console.Utility.Infrastructure;
using Gravityzero.Console.Utility.Model;
using Gravityzero.Console.Utility.Tools;

namespace Gravityzero.Console.Utility.Commands
{
    public class GetUsersCommand : ICommand
    {
        public string Description => "Zwracanie użytkowników";

        public CommandResult Execute(ConsoleContext context)
        {
            var result = WinApiConnector.RequestGet<string,Response<IEnumerable<User>>>($"{context.ConsoleSettings.ServerAddress}:{context.ConsoleSettings.Port}/Api/User/GetUser","");
            foreach(var user in result.Result.Result.Payload){
                System.Console.WriteLine($"{user.Login} - {user.Role.Name}");
            }
            return new CommandResult(result.Result.IsSuccess ? "OK" : result.Result.Message);
        }
    }
}