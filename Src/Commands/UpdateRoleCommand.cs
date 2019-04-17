using System.Collections.Generic;
using System.Linq;
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
            var role = WinApiConnector.RequestPost<string, Response<IEnumerable<Role>>>("http://localhost:5000/Api/Configuration/",arguments.Name);
            if(role.Result.Result.IsSuccess){
                var changeRole = new Role(){Identifier=role.Result.Result.Payload.FirstOrDefault().Identifier, Name = arguments.Rename};
                var result = WinApiConnector.RequestPost<Role,Response<Role>>("http://localhost:5000/Api/Configuration/",changeRole);
                return new CommandResult(result.Result.IsSuccess ? "OK" : result.Result.Message);
            }
            return new CommandResult();
        }
    }
}