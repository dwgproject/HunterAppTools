using HuntingAppSupport.Infrastructure;

namespace HuntingAppSupport.Commands{

    public class BackCommand : ICommand
    {
        public string Description { get; set; }

        public CommandResult Execute(ContextApplication context)
        {
            context.PopDirectory();
            return new CommandResult();
        }
    }


}