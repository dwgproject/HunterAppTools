using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gravityzero.Console.Utility.Context;
using Gravityzero.Console.Utility.Infrastructure;
using Gravityzero.Console.Utility.Model;
using Gravityzero.Console.Utility.Tools;

namespace Gravityzero.Console.Utility.Commands
{
    public class DeleteAnimalCommand : BaseCommand<AnimalArguments>
    {
        public DeleteAnimalCommand(IList<string> args) : base(args)
        {
        }

        protected override CommandResult Execute(ConsoleContext context, AnimalArguments arguments)
        {
            Animal deleteAnimal = new Animal();
            IList<Animal> animals = new List<Animal>();
            if(string.IsNullOrEmpty(arguments.Name)){
                Task<ConnectorResult<Response<IEnumerable<Animal>>>> existsAnimals = WinApiConnector.RequestGet<Response<IEnumerable<Animal>>>($"{context.ConsoleSettings.ServerAddress}:{context.ConsoleSettings.Port}/Api/Configuration/");
                ConnectorResult<Response<IEnumerable<Animal>>> preResult = existsAnimals.Result;
                if(!preResult.IsSuccess)
                    return new CommandResult(preResult.Message, false);
                if(!preResult.Response.IsSuccess)
                    return new CommandResult(preResult.Response.Code,false);
                if(!preResult.Response.Payload.Any())
                    return new CommandResult("The payload of request is null or empty", false);
                foreach(var animal in preResult.Response.Payload){
                    animals.Add(animal);
                    System.Console.WriteLine($"{animal}");
                }
                bool shouldWork = true;
                int choisenOne = 0;

                do{
                    string readLine = System.Console.ReadLine();
                    bool isParsed = int.TryParse(readLine, out choisenOne);
                    shouldWork = isParsed ? choisenOne > animals.Count() : true;
                }while(shouldWork);

                deleteAnimal.Identifier = animals[choisenOne-1].Identifier;
            }
            
            if(!string.IsNullOrEmpty(arguments.Name)){

                Task<ConnectorResult<Response<IEnumerable<Animal>>>> existAnimal = WinApiConnector.RequestGet<Response<IEnumerable<Animal>>>($"{context.ConsoleSettings.ServerAddress}:{context.ConsoleSettings.Port}/Api/Configuration/GetAnimal"+$"/{arguments.Name.ToLower()}");
                ConnectorResult<Response<IEnumerable<Animal>>> connectorResult = existAnimal.Result;

                if(!connectorResult.IsSuccess)
                    return new CommandResult(connectorResult.Message, false);
                if(!connectorResult.Response.IsSuccess)
                    return new CommandResult(connectorResult.Response.Code, false);
                if(!connectorResult.Response.Payload.Any())
                    return new CommandResult("The paylod of request is null or empty",false);
                if(connectorResult.Response.Payload.Count()!=1)
                    return new CommandResult("There is too many results",false);
                deleteAnimal.Identifier = connectorResult.Response.Payload.FirstOrDefault().Identifier;
            }

            Task<ConnectorResult<Response<string>>> deleteResult = WinApiConnector.RequestDelete<Response<string>>($"{context.ConsoleSettings.ServerAddress}:{context.ConsoleSettings.Port}/Api/Configuration/DeleteAnimal"+$"/{deleteAnimal.Identifier}");
            ConnectorResult<Response<string>> postResult = deleteResult.Result;
            if(!postResult.IsSuccess)
                return new CommandResult(postResult.Message,false);
            if(!postResult.Response.IsSuccess)
                return new CommandResult(postResult.Response.Code,false);
            if(string.IsNullOrEmpty(postResult.Response.Payload))
                return new CommandResult("The payload of response is null or empty",false);

            return new CommandResult(postResult.Response.Payload,true);
        }
    }
}