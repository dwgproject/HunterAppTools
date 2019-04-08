using HuntingAppSupport.Infrastructure;

namespace HuntingAppSupport.Commands{
    public class HelpCommand : ICommand
    {
        public string Description { get; set; }

        public CommandResult Execute(ContextApplication context)
        {
        
            return new CommandResult();
        }
    }


}