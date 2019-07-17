using System.Collections.Generic;
using System.Linq;
using Gravityzero.Console.Utility.Context;
using Gravityzero.Console.Utility.Infrastructure;
using Gravityzero.Console.Utility.Model;
using Gravityzero.Console.Utility.Tools;

namespace Gravityzero.Console.Utility.Commands
{
    public class GetUsersCommand : ICommand
    {
        private const short RequredStringLength = 14;
        public string Description => "Zwracanie użytkowników";

        public CommandResult Execute(ConsoleContext context)
        {
            var result = WinApiConnector.RequestGet<Response<IEnumerable<User>>>($"{context.ConsoleSettings.ServerAddress}:{context.ConsoleSettings.Port}/Api/User/GetAll");   
            ConnectorResult<Response<IEnumerable<User>>> connectorResult = result.Result;

            if (!connectorResult.IsSuccess)
                return new CommandResult(connectorResult.Message, false);

            if (!connectorResult.Response.IsSuccess)
                return new CommandResult(connectorResult.Response.Code, false);

            if (!connectorResult.Response.Payload.Any())
                return new CommandResult("There are no users.", false);

            TablePresenter presenter = new TablePresenter(new [] {"Identifier", "Name", "Surname", "Login", "Password", "Issued"}, connectorResult.Response.Payload);
            return new CommandResult(presenter.Render(), true);
        }
    }
}