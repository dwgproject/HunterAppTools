using HuntingAppSupport.Infrastructure;

namespace HuntingAppSupport.Commands{
    public class DummyCommand : ICommand
    {
        public string Description { get; set; }

        public CommandResult Execute(ContextApplication context)
        {
            return new CommandResult("Unknown command.");
        }
    }
}