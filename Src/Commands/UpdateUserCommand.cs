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
    public class UpdateUserCommand: BaseCommand<UserArgumentsExtension> 
    {
        //Brak metody update w kontrolerze user
        public UpdateUserCommand(IList<string> args) : base(args)
        {
        }

        protected override CommandResult Execute(ConsoleContext context, UserArgumentsExtension arguments)
        {
            if(string.IsNullOrEmpty(arguments.Identifier)){
                System.Console.WriteLine("Procedura aktualizacji konta użytkownika. Postępuje zgodnie z instrukcją");
                System.Console.Write("Login użytkownika: ");
                var login = System.Console.ReadLine();
                System.Console.Write("Hasło użytkownika: ");
                var password = System.Console.ReadLine();
                var existUser = GetUser(new User(){Login=login,Password=password});
                if(existUser==null){
                    System.Console.WriteLine("Nie istnieje taki użytkownik");
                }
                var newUSer = new User(){Identifier=existUser.Identifier, Name=arguments.Name, Password=arguments.Password,Surname=arguments.Surname, Email=arguments.Email};
                var result = WinApiConnector.RequestPost<User,Response<string>>($"{context.ConsoleSettings.ServerAddress}:{context.ConsoleSettings.Port}/Api/User/",newUSer);
                return new CommandResult(result.Result.IsSuccess ? "OK" : result.Result.Message);
            }
            else{
                return new CommandResult();
            }
        }

        private User GetUser(User user)
        {
            var result = WinApiConnector.RequestPost<User, Response<IEnumerable<User>>>("http://localhost:5000/Api/User/",user);
            if(result.Result.Result.IsSuccess){
                return new User(){Identifier=result.Result.Result.Payload.FirstOrDefault().Identifier};
            }
            else
            {
                return null;   
            }
        }
    }
    
    public class UserArgumentsExtension: UserArguments
    {
        [Option('i',"identifier", Required=false, HelpText="Identyfikator użytkownika")]
        public string Identifier { get; set; }
    }
}