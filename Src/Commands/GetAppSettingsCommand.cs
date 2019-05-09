using System.Collections.Generic;
using Gravityzero.Console.Utility.Context;
using Gravityzero.Console.Utility.Infrastructure;
using Gravityzero.Console.Utility.Tools;

namespace Gravityzero.Console.Utility.Commands
{
    public class ShowSettingsCommand : ICommand
    {
        public string Description => "Komenda zwracająca aktualne ustawienia aplikacji";
        
        public CommandResult Execute(ConsoleContext context)
        {
            IDictionary<string, object> settings = new Dictionary<string, object>();
            var configuration = new AppConfiguration(settings);
            settings = configuration.LoadConfiguration(PathProvider.SettingsPath());
            foreach(var item in settings){
                System.Console.WriteLine($"{item.Key} - {item.Value}");
            }
            
            return new CommandResult();
        }
    }
}