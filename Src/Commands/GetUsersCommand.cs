using System.Collections.Generic;
using System.Linq;
using System.Text;
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

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("------------------------------------------------------------------------------------------------");
            sb.AppendLine("| Identifier       | Name             | Surname          | Login            | Password         |");
            sb.AppendLine("------------------------------------------------------------------------------------------------");

            foreach(var user in connectorResult.Response.Payload)
            {
                sb.AppendLine($"|{PrepareString(user.Identifier.ToString())}|{PrepareString(user.Name)}|{PrepareString(user.Surname)}|{PrepareString(user.Login)}|{PrepareString(user.Password)}|");
                sb.AppendLine("------------------------------------------------------------------------------------------------");
            }

            return new CommandResult(sb.ToString(), true);
        }
        private string PrepareString(string value)
        {
            if (string.IsNullOrEmpty(value))
                return "                  ";
            string temp = value.PadLeft(value.Length + 1);
            if (temp.Length > RequredStringLength)
            {
                temp = string.Concat(temp.Substring(0, RequredStringLength), "...");
            }
            return  temp.PadRight(RequredStringLength + 4);
        }
    }
}