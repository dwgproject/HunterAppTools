using System.Collections.Generic;
using CommandLine;
using Gravityzero.Console.Utility.Context;
using Gravityzero.Console.Utility.Infrastructure;

namespace Gravityzero.Console.Utility.Commands
{
    public class AddHuntingCommand : BaseCommand<HuntingArguments>
    {
        public AddHuntingCommand(IList<string> args) : base(args)
        {
        }

        protected override CommandResult Execute(ConsoleContext context, HuntingArguments arguments)
        {
            throw new System.NotImplementedException();
        }
    }

    public class HuntingArguments
    {
        [Option('l', "leader", Required=true, HelpText="Lider polowania")]
        public string Leader { get; set; }
    }
}