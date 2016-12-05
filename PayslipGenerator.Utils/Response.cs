namespace PayslipGenerator.Utils
{
    public class Response<T>
    {
        public ResponseCode Code { get; private set; }
        public string Message { get; private set; }
        public T Data { get; private set; }

        private Response(ResponseCode code, string message, T data)
        {
            Code = code;
            Message = message;
            Data = data;
        }

        public static Response<T> From(T data)
        {
            return new Response<T>(ResponseCode.Ok, string.Empty, data);
        }

        public static Response<T> Error(string message)
        {
            return new Response<T>(ResponseCode.Error, message, default(T));
        }
    }
}