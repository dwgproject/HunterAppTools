using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gravityzero.Console.Utility.Context;
using Gravityzero.Console.Utility.Infrastructure;
using Gravityzero.Console.Utility.Model;
using Gravityzero.Console.Utility.Tools;

namespace Gravityzero.Console.Utility.Commands
{
    public class DeleteRoleCommand : BaseCommand<RoleArgument>
    {
        // W kontrolerze nie może być atrybutu [FromBody] jeżeli parametr identifier przekazuje się w url
        // Brak w kontrolerze metody zwracającej obiekt wg query dlatego należy uzupełnić path przy zmiennej role
        // Do uzyskania obiektu po nazwie użyłem RequestPost, a nie RequestGet
        public DeleteRoleCommand(IList<string> args) : base(args)
        {
        }

        protected override CommandResult Execute(ConsoleContext context, RoleArgument arguments)
        {
            Task<ConnectorResult<Response<IEnumerable<Role>>>> role = WinApiConnector.RequestPost<string, Response<IEnumerable<Role>>>($"{context.ConsoleSettings.ServerAddress}:{context.ConsoleSettings.Port}/Api/Configuration/GetRole",arguments.Name);
            ConnectorResult<Response<IEnumerable<Role>>> preResult = role.Result;
            if(!preResult.IsSuccess)
                return new CommandResult(preResult.Message, false);
            if(!preResult.Response.IsSuccess)
                return new CommandResult(preResult.Response.Code, false);
            if(!preResult.Response.Payload.Any())
                return new CommandResult($"The Role {arguments.Name} doesn't exist in database");

            Task<ConnectorResult<Response<string>>> result = WinApiConnector.RequestDelete<Response<string>>($"{context.ConsoleSettings.ServerAddress}:{context.ConsoleSettings.Port}/Api/Configuration/DeleteRole"+
                            $"/{preResult.Response.Payload.FirstOrDefault().Identifier}");
            ConnectorResult<Response<string>> connectorResult = result.Result;
            if(!connectorResult.IsSuccess)
                return new CommandResult(connectorResult.Message, false);
            if(!connectorResult.Response.IsSuccess)
                return new CommandResult(connectorResult.Response.Code, false);
            if(string.IsNullOrEmpty(connectorResult.Response.Payload))
                return new CommandResult("The payload of response <DELETE USER > is null or empty ",false);
            return new CommandResult($"The User {arguments.Name} has been deleted",true);
        }
    }
}