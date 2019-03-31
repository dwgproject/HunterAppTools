using HuntingAppSupport.Infrastructure;

namespace HuntingAppSupport.Commands{
    public class ExitCommand : ICommand
    {
        public ExitCommand()
        {
            
        }

        public string Description { get ; set; } = "Komenda zamyka aplikację.";

        public CommandResult Execute(ContextApplication context)
        {
            context.ShouldWork = false;
            return new CommandResult();
        }
    }
}