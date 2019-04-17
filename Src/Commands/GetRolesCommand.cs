using System;
using System.Collections.Generic;
using System.Linq;
using Gravityzero.Console.Utility.Commands;
using Gravityzero.Console.Utility.Context;
using Gravityzero.Console.Utility.Infrastructure;
using Gravityzero.Console.Utility.Model;
using Gravityzero.Console.Utility.Tools;

namespace Gravityzero.Console.Utility.Commands
{
    public class GetRolesCommand : ICommand
    {
        public string Description => "Zwracanie listy ról istniejących w bazie";

        public CommandResult Execute(ConsoleContext context)
        {
            List<Role> listRoles = new List<Role>();
            var result = WinApiConnector.RequestGet<String, Response<IEnumerable<Role>>>("http://localhost:5000/Api/Configuration/GetAllRoles","");
            if(result.Result.Result.Payload.Count()>0){
                foreach(var role in result.Result.Result.Payload){
                    System.Console.WriteLine($"{role.Name} -- ({role.Identifier})".PadLeft(70));
                    listRoles.Add(new Role(){Identifier=role.Identifier, Name = role.Name});
                }
                return new CommandResult(result.Result.IsSuccess ? "Ok" : result.Result.Message);
            }           
            return new CommandResult();
        }
    }

}