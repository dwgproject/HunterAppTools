using System.Collections.Generic;
using CommandLine;
using Gravityzero.Console.Utility.Context;
using Gravityzero.Console.Utility.Infrastructure;
using Gravityzero.Console.Utility.Tools;
using Gravityzero.Console.Utility.Model;
using log4net;
using System.Linq;

namespace Gravityzero.Console.Utility.Commands
{
    //testPath https://localhost:44377/User/SignUp
    //realPAth http://localhost:5000/Api/Configuration/GetAllRoles
    public class AddUserCommand : BaseCommand<UserArguments>
    {
        private readonly ILog logger = LogManager.GetLogger(typeof(AddUserCommand));
        public AddUserCommand(): base(new string [0]){
            
        }
        public AddUserCommand(IList<string> args) : base(args)
        {
            Description = "Metoda dodaje użytkownika.";
        }

        protected override CommandResult Execute(ConsoleContext context, UserArguments arguments)
        {
            var roles = WinApiConnector.RequestGet<string,Response<IEnumerable<Role>>>($"{context.ConsoleSettings.ServerAddress}:{context.ConsoleSettings.Port}/Api/Configuration/GetAllRoles","");

            System.Console.WriteLine("Wybierz role dla nowego użytkownika");
            int index =0;
            foreach(var r in roles.Result.Result.Payload){
                System.Console.WriteLine($"{index+1}. {r.Name}");
                index++;
            }
            string choice = "";
            int value=0;
            while(choice=="" || value==0){
                choice = System.Console.ReadLine();
                int.TryParse(choice, out value);
            }
            var role = new Role(){Identifier=roles.Result.Result.Payload.Select(i=>i.Identifier).ElementAt(value-1)};
            var result = WinApiConnector.RequestPost<Model.User, Response<User>>($"{context.ConsoleSettings.ServerAddress}:{context.ConsoleSettings.Port}/Api/User/SignUp",new Model.User(){Login = arguments.Login, Name = arguments.Name,
                        Password = arguments.Password, Surname = arguments.Surname, Role = role, Email = arguments.Email});
            System.Console.WriteLine($"Dodaje usera. Oto jego dane: {arguments.Name} {arguments.Surname}");
            return new CommandResult(result.Result.IsSuccess ? "OK" : result.Result.Message);
        }
    }

    public class UserArguments
    {
        [Option('n', "name", Required = true, HelpText = "Imię użytkownika.")]
        public string Name {get; set;}

        [Option('s', "surname", Required = true, HelpText = "Nazwisko użytkownika.")]
        public string Surname {get; set;}

        [Option('l', "login", Required = true, HelpText = "Login użytkownika.")]
        public string Login {get; set;}

        [Option('p', "password", Required = true, HelpText = "Hasło użytkownika.")]
        public string Password {get; set;}

        [Option('e', "email", Required = true, HelpText = "Email użytkownika.")]
        public string Email {get; set;}      
    }

}


            // using(IWinApiService<User, UserResult> service = new WinApiPostService<User, UserResult>()){
            //     try
            //     {
            //         service.Send();
            //     }
            //     catch(Exception ex)
            //     {
            //         if (logger.IsErrorEnabled)
            //             logger.Error(ex);
            //     }
            // }