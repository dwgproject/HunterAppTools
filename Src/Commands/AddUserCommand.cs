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
                return new CommandResult(connectorResult.Message, false);

            if (!connectorResult.Response.IsSuccess)
                return new CommandResult(connectorResult.Response.Code, false);

            if (!connectorResult.Response.Payload.Any())
                return new CommandResult("There are no roles for choice.", false);

            Role[] roleArray = connectorResult.Response.Payload.ToArray();
            System.Console.WriteLine($"Wybierz role dla nowego użytkownika: (1 - {roleArray.Length})");

            int index = 1;
            foreach(Role item in roleArray)
                System.Console.WriteLine($"{ index++ }. { item.Name.ToUpper() }");

            int chosenRoleIndex = 0;
            bool shouldWork = true;
            do
            {
                System.Console.Write("Wybierz numer roli: ");
                string choice = System.Console.ReadLine();
                bool isParsed = int.TryParse(choice, out chosenRoleIndex);
                shouldWork = isParsed ? chosenRoleIndex > roleArray.Length : true;
            }
            while(shouldWork);
    
            var role = new Role()
            {
                Identifier = roleArray[chosenRoleIndex - 1].Identifier
            };
            
            var result = WinApiConnector.RequestPost<Model.User, Response<string>>($"{context.ConsoleSettings.ServerAddress}:{context.ConsoleSettings.Port}/Api/User/SignUp",
                        new Model.User()
                        {
                            Login = arguments.Login.ToLower(), 
                            Name = arguments.Name.ToLower(),
                            Password = arguments.Password, 
                            Surname = arguments.Surname.ToLower(), 
                            Role = role, 
                            Email = arguments.Email.ToLower()
                        });

            ConnectorResult<Response<string>> postResult = result.Result;
            if (!postResult.IsSuccess)
                return new CommandResult(postResult.Message, false);
            
            if (!postResult.Response.IsSuccess)
                return new CommandResult(postResult.Response.Code, false);

            if (string.IsNullOrEmpty(postResult.Response.Payload))
                return new CommandResult("The payload of user is empty.", false);

            return new CommandResult("The user has been added.", true);
        }
    }

    public class UserArguments
    {
        [Option('n', "name", Required = true, HelpText = "Imię użytkownika.")]
        public virtual string Name {get; set;}

        [Option('s', "surname", Required = true, HelpText = "Nazwisko użytkownika.")]
        public virtual string Surname {get; set;}

        [Option('l', "login", Required = true, HelpText = "Login użytkownika.")]
        public virtual string Login {get; set;}

        [Option('p', "password", Required = true, HelpText = "Hasło użytkownika.")]
        public virtual string Password {get; set;}

        [Option('e', "email", Required = true, HelpText = "Email użytkownika.")]
        public virtual string Email {get; set;}      
    }
}