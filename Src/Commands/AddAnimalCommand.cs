using System.Collections.Generic;
using CommandLine;
using Gravityzero.Console.Utility.Context;
using Gravityzero.Console.Utility.Infrastructure;
using Gravityzero.Console.Utility.Model;
using Gravityzero.Console.Utility.Tools;

namespace Gravityzero.Console.Utility.Commands
{
    public class AddAnimalCommand : BaseCommand<AnimalArguments>
    {
        public AddAnimalCommand(IList<string> args) : base(args)
        {
        }

        protected override CommandResult Execute(ConsoleContext context, AnimalArguments arguments)
        {
            var result = WinApiConnector.RequestPost<Animal, Response<Animal>>("https://localhost:44377/Animal/Add",new Animal(){Name = arguments.Name});
            return new CommandResult(result.Result.IsSuccess ? "OK" : result.Result.Message);
        }
    }

    public class AnimalArguments
    {
        [Option('n',"name", Required=true, HelpText="Nazwa zwierzęcia")]
        public string Name { get; set; }
    }
}