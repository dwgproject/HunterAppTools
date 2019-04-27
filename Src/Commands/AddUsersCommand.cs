using System.Collections.Generic;
using CommandLine;
using Gravityzero.Console.Utility.Context;
using Gravityzero.Console.Utility.Infrastructure;
using Gravityzero.Console.Utility.Logger;
using Gravityzero.Console.Utility.Model;
using Gravityzero.Console.Utility.Tools;
using log4net;

namespace Gravityzero.Console.Utility.Commands
{
    public class AddUsersCommand : BaseCommand<UsersAgruments>
    {
        private readonly ILog log = LogManager.GetLogger(typeof(AddUsersCommand));
        public AddUsersCommand(IList<string> args) : base(args)
        {
            LoggerConfiguration.LoadConfiguration();
        }

        protected override CommandResult Execute(ConsoleContext context, UsersAgruments arguments)
        {
            if(string.IsNullOrEmpty(arguments.Path)){
                return new CommandResult($"Path{arguments.Path} jest pusty");
            }

            var file = new CsvReader<User>();
            var users = file.LoadFile(arguments.Path);
            int success = 0;
            foreach(var user in users){
                var result = WinApiConnector.RequestPost<User, Response<User>>("",user);
                if(result.Result.IsSuccess){
                    success++;
                }
                else{
                    log.Error(result.Result.Message);
                }
            }
            return new CommandResult(success==users.Count ? "OK" : "Error! Sprawdź logi");
        }
    }

    public class UsersAgruments
    {
        [Option('p',"path", Required=true, HelpText="Ścieżka do pliku z listą użytkowników")]
        public string Path { get; set; }
    }
}