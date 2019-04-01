using CommandLine;
using HuntingAppSupport.Infrastructure;

namespace HuntingAppSupport.Commands{

    public abstract class BaseCommand<TArgs> : ICommand
    {
        private readonly string [] args;
        private TArgs arguments;
        public BaseCommand(string [] args)
        {
            Parser.Default.ParseArguments<TArgs>(args)
                                .WithParsed<TArgs>((o) => arguments = o)
                                 .WithNotParsed((errs) => {});

        }

        public string Description { get; set; }

        public CommandResult Execute(ContextApplication context)
        {
            return Execute(context, arguments);
        }

        protected abstract CommandResult Execute(ContextApplication context, TArgs arguments);
    }
}