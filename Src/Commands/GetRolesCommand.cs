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
    public class GetRolesCommand : ICommand
    {
        public string Description => "Zwracanie listy ról istniejących w bazie";

        public CommandResult Execute(ConsoleContext context)
        {
            Task<ConnectorResult<Response<IEnumerable<Role>>>> result = WinApiConnector.RequestGet<Response<IEnumerable<Role>>>($"{context.ConsoleSettings.ServerAddress}:{context.ConsoleSettings.Port}/Api/Configuration/GetAllRoles");
            ConnectorResult<Response<IEnumerable<Role>>> connectorResult = result.Result;

            if(!connectorResult.IsSuccess)
                return new CommandResult(connectorResult.Message, false);
            if(!connectorResult.Response.IsSuccess)
                return new CommandResult(connectorResult.Response.Code, false);
            if(!connectorResult.Response.Payload.Any())
                return new CommandResult("The payload of reguest is null or empty", false);            

            TablePresenter table = new TablePresenter(new [] {"Identifier", "Name"}, connectorResult.Response.Payload);
            return new CommandResult(table.Render(), true);        
        }
    }
}