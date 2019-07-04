using System.Collections.Generic;
using System.Linq;
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

        protected override CommandResult Execute(ConsoleContext context, QuarryArguments arguments)
        {
            var animals = WinApiConnector.RequestGet<Response<IEnumerable<Animal>>>($"{context.ConsoleSettings.ServerAddress}:{context.ConsoleSettings.Port}/Api");
            Animal tmpAnimal = new Animal();
            if(string.IsNullOrEmpty(arguments.Animal)){             
                foreach(var item in animals.Result.Result.Payload){
                    System.Console.WriteLine($"{item.Name}");
                }
                System.Console.Write("Wpisz nazwę zwierzęcia: ");
                var animal = System.Console.ReadLine();
                var existAnimal = animals.Result.Result.Payload.Where(n=>n.Name == animal).SingleOrDefault();
                tmpAnimal = existAnimal;
            }
            else{
                tmpAnimal = animals.Result.Result.Payload.Where(n=>n.Name == arguments.Animal).SingleOrDefault();
            }
            
            var result = WinApiConnector.RequestPost<Quarry,Response<Quarry>>($"{context.ConsoleSettings.ServerAddress}:{context.ConsoleSettings.Port}/Api/", new Quarry(){Animal = tmpAnimal, Amount = arguments.Amount});
            return new CommandResult(result.Result.Result.IsSuccess ? "OK" : result.Result.Message);
        }
    }

    public class QuarryArguments
    {
        [Option('i',"identifier",Required=false,HelpText="Identyfikator zwierzyny łownej")]
        public string Identifier { get; set; }
        [Option('a', "animal", Required=false, HelpText="Nazwa zwierzyny")]
        public string Animal { get; set; }
        [Option('v',"value", Required=false, HelpText="Ilość do odstrzału")]
        public int Amount { get; set; }
    }
}