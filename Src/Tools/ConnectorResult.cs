namespace Gravityzero.Console.Utility.Tools{

    public class ConnectorResult<TResult>  where TResult : new()
    {
        public bool IsSuccess { get; private set; }
        public string Message { get; private set; }
        public TResult Result { get; private set;}

        public ConnectorResult(TResult result)
        {
            IsSuccess = true;
            Result = result;
        }

        public ConnectorResult(string message)
        {
            Init();
            Message = message;

        }
        public ConnectorResult()
        {
            Init();
        }

        private void Init(){
            IsSuccess = false;
            Result = new TResult();
        }





        
    }
}