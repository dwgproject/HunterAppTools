using System;
using System.Collections.Generic;
using Gravityzero.Console.Utility.Context;
using Gravityzero.Console.Utility.Infrastructure;
using Gravityzero.Console.Utility.Tools;
using Gravityzero.Console.Utility.Model;
using System.Linq;
using System.Threading.Tasks;

namespace Gravityzero.Console.Utility.Commands
{
    public class DeleteUserCommand : BaseCommand<UserArgumentsExtension>
    {
        public DeleteUserCommand(IList<string> args) : base(args)
        {
        }

        protected override CommandResult Execute(ConsoleContext context, UserArgumentsExtension arguments)
        {
            IList<User> users = new List<User>();
            User deleteUser = new User();
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

                deleteUser.Identifier = users[choisenOne-1].Identifier;
            }

            if(!string.IsNullOrEmpty(arguments.Identifier)){
                var tmp = Guid.TryParse(arguments.Identifier, out identifier);
                if(!tmp)
                    return new CommandResult("Cannot parse identifier from -i param", false);
                
                deleteUser.Identifier = identifier;
            }
            if(!string.IsNullOrEmpty(arguments.Login) && string.IsNullOrEmpty(arguments.Identifier)){
                User tmpUser = new User(){Login = arguments.Login};
                Task<ConnectorResult<Response<IEnumerable<User>>>> userByLogin = WinApiConnector.RequestPost<User, Response<IEnumerable<User>>>($"{context.ConsoleSettings.ServerAddress}:{context.ConsoleSettings.Port}/Api/User/GetByLogin",tmpUser);
                ConnectorResult<Response<IEnumerable<User>>> preResponse = userByLogin.Result;

                if(!preResponse.IsSuccess)
                    return new CommandResult(preResponse.Message, false);
                if(!preResponse.Response.IsSuccess)
                    return new CommandResult(preResponse.Response.Code, false);
                if(!preResponse.Response.Payload.Any())
                    return new CommandResult("The Payload of request is null or empty", false);
                if(preResponse.Response.Payload.Count() != 1)
                    return new CommandResult("There is too many results", true);
                
                deleteUser.Identifier = preResponse.Response.Payload.FirstOrDefault().Identifier;
            }

            var result =  WinApiConnector.RequestDelete<Response<string>>($"{context.ConsoleSettings.ServerAddress}:{context.ConsoleSettings.Port}/Api/User/Delete/"+
                            $"{deleteUser.Identifier}");
            ConnectorResult<Response<string>> connectorResult = result.Result;
            if(!connectorResult.IsSuccess)
                return new CommandResult(connectorResult.Message, false);
            if(!connectorResult.Response.IsSuccess)
                return new CommandResult(connectorResult.Response.Code, false);
            if(string.IsNullOrEmpty(connectorResult.Response.Payload))
                return new CommandResult("The payload of response <DELETE USER > is null or empty ",false);
            return new CommandResult($"The User {deleteUser.Identifier} has been deleted",true);
        }
    }
}