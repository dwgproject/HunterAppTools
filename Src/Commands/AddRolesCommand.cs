using System.Collections.Generic;
using CommandLine;
using Gravityzero.Console.Utility.Context;
using Gravityzero.Console.Utility.Infrastructure;
using Gravityzero.Console.Utility.Model;
using Gravityzero.Console.Utility.Tools;

namespace Gravityzero.Console.Utility.Commands
{
    public class AddRolesCommand : BaseCommand<RolesArguments>
    {
        public AddRolesCommand(IList<string> args) : base(args)
        {
        }

        protected override CommandResult Execute(ConsoleContext context, RolesArguments arguments)
        {
            System.Console.WriteLine($"Dodaje role dla userów z pliku: {arguments.Path}");
            var file = new CsvReader<Role>();
            if(arguments.Path!=null){
                var listRoles = file.LoadFile(arguments.Path); 
            }
            else{
                System.Console.WriteLine($"Ścieżka nieprawidłowa -{arguments.Path}");
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