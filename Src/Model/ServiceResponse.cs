namespace GravityZero.HuntingSupport.Service.Response
{
    public class ServiceResponse<TData>{

        public bool IsSuccess { get; private set; }
        public string PayloadType { get; private set; }
        public TData Payload { get; private set; }
        public string Code { get; private set; }

        public static ServiceResponse<TData> Create(bool isSuccess, TData payload, string code){
            return new ServiceResponse<TData>(isSuccess, payload, code);
        }
        private ServiceResponse(bool isSuccess, TData payload, string code){
            this.IsSuccess = isSuccess;
            this.Payload = payload;
            this.PayloadType = payload != null ? payload.GetType().Name : "Unknown";
        }
    }
}