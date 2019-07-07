using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gravityzero.Console.Utility.Commands;
using Gravityzero.Console.Utility.Context;
using Gravityzero.Console.Utility.Infrastructure;
using Gravityzero.Console.Utility.Model;
using Gravityzero.Console.Utility.Tools;

namespace Gravityzero.Console.Utility.Commands
{
    public class GetRolesCommand : ICommand
    {
        public string Description => "Zwracanie listy ról istniejących w bazie";

        public CommandResult Execute(ConsoleContext context)
        {
            List<Role> listRoles = new List<Role>();
            Task<ConnectorResult<Response<IEnumerable<Role>>>> result = WinApiConnector.RequestGet<Response<IEnumerable<Role>>>($"{context.ConsoleSettings.ServerAddress}:{context.ConsoleSettings.Port}/Api/Configuration/GetAllRoles");
            ConnectorResult<Response<IEnumerable<Role>>> connectorResult = result.Result;

            if(!connectorResult.IsSuccess)
                return new CommandResult(connectorResult.Message, false);
            if(!connectorResult.Response.IsSuccess)
                return new CommandResult(connectorResult.Response.Code, false);
            if(!connectorResult.Response.Payload.Any())
                return new CommandResult("The payload of reguest is null or empty", false);            
                       
            foreach(var role in connectorResult.Response.Payload){
                System.Console.WriteLine($"{role.Name} -- ({role.Identifier})".PadLeft(70));
                listRoles.Add(new Role(){Identifier=role.Identifier, Name = role.Name});
            }                
              
            return new CommandResult("OK", true);        
        }
    }

}
