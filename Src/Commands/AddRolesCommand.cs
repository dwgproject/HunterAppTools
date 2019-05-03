using System;
using System.Collections.Generic;
using CommandLine;
using Gravityzero.Console.Utility.Context;
using Gravityzero.Console.Utility.Infrastructure;
using Gravityzero.Console.Utility.Logger;
using Gravityzero.Console.Utility.Model;
using Gravityzero.Console.Utility.Tools;
using log4net;

namespace Gravityzero.Console.Utility.Commands
{
    public class AddRolesCommand : BaseCommand<RolesArguments>
    {
        private readonly ILog log = LogManager.GetLogger(typeof(AddRolesCommand));
        public AddRolesCommand(IList<string> args) : base(args)
        {
            LoggerConfiguration.LoadConfiguration();
        }

        protected override CommandResult Execute(ConsoleContext context, RolesArguments arguments)
        {
            
            if(String.IsNullOrEmpty(arguments.Path)){
                System.Console.WriteLine($"Path {arguments.Path} is empty");
                return new CommandResult();}
            
            var file = new CsvReader<Role>();
            var listRoles = file.LoadFile(arguments.Path); 
            int successIndex=0;
                
            foreach(var role in listRoles){
                var result = WinApiConnector.RequestPost<Role, Response<Role>>("http://localhost:5000/Api/Configuration/AddRole", role);
                if(result.Result.Result.IsSuccess){
                    successIndex++;
                }
                else{
                    log.Error($"{result.Result.Message}");  
                }                                
            }
            System.Console.WriteLine($"Dodano obiektów: {successIndex}");
            return new CommandResult(successIndex==listRoles.Count ? "Ok": "Error. Check log file!");           
        }
    }

    public class RolesArguments
    {
        [Option('p', "path", Required=true, HelpText="Ścieżka do pliku z danymi")]
        public string  Path { get; set; }
    }
}