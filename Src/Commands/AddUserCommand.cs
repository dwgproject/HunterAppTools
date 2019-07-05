using System.Collections.Generic;
using CommandLine;
using Gravityzero.Console.Utility.Context;
using Gravityzero.Console.Utility.Infrastructure;
using Gravityzero.Console.Utility.Tools;
using Gravityzero.Console.Utility.Model;
using log4net;
using System.Linq;
using System.Threading.Tasks;

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
            Task<ConnectorResult<Response<IEnumerable<Role>>>> roles = WinApiConnector.RequestGet<Response<IEnumerable<Role>>>($"{context.ConsoleSettings.ServerAddress}:{context.ConsoleSettings.Port}/Api/Configuration/GetAllRoles");
            ConnectorResult<Response<IEnumerable<Role>>> connectorResult = roles.Result;

            if (!connectorResult.IsSuccess)
                return new CommandResult(connectorResult.Message);

            if (!connectorResult.Response.IsSuccess)
                return new CommandResult(connectorResult.Response.Code);

            if (!connectorResult.Response.Payload.Any())
                return new CommandResult("There are no roles for choice.");

            Role[] roleArray = connectorResult.Response.Payload.ToArray();
            System.Console.WriteLine($"Wybierz role dla nowego użytkownika: (1 - {roleArray.Length})");

            int index = 1;
            foreach(Role item in roleArray)
                System.Console.WriteLine($"{ index++ }. { item.Name }");

            int chosenRoleIndex = 0;
            bool shouldWork = true;
            do
            {
                string choice = System.Console.ReadLine();
                bool isParsed = int.TryParse(choice, out chosenRoleIndex);
                shouldWork = isParsed ? chosenRoleIndex > roleArray.Length : true;
            }
            while(shouldWork);
    
            var role = new Role()
            {
                Identifier = roleArray[chosenRoleIndex - 1].Identifier
            };
            
            var result = WinApiConnector.RequestPost<Model.User, Response<User>>($"{context.ConsoleSettings.ServerAddress}:{context.ConsoleSettings.Port}/Api/User/SignUp",
                        new Model.User()
                        {
                            Login = arguments.Login, 
                            Name = arguments.Name,
                            Password = arguments.Password, 
                            Surname = arguments.Surname, 
                            Role = role, 
                            Email = arguments.Email
                        });

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


// string choice = string.Empty;
            // int value = 0;
            // while(string.IsNullOrEmpty(choice) || value==0){
            //     choice = System.Console.ReadLine();
            //     int.TryParse(choice, out value);
            // }

            //roles.Result.Response.Payload.Select(i => i.Identifier).ElementAt(value - 1)