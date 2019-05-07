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
            ConsoleSettings consoleSettings = new ConsoleSettings();
            consoleSettings.ServerAddress = arguments.Address;
            consoleSettings.Port = arguments.Port;
            consoleSettings.Save();
            return new CommandResult("OK");
        }
    }

    public class SettingsArguments
    {
        [Option('a',"address", Required = true, HelpText = "Adres serwera")]
        public string Address { get; set; }

        [Option('p',"port", Required = true, HelpText = "Port")]
        public string Port { get; set; }
    }
}