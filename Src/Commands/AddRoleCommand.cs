using System;
using System.Collections.Generic;
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
            Role role = new Role(){ Name = arguments.Name};
            var result = WinApiConnector.RequestPost<Role, Result>("http://localhost:5000/Api/Configuration/AddRole", role);
            return new CommandResult(result.Result.IsSuccess ? "OK." : result.Result.Message);            
        }
    }

    public class RoleArgument
    {
        [Option('n', "name", Required = true, HelpText = "Nazwa roli.")]
        public string Name { get; set; }
    }
    
    public class Result{

    }
}