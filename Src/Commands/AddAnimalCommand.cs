using System.Collections.Generic;
using System.Threading.Tasks;
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
            if(string.IsNullOrEmpty(arguments.Name))
                return new CommandResult("The name of animal is required, use param -n <NAME>");
            Task<ConnectorResult<Response<string>>> result = WinApiConnector.RequestPost<Animal, Response<string>>($"{context.ConsoleSettings.ServerAddress}:{context.ConsoleSettings.Port}/Api/Configuration/AddAnimal",
                                new Animal()
                                {
                                    Name = arguments.Name.ToLower()
                                });
            ConnectorResult<Response<string>> connectorResult = result.Result;
            
            if(!connectorResult.IsSuccess)
                return new CommandResult(connectorResult.Message, false);
            if(!connectorResult.Response.IsSuccess)
                return new CommandResult(connectorResult.Response.Code, false);
            if(string.IsNullOrEmpty(connectorResult.Response.Payload))
                return new CommandResult("The payload of request is null or empty", false);
            
            return new CommandResult(connectorResult.Response.Payload, true);
        }
    }

    public class AnimalArguments
    {
        [Option('n',"name", Required=true, HelpText="Nazwa zwierzęcia")]
        public string Name { get; set; }
        [Option('r',"rename", Required=false, HelpText="Zmiana nazwys zwierzęcia")]
        public string Rename { get; set; }
    }
}