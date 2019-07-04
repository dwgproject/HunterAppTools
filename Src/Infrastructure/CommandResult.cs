namespace Gravityzero.Console.Utility.Infrastructure
{
    public class CommandResult{
       
        public CommandResult()
        {

        }

        public CommandResult(string message)
        {
            this.Message = message;
            this.IsSuccess = true;
        }
        
        public CommandResult(string message, bool isSuccess)
        {
            this.Message = message;
            this.IsSuccess = isSuccess;
        }

        public string Message { get; private set; }
        public bool IsSuccess { get; private set; }
    }
}
