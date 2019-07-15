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

            Task<ConnectorResult<Response<IEnumerable<Role>>>> role = WinApiConnector.RequestGet<Response<IEnumerable<Role>>>($"{context.ConsoleSettings.ServerAddress}:{context.ConsoleSettings.Port}/Api/Configuration/GetRole/"+
                                $"{arguments.Name.ToLower()}");
            ConnectorResult<Response<IEnumerable<Role>>> preResult = role.Result;
            if(!preResult.IsSuccess)
                return new CommandResult(preResult.Message, false);
            if(!preResult.Response.IsSuccess)
                return new CommandResult(preResult.Response.Code, false);
            if(!preResult.Response.Payload.Any())
                return new CommandResult($"The Role {arguments.Name} doesn't exist in database", false);

            if(preResult.Response.Payload.Count() != 1){
                return new CommandResult(preResult.Response.Payload.Count() == 0 ? "Nie można zmienić roli, która nie istnieje" : "Wiecej niż jeden wynik", false);
            } 
                
            Task<ConnectorResult<Response<string>>> result = WinApiConnector.RequestPut<Role, Response<string>>($"{context.ConsoleSettings.ServerAddress}:{context.ConsoleSettings.Port}/Api/Configuration/UpdateRole",
                    new Role()
                    {
                        Identifier = preResult.Response.Payload.FirstOrDefault().Identifier, 
                        Name = arguments.Rename.ToLower()
                    });
            ConnectorResult<Response<string>> connectorResult = result.Result;
            if(!connectorResult.IsSuccess)
                return new CommandResult(connectorResult.Message, false);
            if(!connectorResult.Response.IsSuccess)
                return new CommandResult(connectorResult.Response.Code, false);
            if(connectorResult.Response.Payload == null)
                return new CommandResult("The payload of response is null or empty",false);
            
            return new CommandResult($"The Role {arguments.Rename} has been updated",true);         
        }
    }
}