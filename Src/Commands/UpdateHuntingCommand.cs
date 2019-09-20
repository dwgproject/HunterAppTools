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
            IList<UserHunting> userHuntings = new List<UserHunting>();
            if(string.IsNullOrEmpty(arguments.Identifier))
                return new CommandResult("Identifier is required to update hunting event", false);

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

            if(!string.IsNullOrEmpty(arguments.Path)){
                IDictionary<string,string> pathDictionary = new Dictionary<string,string>();
                IReader<PathClass> pathFile = new CsvReader<PathClass>();
                var pathsData = pathFile.LoadFile(arguments.Path);
                pathDictionary = pathsData.ToDictionary(x=>x.Path,y=>y.TypeName);
                
                if(pathDictionary.ContainsKey("User")){
                    IReader<User> userCsv = new CsvReader<User>();
                    var usersListFromCsv = userCsv.LoadFile(pathDictionary["User"]);
                    foreach (var item in usersListFromCsv)
                    {
                        userHuntings.Add(new UserHunting(){UserId = item.Identifier, HuntingId = existHunting.Identifier});
                    }
                    existHunting.Users = userHuntings;
                }
                if(pathDictionary.ContainsKey("Quarry")){
                    IReader<Quarry> quarryCsv = new CsvReader<Quarry>();
                    var quarriesFromCsv = quarryCsv.LoadFile(pathDictionary["Quarry"]);
                }
                if(pathDictionary.ContainsKey("Leader")){
                    IReader<User> leaderCsv = new CsvReader<User>();
                    var leaderFromCsv = leaderCsv.LoadFile(pathDictionary["Leader"]);
                    existHunting.Leader = leaderFromCsv.FirstOrDefault();
                }
            }

            existHunting.Description = arguments.Description ?? existHunting.Description;

            Task<ConnectorResult<Response<Hunting>>> request = WinApiConnector.RequestPut<Hunting,Response<Hunting>>($"{context.ConsoleSettings.ServerAddress}:{context.ConsoleSettings.Port}/Api/Hunting/UpdateHunting",existHunting);
            ConnectorResult<Response<Hunting>> response = request.Result;
            if(!response.IsSuccess)
                return new CommandResult(response.Message, false);
            if(!response.Response.IsSuccess)
                return new CommandResult(response.Response.Code, false);
            if(response.Response.Payload == null)
                return new CommandResult("The payload of response is null",false);

            return new CommandResult("OK", true);
        }
    }

    public class PathClass
    {
        public string Path { get; set; }
        public string TypeName { get; set; }
    }
}