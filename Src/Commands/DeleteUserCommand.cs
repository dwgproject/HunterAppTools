using System;
using System.Collections.Generic;
using Gravityzero.Console.Utility.Context;
using Gravityzero.Console.Utility.Infrastructure;
using Gravityzero.Console.Utility.Tools;
using Gravityzero.Console.Utility.Model;
using System.Linq;

namespace Gravityzero.Console.Utility.Commands
{
    public class DeleteUserCommand : ICommand
    {
        // w kontrolerze User przy request delete nazwa parametru musi być id
        // w kontrolerze User trzeba zaimplementować metodę Query z repozytorium
        public string Description => "Komenda usunięcia użytkownika";

        public CommandResult Execute(ConsoleContext context)
        {
            System.Console.WriteLine("Procedura usunięcia użytkownika. Postępuj zgdnie z instrukcją");
            System.Console.Write("Login użytkownika: ");
            var login = System.Console.ReadLine();

            System.Console.Write("Hasło użytkownika: ");
            var password = System.Console.ReadLine();
            var findUser = WinApiConnector.RequestPost<User,Response<IEnumerable<User>>>($"{context.ConsoleSettings.ServerAddress}:{context.ConsoleSettings.Port}/Api/User/GetUser",
                            new User(){Login = login, Password = password});
            if(!findUser.Result.IsSuccess){
                return new CommandResult();
            }
            var result =  WinApiConnector.RequestDelete<string, Response<string>>($"{context.ConsoleSettings.ServerAddress}:{context.ConsoleSettings.Port}/Api/User/Delete"+
                            $"/{findUser.Result.Result.Payload.FirstOrDefault().Identifier}");
            return new CommandResult(result.Result.IsSuccess ? "Ok" : "Nie");
        }
    }
}