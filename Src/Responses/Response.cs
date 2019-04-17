namespace Gravityzero.Console.Utility.Commands
{
    public class Response<TData>
    {
        public bool IsSuccess { get; set; }
        public string PayloadType { get; set; }
        public TData Payload { get; set; }
        public string Code { get; set; }
    }
}