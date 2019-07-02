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
            if(string.IsNullOrEmpty(arguments.Rename)){
                System.Console.WriteLine("Przy update wymagany parametr -r <NAZWA>");
                return new CommandResult();
            }
            var role = WinApiConnector.RequestPost<string, Response<IEnumerable<Role>>>($"{context.ConsoleSettings.ServerAddress}:{context.ConsoleSettings.Port}/Api/Configuration/GetRole",arguments.Name);
            if (role == null)             
                return new CommandResult("RoleResult is null.");
            if (!role.Result.IsSuccess)             
                return new CommandResult("Task result is unsuccesful.");
            if(!role.Result.Result.IsSuccess)
                return new CommandResult(role.Result.Message);

            Role rowRole = role.Result.Result.Payload.FirstOrDefault();
            if (rowRole == null)
                return new CommandResult("There are no roles.");

            var result = WinApiConnector.RequestPut<string, Response<IEnumerable<Role>>>($"{context.ConsoleSettings.ServerAddress}:{context.ConsoleSettings.Port}/Api/Configuration/UpdateRole/{rowRole.Identifier}", arguments.Rename);
            
            if (role == null)             
                return new CommandResult("RoleResult after update is null.");
            if (!role.Result.IsSuccess)             
                return new CommandResult("Task result after update is unsuccesful.");
            if(!role.Result.Result.IsSuccess)
                return new CommandResult(role.Result.Message);          
            
            return new CommandResult(result.Result.IsSuccess ? "OK" : result.Result.Message);
        }
    }
}