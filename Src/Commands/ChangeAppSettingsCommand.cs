using System;
using System.Collections.Generic;
using CommandLine;
using Gravityzero.Console.Utility.Context;
using Gravityzero.Console.Utility.Infrastructure;
using Gravityzero.Console.Utility.Tools;

namespace Gravityzero.Console.Utility.Commands
{
    public class ChangeAppSettingsCommand : BaseCommand<SettingsArguments>
    {
        public ChangeAppSettingsCommand(IList<string> args) : base(args)
        {
        }

        protected override CommandResult Execute(ConsoleContext context, SettingsArguments arguments)
        {
            IDictionary<string,object> settings = new Dictionary<string,object>();
            settings.Add("address",arguments.Server);
            settings.Add("port", arguments.Port);
            var configuration = new AppConfiguration(settings);
            configuration.UpdateConfiguration(settings);
            return new CommandResult("OK");
        }
    }

    public class SettingsArguments
    {
        [Option('s',"server", Required = true, HelpText = "Adres serwera")]
        public string Server { get; set; }

        [Option('p',"port", Required = true, HelpText = "Port")]
        public string Port { get; set; }
    }
}