using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gravityzero.Console.Utility.Context;
using Gravityzero.Console.Utility.Infrastructure;
using Gravityzero.Console.Utility.Model;
using Gravityzero.Console.Utility.Tools;

namespace Gravityzero.Console.Utility.Commands
{
    public class GetHuntingsCommand : ICommand
    {
        public string Description => "Lista polowań";

        public CommandResult Execute(ConsoleContext context)
        {
            Task<ConnectorResult<Response<IEnumerable<Hunting>>>> huntings = WinApiConnector.RequestGet<Response<IEnumerable<Hunting>>>($"{context.ConsoleSettings.ServerAddress}:{context.ConsoleSettings.Port}/Api/Hunting/GetHuntings");
            ConnectorResult<Response<IEnumerable<Hunting>>> connectorResulte = huntings.Result;

            if(!connectorResulte.IsSuccess)
                return new CommandResult(connectorResulte.Message,false);
            if(!connectorResulte.Response.IsSuccess)
                return new CommandResult(connectorResulte.Response.Code, false);
            if(!connectorResulte.Response.Payload.Any())
                return new CommandResult("The Payload of response is null or empty", false);
            int index = 1;

            foreach(var hunt in connectorResulte.Response.Payload){
                System.Console.WriteLine($"{index++}. {hunt.Identifier}");
                System.Console.WriteLine($"Data: {hunt.Issued} - Lider: {hunt.Leader.Login.ToUpper()} - Opis: {hunt.Description}");
                foreach (var item in hunt.Quarries)
                {
                    System.Console.WriteLine($"-- {item.Animal.Name}, szt. {item.Amount}");
                }
            }
            return new CommandResult("OK", true);
        }
    }
}