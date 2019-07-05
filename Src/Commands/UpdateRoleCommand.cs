using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gravityzero.Console.Utility.Context;
using Gravityzero.Console.Utility.Infrastructure;
using Gravityzero.Console.Utility.Model;
using Gravityzero.Console.Utility.Tools;

namespace Gravityzero.Console.Utility.Commands
{
    public class UpdateRoleCommand : BaseCommand<RoleArgument>
    {
        // Brak w kontrolerze metody wywołującej update roli
        // Do uzyskania obiektu po nazwie użyłem RequestPost, a nie RequestGet

        public UpdateRoleCommand(IList<string> args) : base(args)
        {
        }

        protected override CommandResult Execute(ConsoleContext context, RoleArgument arguments)
        {
            if(string.IsNullOrEmpty(arguments.Rename) || string.IsNullOrEmpty(arguments.Name)){
                System.Console.WriteLine("Przy update wymagany parametr -n <NAZWA> -r <NAZWA>");
                return new CommandResult();
            }

            var role = WinApiConnector.RequestPost<string, Response<IEnumerable<Role>>>($"{context.ConsoleSettings.ServerAddress}:{context.ConsoleSettings.Port}/Api/Configuration/GetRole",arguments.Name);
                     
            if(!role.Result.Response.IsSuccess){
                return new CommandResult(role.Result.Message);
            }

            if(role == null){
                return new CommandResult("Zapytanie nie powiodło się");
            } 
            if(role.Result.Response.Payload.Count() != 1){
                return new CommandResult(role.Result.Response.Payload.Count() == 0 ? "Nie można zmienić roli, która nie istnieje" : "Wiecej niż jeden wynik");
            } 
            var existRole = role.Result.Response.Payload.FirstOrDefault();
                
            if(existRole == null){
                return new CommandResult("NULL");
            }
            var result = WinApiConnector.RequestPut<Role, Response<Role>>($"{context.ConsoleSettings.ServerAddress}:{context.ConsoleSettings.Port}/Api/Configuration/UpdateRole",
                    new Role(){Identifier = existRole.Identifier, Name = arguments.Rename});
            return new CommandResult(result.Result.IsSuccess ? "OK" : result.Result.Message);           
        }
    }
}