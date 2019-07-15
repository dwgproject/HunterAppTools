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
    public class AddRoleCommand : BaseCommand<RoleArgument>
    {
        public AddRoleCommand(IList<string> args) : base(args)
        {
        }

        protected override CommandResult Execute(ConsoleContext context, RoleArgument arguments)
        {
            Role role = new Role(){ Name = arguments.Name.ToLower()};
            Task<ConnectorResult<Response<IEnumerable<Role>>>> checkIfExist = WinApiConnector.RequestGet<Response<IEnumerable<Role>>>($"{context.ConsoleSettings.ServerAddress}:{context.ConsoleSettings.Port}/Api/Configuration/GetRole/"+
                            $"{arguments.Name.ToLower()}");
            ConnectorResult<Response<IEnumerable<Role>>> preResult = checkIfExist.Result;
            if(!preResult.IsSuccess)
                return new CommandResult(preResult.Message, false);
            if(!preResult.Response.IsSuccess)
                return new CommandResult(preResult.Response.Code, false);
            if(preResult.Response.Payload.Any())
                return new CommandResult($"The Role {arguments.Name} exists in database");

            Task<ConnectorResult<Response<string>>> result = WinApiConnector.RequestPost<Role, Response<string>>($"{context.ConsoleSettings.ServerAddress}:{context.ConsoleSettings.Port}/Api/Configuration/AddRole", role);
            ConnectorResult<Response<string>> connectorResult = result.Result;
            if(!connectorResult.IsSuccess)
                return new CommandResult(connectorResult.Message, false);
            if(!connectorResult.Response.IsSuccess)
                return new CommandResult(connectorResult.Response.Code, false);
            if(connectorResult.Response.Payload == null)
                return new CommandResult("The payload of reguest is null or empty", false);
            return new CommandResult($"The Role {arguments.Name} has been added");            
        }
    }

    public class RoleArgument
    {
        [Option('n', "name", Required = true, HelpText = "Nazwa roli.")]
        public string Name { get; set; }

        [Option('r', "rename", Required = false, HelpText = "Nowa nazwa dla roli.")]
        public string Rename { get; set; }
    }
    
    public class Result{

    }
}