using System;
using System.Collections.Generic;
using CommandLine;
using HuntingAppSupport.Infrastructure;
using Src.Tools;

namespace HuntingAppSupport.Commands
{

    public class AddRoleCommand : BaseCommand<RoleArgument>
    {
        public AddRoleCommand(IList<string> args) : base(args)
        {
        }

        protected override CommandResult Execute(ContextApplication context, RoleArgument arguments)
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

    public class Role
    {
        public Guid Identifier {get; set;} = Guid.Empty;
        public string Name { get; set; }
    }

    public class Result{

    }
}