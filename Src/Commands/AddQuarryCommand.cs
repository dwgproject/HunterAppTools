using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommandLine;
using Gravityzero.Console.Utility.Context;
using Gravityzero.Console.Utility.Infrastructure;
using Gravityzero.Console.Utility.Model;
using Gravityzero.Console.Utility.Tools;

namespace Gravityzero.Console.Utility.Commands
{
    public class AddQuarryCommand : BaseCommand<QuarryArguments>
    {
        public AddQuarryCommand(IList<string> args) : base(args)
        {
        }

        public AddQuarryCommand(): base(new string[0])
        {           
        }

        protected override CommandResult Execute(ConsoleContext context, QuarryArguments arguments)
        {
            Animal animal = new Animal();
            if(string.IsNullOrEmpty(arguments.Animal)){
                Task<ConnectorResult<Response<IEnumerable<Animal>>>> animals = WinApiConnector.RequestGet<Response<IEnumerable<Animal>>>($"{context.ConsoleSettings.ServerAddress}:{context.ConsoleSettings.Port}/Api/Configuration/GetAllAnimals");
                ConnectorResult<Response<IEnumerable<Animal>>> preResult = animals.Result;
                if(!preResult.IsSuccess)
                    return new CommandResult(preResult.Message,false);
                if(!preResult.Response.IsSuccess)
                    return new CommandResult(preResult.Response.Code, false);
                if(!preResult.Response.Payload.Any())
                    return new CommandResult("The payload of response is empty",false);

                foreach(var item in preResult.Response.Payload){
                    System.Console.WriteLine($"{item.Name.ToUpper()}");
                }
                System.Console.Write("Wpisz nazwę zwierzęcia: ");
                var readLine = System.Console.ReadLine();
                var existAnimal = animals.Result.Response.Payload.Where(n=>n.Name == readLine.ToLower()).SingleOrDefault();
                animal = existAnimal;
            }

            if(!string.IsNullOrEmpty(arguments.Animal)){
                Task<ConnectorResult<Response<IEnumerable<Animal>>>> existsAnimal = WinApiConnector.RequestGet<Response<IEnumerable<Animal>>>($"{context.ConsoleSettings.ServerAddress}:{context.ConsoleSettings.Port}/Api/Configuration/GetAnimal/"+
                                    $"{arguments.Animal.ToLower()}");
                ConnectorResult<Response<IEnumerable<Animal>>> connectorResult = existsAnimal.Result;
                if(!connectorResult.IsSuccess)
                    return new CommandResult(connectorResult.Message,false);
                if(!connectorResult.Response.IsSuccess)
                    return new CommandResult(connectorResult.Response.Code, false);
                if(!connectorResult.Response.Payload.Any())
                    return new CommandResult($"The Animal {arguments.Animal.ToUpper()} doesn't exists",false);
                if(connectorResult.Response.Payload.Count() !=1 )
                    return new CommandResult("Too many results", false);
                
                animal = connectorResult.Response.Payload.FirstOrDefault();
            }
            
            Task<ConnectorResult<Response<string>>> result = WinApiConnector.RequestPost<Model.Quarry,Response<string>>($"{context.ConsoleSettings.ServerAddress}:{context.ConsoleSettings.Port}/Api/Hunting/AddQuarry",
                                 new Model.Quarry()
                                 {
                                     Animal = animal,
                                     Amount = arguments.Amount
                                 });
            ConnectorResult<Response<string>> postResult = result.Result;
            if(!postResult.IsSuccess)
                return new CommandResult(postResult.Message, false);
            if(!postResult.Response.IsSuccess)
                return new CommandResult(postResult.Response.Code, false);
            if(string.IsNullOrEmpty(postResult.Response.Payload))
                return new CommandResult("The payload of response is null or empty", false);
            return new CommandResult(postResult.Response.Payload,true);
        }
    }

    public class QuarryArguments
    {
        [Option('i',"identifier",Required=false,HelpText="Identyfikator zwierzyny łownej")]
        public string Identifier { get; set; }
        [Option('a', "animal", Required=false, HelpText="Nazwa zwierzyny")]
        public string Animal { get; set; }
        [Option('v',"value", Required=true, HelpText="Ilość do odstrzału")]
        public int Amount { get; set; }
    }
}