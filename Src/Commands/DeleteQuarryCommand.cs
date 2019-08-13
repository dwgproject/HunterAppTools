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
    public class DeleteQuarryCommand : BaseCommand<QuarryArguments>
    {
        //Do automatyzacji testów można będzie wykorzystać opconalny parametr -i<IDENTIFIER> klasy QuarryArguments
        public DeleteQuarryCommand(IList<string> args) : base(args)
        {
        }       
        protected override CommandResult Execute(ConsoleContext context, QuarryArguments arguments)
        {
            List<Quarry> quarries = new List<Quarry>();
            Quarry quarry = new Quarry();
            if(string.IsNullOrEmpty(arguments.Identifier)){
                Task<ConnectorResult<Response<IEnumerable<Quarry>>>> result = WinApiConnector.RequestGet<Response<IEnumerable<Quarry>>>($"{context.ConsoleSettings.ServerAddress}:{context.ConsoleSettings.Port}/Api/Hunting/GetQuarries");
                ConnectorResult<Response<IEnumerable<Quarry>>> preResult = result.Result;
                if(!preResult.IsSuccess)
                    return new CommandResult(preResult.Message, false);
                if(!preResult.Response.IsSuccess)
                    return new CommandResult(preResult.Response.Code, false);
                if(!preResult.Response.Payload.Any())
                    return new CommandResult("The payload of response is null or empty", false);

                foreach (var item in preResult.Response.Payload)
                {
                    System.Console.WriteLine($"{item.Animal.Name} - {item.Amount}");
                    quarries.Add(item);
                }

                bool shoudWork = true;
                int choisenOne = 0;

                do{
                    System.Console.Write("WYbierz zwierzyne do usunięcia");
                    string readLine = System.Console.ReadLine();
                    bool isParsed = int.TryParse(readLine, out choisenOne);
                    shoudWork = isParsed ? choisenOne > quarries.Count() : true;
                }while(shoudWork);

                quarry.Identifier = quarries[choisenOne-1].Identifier;
            }
            if(!string.IsNullOrEmpty(arguments.Identifier)){
                Guid deleteGuid = Guid.Empty;               
                bool isParsedGuid = Guid.TryParse(arguments.Identifier, out deleteGuid);
                if(isParsedGuid)
                   quarry.Identifier = deleteGuid; 
                return new CommandResult("The Identifier parsing error. Please use the correct identifier or do not use the -i param");               
            }

            Task<ConnectorResult<Response<string>>> deleteResult = WinApiConnector.RequestDelete<Response<string>>($"{context.ConsoleSettings.ServerAddress}:{context.ConsoleSettings.Port}/Api/Hunting/DeleteQuarry"+
                                $"{quarry.Identifier}");
            ConnectorResult<Response<string>> connectorResult = deleteResult.Result;
            if(!connectorResult.IsSuccess)
                return new CommandResult(connectorResult.Message, false);
            if(!connectorResult.Response.IsSuccess)
                return new CommandResult(connectorResult.Response.Code, false);
            if(string.IsNullOrEmpty(connectorResult.Response.Payload))
                return new CommandResult("The payload of response is null or empty", false);
            return new CommandResult(connectorResult.Response.Payload, true);
        }
    }
}