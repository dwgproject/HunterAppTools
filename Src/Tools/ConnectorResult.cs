namespace Gravityzero.Console.Utility.Tools{

    public class ConnectorResult<TResponse>  where TResponse : new()
    {
        public bool IsSuccess { get; private set; }
        public string Message { get; private set; }
        public TResponse Response { get; private set;}

        public ConnectorResult(TResponse result)
        {
            IsSuccess = true;
            Response = result;
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
            Response = new TResponse();
        }        
    }
}