using System.Collections.Generic;
using System.Threading.Tasks;
using Gravityzero.Console.Utility.Commands;
using Gravityzero.Console.Utility.Context;
using Gravityzero.Console.Utility.Infrastructure;
using Gravityzero.Console.Utility.Model;
using Gravityzero.Console.Utility.Tools;
using System.Linq;

namespace Gravityzero.Console.Utility.Commands
{
    public class GetQuarriesCommand : ICommand
    {
        public string Description => "Lista zwierzyny łownej";

        public CommandResult Execute(ConsoleContext context)
        {
            Task<ConnectorResult<Response<IEnumerable<Quarry>>>> quarrier = WinApiConnector.RequestGet<Response<IEnumerable<Quarry>>>($"{context.ConsoleSettings.ServerAddress}:{context.ConsoleSettings.Port}/Api/Hunting/GetQuarries");
            ConnectorResult<Response<IEnumerable<Quarry>>> connectorResult = quarrier.Result;
            if(!connectorResult.IsSuccess)
                return new CommandResult(connectorResult.Message, false);
            if(!connectorResult.Response.IsSuccess)
                return new CommandResult(connectorResult.Response.Code, false);
            if(!connectorResult.Response.Payload.Any())
                return new CommandResult("The payload of response is null or empty", false);
            
            int index = 1;
            foreach(var quarry in connectorResult.Response.Payload){
                System.Console.WriteLine($"{index++}. {quarry.Animal.Name.ToUpper()} - {quarry.Amount}");
            }
            return new CommandResult("OK",true);
        }
    }
}