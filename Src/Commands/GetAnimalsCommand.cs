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
    public class GetAnimalsCommand : ICommand
    {
        // komendy animal znajdują sie w katalogu Configuration
        public string Description => "Zwrócenie listy zwierząt";

        public CommandResult Execute(ConsoleContext context)
        {
            Task<ConnectorResult<Response<IEnumerable<Animal>>>> result = WinApiConnector.RequestGet<Response<IEnumerable<Animal>>>($"{context.ConsoleSettings.ServerAddress}:{context.ConsoleSettings.Port}/Api/Configuration/GetAllAnimals");
            ConnectorResult<Response<IEnumerable<Animal>>> connectorResult = result.Result;

            if(!connectorResult.IsSuccess)
                return new CommandResult(connectorResult.Message, false);
            if(!connectorResult.Response.IsSuccess)
                return new CommandResult(connectorResult.Response.Code, false);
            if(!connectorResult.Response.Payload.Any())
                return new CommandResult("The payload of request is null or empty", false);
            
            int index =1;
            foreach(var animal in connectorResult.Response.Payload){
                System.Console.WriteLine($"{index++}. {animal.Name.ToUpper()}");
            }
            
            return new CommandResult("OK",true);
        }
    }
}