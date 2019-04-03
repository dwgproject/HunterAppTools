namespace HuntingAppSupport.Infrastructure{
    public interface ICommand{
        string Description {get;}
        CommandResult Execute(ContextApplication context);
    }
}