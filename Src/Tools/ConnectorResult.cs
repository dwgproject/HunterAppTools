namespace Src.Tools{

    public class ConnectorResult<TResult>
    {
        public TResult Result { get; private set;}

        public ConnectorResult(TResult result)
        {
            Result = result;
        }   
    }
}