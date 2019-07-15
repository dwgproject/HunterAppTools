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
    public class UpdateUserCommand: BaseCommand<UserArgumentsExtension> 
    {
        //Brak metody update w kontrolerze user
        public UpdateUserCommand(IList<string> args) : base(args)
        {
        }

        protected override CommandResult Execute(ConsoleContext context, UserArgumentsExtension arguments)
        {
            IList<User> users = new List<User>();
            User updateUser = new User();
            Guid identifier = Guid.Empty;

            if(string.IsNullOrEmpty(arguments.Identifier) && string.IsNullOrEmpty(arguments.Login)){

                Task<ConnectorResult<Response<IEnumerable<User>>>> usersRequest = WinApiConnector.RequestGet<Response<IEnumerable<User>>>($"{context.ConsoleSettings.ServerAddress}:{context.ConsoleSettings.Port}/Api/User/GetAll");
                ConnectorResult<Response<IEnumerable<User>>> preResponse = usersRequest.Result;

                if(!preResponse.IsSuccess)
                    return new CommandResult(preResponse.Message, false);
                if(!preResponse.Response.IsSuccess)
                    return new CommandResult(preResponse.Response.Code, false);
                if(!preResponse.Response.Payload.Any())
                    return new CommandResult("The payload od request is null or empty", false);
                int index = 1;
                foreach(var ur in preResponse.Response.Payload){
                    users.Add(ur);
                    System.Console.WriteLine($"{index++}. {ur.Login} - {ur.Identifier}");
                }
                
                System.Console.WriteLine($"Wybierz numer użytkownika do aktualizacji 1-{users.Count}");
                bool shouldWork = true;
                int choisenOne = 0;
                do{
                    string readLine =  System.Console.ReadLine();
                    bool isParsed = int.TryParse(readLine, out choisenOne);
                    shouldWork = isParsed ? choisenOne > users.Count : true;
                }
                while(shouldWork);

                updateUser.Identifier = users[choisenOne-1].Identifier;
            }

            if(!string.IsNullOrEmpty(arguments.Identifier)){
                var tmp = Guid.TryParse(arguments.Identifier,out identifier);
                if(!tmp)
                    return new CommandResult("Cannot parse identifier from -i param",false);

                updateUser.Identifier = identifier;
            }
                
            if(!string.IsNullOrEmpty(arguments.Login) && string.IsNullOrEmpty(arguments.Identifier)){
                User tmpUser = new User(){Login = arguments.Login};
                Task<ConnectorResult<Response<IEnumerable<User>>>> userByLogin = WinApiConnector.RequestGet<Response<IEnumerable<User>>>($"{context.ConsoleSettings.ServerAddress}:{context.ConsoleSettings.Port}/Api/User/GetByLogin/"
                                    +$"{arguments.Login}");
                ConnectorResult<Response<IEnumerable<User>>> preResponse = userByLogin.Result;

                if(!preResponse.IsSuccess)
                    return new CommandResult(preResponse.Message, false);
                if(!preResponse.Response.IsSuccess)
                    return new CommandResult(preResponse.Response.Code, false);
                if(!preResponse.Response.Payload.Any())
                    return new CommandResult("The Payload of request is null or empty", false);
                if(preResponse.Response.Payload.Count() != 1)
                    return new CommandResult("There is too many results", true);
                
                updateUser.Identifier = preResponse.Response.Payload.FirstOrDefault().Identifier;           
            }
            
            updateUser.Name = arguments.Name;
            updateUser.Surname = arguments.Surname;
            updateUser.Password = arguments.Password;
            updateUser.Email = arguments.Email;
            Task<ConnectorResult<Response<User>>> result = WinApiConnector.RequestPut<User,Response<User>>($"{context.ConsoleSettings.ServerAddress}:{context.ConsoleSettings.Port}/Api/User/Update",updateUser);
            ConnectorResult<Response<User>> postResponse = result.Result;
            
            if(!postResponse.IsSuccess)
                return new CommandResult(postResponse.Message, false);
            if(!postResponse.Response.IsSuccess)
                return new CommandResult(postResponse.Response.Code, false);
            if(postResponse.Response.Payload == null)
                return new CommandResult("The payload of response is null or empty",false);
            
            return new CommandResult($"The User {updateUser.Identifier} has been updated",true);
        }
    }
    
    public class UserArgumentsExtension: UserArguments
    {
        [Option('i',"identifier", Required=false, HelpText="Identyfikator użytkownika")]
        public string Identifier { get; set; }

        [Option('n',"name", Required = false, HelpText="Imie użytkownika")]
        override public string Name {get;set;}

        [Option('l', "login", Required = false, HelpText = "Login użytkownika.")]
        override public string Login {get;set;}

        [Option('s', "surname", Required = false, HelpText = "Nazwisko użytkownika.")]
        override public string Surname {get;set;}

        [Option('p', "password", Required = false, HelpText = "Hasło użytkownika.")]
        override public string Password {get;set;}

        [Option('e', "email", Required = false, HelpText = "Email użytkownika.")]
        override public string Email {get;set;}

        [Option('t',"test", Required = false, HelpText = "Parametr testowy")]
        public string TestParam {get;set;}
    }
}