using System.Collections.Generic;
using System.Linq;
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
            if(string.IsNullOrEmpty(arguments.Rename)){
                return new CommandResult("Przy update wymagany parametr -r <NAZWA>");
            }
            var existAnimal = WinApiConnector.RequestPost<string,Response<IEnumerable<Animal>>>($"{context.ConsoleSettings.ServerAddress}:{context.ConsoleSettings.Port}/Api/",arguments.Name);
            if(!existAnimal.Result.IsSuccess){
                return new CommandResult(existAnimal.Result.Message);
            }
            var result = WinApiConnector.RequestPost<Animal,Response<Animal>>($"{context.ConsoleSettings.ServerAddress}:{context.ConsoleSettings.Port}/Api/", 
                        new Animal(){Identifier = existAnimal.Result.Result.Payload.FirstOrDefault().Identifier, Name=arguments.Rename});
            return new CommandResult(result.Result.IsSuccess ? "OK" : result.Result.Message);
        }
    }
}