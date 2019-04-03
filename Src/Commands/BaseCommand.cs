using System.Collections.Generic;
using CommandLine;
using HuntingAppSupport.Infrastructure;
using System.Linq;

namespace HuntingAppSupport.Commands{

    public abstract class BaseCommand<TArgs> : ICommand
    {
        private TArgs arguments;
        private IEnumerable<Error> errors;
        public BaseCommand(object [] args)
        {
            string [] @params = args.Cast<string>().ToArray();
            errors = new List<Error>();
            Parser.Default.ParseArguments<TArgs>(@params)
                                .WithParsed<TArgs>((o) => arguments = o)
                                 .WithNotParsed((errs) => errors = errs);

        }

        public string Description { get; set; }

        public CommandResult Execute(ContextApplication context)
        {
            if (errors.Count() == 0){
                return Execute(context, arguments);

            }else{
                
            }
                

            return new CommandResult("Problem with parsing arguments.");
        }

        protected abstract CommandResult Execute(ContextApplication context, TArgs arguments);
    }
}