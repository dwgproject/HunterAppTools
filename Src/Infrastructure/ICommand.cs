namespace HuntingAppSupport.Infrastructure{
    public interface ICommand{
        string Description {get; set;}
        CommandResult Execute(ContextApplication context);
    }
}