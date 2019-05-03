using System.Collections.Generic;
using System.Linq;
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
            var existAnimal = WinApiConnector.RequestPost<string,Response<IEnumerable<Animal>>>("", arguments.Name);
            if(existAnimal.Result.Result.Payload.Count()==1){
                var result = WinApiConnector.RequestDelete<string,Response<string>>(""+$"/{existAnimal.Result.Result.Payload.FirstOrDefault().Identifier}");
                return new CommandResult(result.Result.IsSuccess ? "OK" : result.Result.Message);
            }
            else if(existAnimal.Result.Result.Payload.Count()>1){
                System.Console.WriteLine($"Znaleziono więcej niż jeden obiekt o nazwie {arguments.Name} ({existAnimal.Result.Result.Payload.Count()})");
                System.Console.WriteLine($"Dokonaj updatu jednej z nazw ({arguments.Name})");
                return new CommandResult();
            }
            else if(existAnimal.Result.Result.Payload.Count()==0){
                System.Console.WriteLine($"Brak obiektu w db ({arguments.Name})");
                return new CommandResult();
            }
            return new CommandResult();
        }
    }
}