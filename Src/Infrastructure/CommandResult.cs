namespace HuntingAppSupport.Infrastructure{

    public class CommandResult{
        public CommandResult()
        {

        }

        public CommandResult(string message)
        {
            this.Message = message;
        }

        public string Message { get; private set; }
    }
}