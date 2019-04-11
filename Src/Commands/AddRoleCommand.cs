using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using CommandLine;
using HuntingAppSupport;
using HuntingAppSupport.Commands;
using HuntingAppSupport.Infrastructure;
using Newtonsoft.Json;
using Src.Model;
using Src.Tools;

namespace Src.Commands
{
    public class AddRoleCommand : BaseCommand<RoleArguments>
    {
        public AddRoleCommand(IList<string> args) : base(args)
        {
        }

        protected override CommandResult Execute(ContextApplication context, RoleArguments arguments)
        {
            Console.WriteLine($"Dodaje usera. Oto jego dane: {arguments.Name}");
            var file = new CsvReader<Role>();
            var listRoles = file.LoadFile( @"C:\Users\Patryk\Desktop\Role.csv");           
            return new CommandResult();
        }

    }

    public class RoleArguments
    {
        [Option('n', "name", Required=true, HelpText="Nazwa roli")]
        public string  Name { get; set; }
    }
}