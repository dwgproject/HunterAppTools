using System;
using System.Collections.Generic;
using System.Linq;
using CommandLine;
using Gravityzero.Console.Utility.Context;
using Gravityzero.Console.Utility.Infrastructure;
using Gravityzero.Console.Utility.Model;
using Gravityzero.Console.Utility.Tools;

namespace Gravityzero.Console.Utility.Commands
{
    public class AddHuntingCommand : BaseCommand<HuntingArguments>
    {

        User leader = new User();
        List<UserHunting> users = new List<UserHunting>();
        List<Quarry> quarries = new List<Quarry>();
        List<PartialHunting> partialHuntings = new List<PartialHunting>();
        public AddHuntingCommand(IList<string> args) : base(args)
        {
        }


        protected override CommandResult Execute(ConsoleContext context, HuntingArguments arguments)
        {

            List<string> emptyArguments = new List<string>();

            foreach(var arg in arguments.GetType().GetProperties())
            {
                var tmp = arg.GetValue(arguments, null);
                if(string.IsNullOrEmpty(tmp.ToString())){
                    System.Console.WriteLine(arg.Name);
                    emptyArguments.Add(arg.Name);
                }
            }

            var readerLider = new CsvReader<User>();
            leader = readerLider.LoadFile(arguments.Leader).FirstOrDefault();           
            var readerUsers = new CsvReader<UserHunting>();
            users = readerUsers.LoadFile(arguments.Users);
            var readerQuarry = new CsvReader<Quarry>();
            quarries = readerQuarry.LoadFile(arguments.Users);
            for(int i =0;i<arguments.PartialHunting;i++){
                partialHuntings.Add(new PartialHunting());
            }

            var result = WinApiConnector.RequestPost<Hunting, Response<Hunting>>($"{context.ConsoleSettings.ServerAddress}:{context.ConsoleSettings.Port}/Api/",
                        new Hunting(){Leader = leader, Users =users , Quarries = quarries, PartialHuntings = partialHuntings, });

            return new CommandResult(result.Result.IsSuccess ? "OK": result.Result.Message);
        }
    }


    public class HuntingArguments
    {
        [Option('l', "leader", Required=true, HelpText="Lider polowania")]
        public string Leader { get; set; }

        [Option('u', "users", Required=true, HelpText="Uczestnicy polowania")]
        public string Users { get; set; }

        [Option('q', "quarries", Required=true, HelpText="Zwierzyna łowna na polowanie")]
        public string Quarries { get; set; }

        [Option('p', "partial", Required=true, HelpText="Ilość miotów na polowanie")]
        public int PartialHunting { get; set; }
    }
}