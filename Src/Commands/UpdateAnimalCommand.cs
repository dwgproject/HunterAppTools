using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gravityzero.Console.Utility.Context;
using Gravityzero.Console.Utility.Infrastructure;
using Gravityzero.Console.Utility.Model;
using Gravityzero.Console.Utility.Tools;

namespace Gravityzero.Console.Utility.Commands
{
    public class UpdateAnimalCommand : BaseCommand<AnimalArguments>
    {
        public UpdateAnimalCommand(IList<string> args) : base(args)
        {
        }

        protected override CommandResult Execute(ConsoleContext context, AnimalArguments arguments)
        {
            IList<Animal> animals = new List<Animal>();
            Animal updateAnimal = new Animal();

            if(string.IsNullOrEmpty(arguments.Rename)){
                return new CommandResult("Przy update wymagany parametr -r <NAZWA>");
            }

            if(string.IsNullOrEmpty(arguments.Name)){
                Task<ConnectorResult<Response<IEnumerable<Animal>>>> animalsList = WinApiConnector.RequestGet<Response<IEnumerable<Animal>>>($"{context.ConsoleSettings.ServerAddress}:{context.ConsoleSettings.Port}/Api/Configuration/GetAllAnimals");
                ConnectorResult<Response<IEnumerable<Animal>>> preResponse = animalsList.Result;
                if(!preResponse.IsSuccess)
                    return new CommandResult(preResponse.Message, false);
                if(!preResponse.Response.IsSuccess)
                    return new CommandResult(preResponse.Response.Code, false);
                if(!preResponse.Response.Payload.Any())
                    return new CommandResult("The payload od request is null or empty", false);

                foreach(var preRes in preResponse.Response.Payload){
                    animals.Add(preRes);
                    System.Console.WriteLine($"{preRes.Name}");
                }
                
                bool shouldWork = true;
                int choisenOne = 0;
                do{
                    string readLine = System.Console.ReadLine();
                    bool isParsed = int.TryParse(readLine, out choisenOne);
                    shouldWork = isParsed ? choisenOne > animals.Count() : true;

                }while(shouldWork);

                updateAnimal.Identifier = animals[choisenOne-1].Identifier;                              
            }
            if(!string.IsNullOrEmpty(arguments.Name)){
                Task<ConnectorResult<Response<IEnumerable<Animal>>>> existAnimal = WinApiConnector.RequestGet<Response<IEnumerable<Animal>>>($"{context.ConsoleSettings.ServerAddress}:{context.ConsoleSettings.Port}/Api/Configuration/GetAnimal/"+
                                    $"{arguments.Name.ToLower()}");
                ConnectorResult<Response<IEnumerable<Animal>>> connectorResult = existAnimal.Result;

                if(!connectorResult.IsSuccess)
                    return new CommandResult(connectorResult.Message, false);
                if(!connectorResult.Response.IsSuccess)
                    return new CommandResult(connectorResult.Response.Code, false);
                if(!connectorResult.Response.Payload.Any())
                    return new CommandResult("The Payload of request is null or empty", false);
                if(connectorResult.Response.Payload.Count() != 1)
                    return new CommandResult("There is too many results", true);
                
                updateAnimal.Identifier = connectorResult.Response.Payload.FirstOrDefault().Identifier;
            }

            updateAnimal.Name = arguments.Rename.ToLower();
            Task<ConnectorResult<Response<string>>> result = WinApiConnector.RequestPut<Animal, Response<string>>($"{context.ConsoleSettings.ServerAddress}:{context.ConsoleSettings.Port}/Api/Configuration/UpdateAnimal",updateAnimal);
            ConnectorResult<Response<string>> postResult = result.Result;
            if(!postResult.IsSuccess)
                return new CommandResult(postResult.Message, false);
            if(!postResult.Response.IsSuccess)
                return new CommandResult(postResult.Response.Code, false);
            if(postResult.Response.Payload == null)
                return new CommandResult("The payload of response is null or empty",false);
            
            return new CommandResult($"The Animal {updateAnimal.Identifier} has been updated",true);
        }
    }
}