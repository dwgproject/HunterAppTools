using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gravityzero.Console.Utility.Commands;
using Gravityzero.Console.Utility.Context;
using Gravityzero.Console.Utility.Infrastructure;
using Gravityzero.Console.Utility.Model;
using Gravityzero.Console.Utility.Tools;

namespace Gravityzero.Console.Utility.Commands
{
    public class UpdateHuntingCommand : BaseCommand<HuntingArguments>
    {
        public UpdateHuntingCommand(IList<string> args) : base(args)
        {
        }

        protected override CommandResult Execute(ConsoleContext context, HuntingArguments arguments)
        {
            if(string.IsNullOrEmpty(arguments.Identifier))
                return new CommandResult("Identifier is required to update hunting event", false);
            // if(!string.IsNullOrEmpty(arguments.Path)){

            //     var file = new CsvReader<Hunting>();
            //     var huntingFromFile = file.LoadFile(arguments.Path);
            //     //dokończyć
            // }
            
            Task<ConnectorResult<Response<IEnumerable<Hunting>>>> preRequest = WinApiConnector.RequestGet<Response<IEnumerable<Hunting>>>($"{context.ConsoleSettings.ServerAddress}:{context.ConsoleSettings.Port}/Api/Hunting/GetHunting/{arguments.Identifier}");
            ConnectorResult<Response<IEnumerable<Hunting>>> preResponse = preRequest.Result;
            if(!preResponse.IsSuccess)
                return new CommandResult(preRequest.Result.Message, false);
            if(!preResponse.Response.IsSuccess)
                return new CommandResult(preResponse.Response.Code, false);
            if(!preResponse.Response.Payload.Any())
                return new CommandResult("The Payload od request is null or empty", false);
            if(preResponse.Response.Payload.Count()!=1)
                return new CommandResult("Too many results in response", false);

            Hunting existHunting = preResponse.Response.Payload.FirstOrDefault();

            System.Console.WriteLine($"{existHunting.Issued}, Leader: {existHunting.Leader.Surname}, {existHunting.Status}");
            System.Console.WriteLine("Users:");
            foreach (var item in existHunting.Users)
            {
                System.Console.WriteLine($"-- {item.User.Surname.ToUpper()}, {item.User.Name.ToUpper()}");
            }
            System.Console.WriteLine("Quarries:");
            foreach (var item in existHunting.Quarries)
            {
                System.Console.WriteLine($"-- {item.Animal.Name}, {item.Amount}");
            }

            return new CommandResult("OK", true);
        }
    }
}