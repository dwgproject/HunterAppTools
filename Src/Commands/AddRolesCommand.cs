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
    public class AddRolesCommand : BaseCommand<RolesArguments>
    {
        public AddRolesCommand(IList<string> args) : base(args)
        {
        }

        protected override CommandResult Execute(ContextApplication context, RolesArguments arguments)
        {
            Console.WriteLine($"Dodaje role dla userów z pliku: {arguments.Path}");
            var file = new CsvReader<Role>();
            if(arguments.Path!=null){
                var listRoles = file.LoadFile(arguments.Path); 
            }
            else{
                Console.WriteLine($"Ścieżka nieprawidłowa -{arguments.Path}");
            }
                      
            return new CommandResult();
        }

    }

    public class RolesArguments
    {
        [Option('p', "path", Required=true, HelpText="Ścieżka do pliku z danymi")]
        public string  Path { get; set; }
    }
}