using System;
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
    public class AddHuntingCommand : BaseCommand<HuntingArguments>
    {

        
        List<UserHunting> users = new List<UserHunting>();
        
        
        public AddHuntingCommand(IList<string> args) : base(args)
        {
        }


        protected override CommandResult Execute(ConsoleContext context, HuntingArguments arguments)
        {
            IList<User> usersList = new List<User>();
            IList<UserHunting> users = new List<UserHunting>();
            IList<Animal> animals = new List<Animal>();
            IList<Quarry> quarries = new List<Quarry>();
            IList<PartialHunting> partialHuntings = new List<PartialHunting>();
            User leader = new User();
            Hunting hunting = new Hunting();
            Task<ConnectorResult<Response<IEnumerable<User>>>> usersRequest = WinApiConnector.RequestGet<Response<IEnumerable<User>>>($"{context.ConsoleSettings.ServerAddress}:{context.ConsoleSettings.Port}/Api/User/GetAll");
            ConnectorResult<Response<IEnumerable<User>>> preResult = usersRequest.Result;
            if(!preResult.IsSuccess)
                return new CommandResult(preResult.Message, false);
            if(!preResult.Response.IsSuccess)
                return new CommandResult(preResult.Response.Code, false);
            if(!preResult.Response.Payload.Any())
                return new CommandResult("The payload of response is null or empty", false);
                        
            foreach (var item in preResult.Response.Payload){
                usersList.Add(item);
            }

            Task<ConnectorResult<Response<IEnumerable<Animal>>>> animalsList = WinApiConnector.RequestGet<Response<IEnumerable<Animal>>>($"{context.ConsoleSettings.ServerAddress}:{context.ConsoleSettings.Port}/Api/COnfiguration/GetAllAnimals");
            ConnectorResult<Response<IEnumerable<Animal>>> animalsResult = animalsList.Result;
            if(!animalsResult.IsSuccess)
                return new CommandResult(animalsResult.Message, false);
            if(!animalsResult.Response.IsSuccess)
                return new CommandResult(animalsResult.Response.Code, false);
            if(!animalsResult.Response.Payload.Any())
                return new CommandResult("The payload of response is null or empty", false);
            
            foreach (var item in animalsResult.Response.Payload){
                animals.Add(item);               
            }
                                 
            if(string.IsNullOrEmpty(arguments.Leader)){
                
                System.Console.WriteLine("Wybierz lidera");
                int index = 1;
                foreach (var item in usersList)
                {
                    System.Console.WriteLine($"{index++}. {item.Name}, {item.Surname}");
                }
                bool shouldWork = true;
                int choisenOne = 0;

                do{
                    string choiceLeader = System.Console.ReadLine();
                    bool isParsed = int.TryParse(choiceLeader,out choisenOne);
                    shouldWork = isParsed ? choisenOne > usersList.Count() : true;
                }while(shouldWork);

                leader = usersList[choisenOne-1];
            }
            foreach (var item in animals)
            {
                quarries.Add(new Quarry()
                    {
                        Animal = item,
                        Amount = 3
                    });
            }

            foreach (var item in usersList)
            {
                users.Add(new UserHunting(){UserId = item.Identifier});
            }

            hunting.Leader = leader;
            hunting.Quarries = quarries;
            hunting.Users = users;

            for(int i=0; i<arguments.PartialHunting; i++){
                partialHuntings.Add(new PartialHunting(){
                    Number = i+1,
                    Status = Status.Create,
                    PartialHunters = new List<PartialHuntersList>()
                });
            }
            
            hunting.PartialHuntings = partialHuntings;
            hunting.Description = "Pierwsze";

            Task<ConnectorResult<Response<string>>> addResult = WinApiConnector.RequestPost<Hunting,Response<string>>($"{context.ConsoleSettings.ServerAddress}:{context.ConsoleSettings.Port}/Api/Hunting/AddHunting",hunting);
            ConnectorResult<Response<string>> connectorResult = addResult.Result;
            if(!connectorResult.IsSuccess)
                return new CommandResult(connectorResult.Message, false);
            if(!connectorResult.Response.IsSuccess)
                return new CommandResult(connectorResult.Response.Code, false);
            if(string.IsNullOrEmpty(connectorResult.Response.Payload))
                return new CommandResult("The payload of response is null or empty", false);
            return new CommandResult("OK",true);
        }
    }


    public class HuntingArguments
    {
        [Option('i',"identifier", Required = false, HelpText = "identyfikator polowania")]
        public string Identifier { get; set; }

        [Option('l', "leader", Required=false, HelpText="Lider polowania")]
        public string Leader { get; set; }

        [Option('u', "users", Required=false, HelpText="Uczestnicy polowania")]
        public string Users { get; set; }

        [Option('q', "quarries", Required=false, HelpText="Zwierzyna łowna na polowanie")]
        public string Quarries { get; set; }

        [Option('h', "partialhunting", Required=false, HelpText="Ilość miotów na polowanie")]
        public int PartialHunting { get; set; }

        [Option('p', "path", Required = false, HelpText = "path do pliku")]
        public string Path { get; set; }
    }
}