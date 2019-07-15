using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Gravityzero.Console.Utility.Context;
using Gravityzero.Console.Utility.Infrastructure;
using Gravityzero.Console.Utility.Model;
using Gravityzero.Console.Utility.Tools;

namespace Gravityzero.Console.Utility.Commands
{
    public class UpdateQuarryCommand : BaseCommand<QuarryArguments>
    {
        public UpdateQuarryCommand(IList<string> args) : base(args)
        {
        }

        protected override CommandResult Execute(ConsoleContext context, QuarryArguments arguments)
        {
            IList<Quarry> quarriesList = new List<Quarry>();
            Quarry quarry = new Quarry();
            if(string.IsNullOrEmpty(arguments.Identifier)){
                Task<ConnectorResult<Response<IEnumerable<Quarry>>>> quarries = WinApiConnector.RequestGet<Response<IEnumerable<Quarry>>>($"{context.ConsoleSettings.ServerAddress}:{context.ConsoleSettings.Port}/Api/Hunting/GetQuarries");
                ConnectorResult<Response<IEnumerable<Quarry>>> preResult = quarries.Result;
                if(!preResult.IsSuccess)
                    return new CommandResult(preResult.Message, false);
                if(!preResult.Response.IsSuccess)
                    return new CommandResult(preResult.Response.Code, false);
                if(!preResult.Response.Payload.Any())
                    return new CommandResult("The payload of response is null or empty", false);
                int index = 1;
                foreach (var item in preResult.Response.Payload)
                {
                    System.Console.WriteLine($"{index++}. {item.Animal.Name} - amount: {item.Amount} szt.");
                    quarriesList.Add(item);
                }
                bool shouldWork = true;
                int choisenOne = 0;

                do{
                    System.Console.Write("Wybierz zwierzyne do aktualizacji: ");
                    string userChoice = System.Console.ReadLine();
                    bool isParsed = int.TryParse(userChoice, out choisenOne);
                    shouldWork = isParsed ? choisenOne > quarriesList.Count(): true;

                }while(shouldWork);

                quarry.Identifier = quarriesList[choisenOne-1].Identifier;
            }
            if(!string.IsNullOrEmpty(arguments.Identifier)){
                Task<ConnectorResult<Response<IEnumerable<Quarry>>>> result = WinApiConnector.RequestGet<Response<IEnumerable<Quarry>>>($"{context.ConsoleSettings.ServerAddress}:{context.ConsoleSettings.Port}/Api/Hunting/GetQuarry/"+
                                    $"{arguments.Identifier}");
                ConnectorResult<Response<IEnumerable<Quarry>>> connectorResult = result.Result;
                if(!connectorResult.IsSuccess)
                    return new CommandResult(connectorResult.Message,false);
                if(!connectorResult.Response.IsSuccess)
                    return new CommandResult(connectorResult.Response.Code, false);
                if(!connectorResult.Response.Payload.Any())
                    return new CommandResult("The payload od response is null or empty", false);
                if(connectorResult.Response.Payload.Count() != 1)
                    return new CommandResult("Too many results in payload", false);
                quarry.Identifier = connectorResult.Response.Payload.FirstOrDefault().Identifier;
            }
            quarry.Amount = arguments.Amount;
            Task<ConnectorResult<Response<string>>> update = WinApiConnector.RequestPut<Quarry,Response<string>>($"{context.ConsoleSettings.ServerAddress}:{context.ConsoleSettings.Port}/Api/Hunting/UpdateQuarry", quarry);
            ConnectorResult<Response<string>> postResult = update.Result;
            if(!postResult.IsSuccess)
                return new CommandResult(postResult.Message, false);
            if(!postResult.Response.IsSuccess)
                return new CommandResult(postResult.Response.Code, false);
            if(string.IsNullOrEmpty(postResult.Response.Payload))
                return new CommandResult("The paylod of response is null or empty", false);
            return new CommandResult(postResult.Response.Payload,true);
        }
    }
}