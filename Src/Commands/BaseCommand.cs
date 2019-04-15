using System.Collections.Generic;
using CommandLine;
using System.Linq;
using Gravityzero.Console.Utility.Infrastructure;
using Gravityzero.Console.Utility.Context;

namespace Gravityzero.Console.Utility.Commands
{

    public abstract class BaseCommand<TArgs> : ICommand
    {
        private TArgs arguments;
        private IEnumerable<Error> errors;
        public BaseCommand(IList<string> args)
        {
            errors = new List<Error>();
            Parser.Default.ParseArguments<TArgs>(args.ToArray())
                                .WithParsed<TArgs>((o) => arguments = o)
                                 .WithNotParsed((errs) => errors = errs);

        }

        public string Description { get; set; }

        public CommandResult Execute(ConsoleContext context)
        {
            if (errors.Count() == 0){
                return Execute(context, arguments);

            }else{
                
            }
                

            return new CommandResult("Problem with parsing arguments.");
        }

        protected abstract CommandResult Execute(ConsoleContext context, TArgs arguments);
    }
}