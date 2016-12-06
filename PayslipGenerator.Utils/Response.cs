namespace PayslipGenerator.Utils
{
    /// <summary>
    /// A utility class to represent a result from a function.
    /// Use this class to return the result instead of the just the result itself because it
    /// can hold more information about the result as well.
    /// </summary>
    /// <typeparam name="T">The data returned by the function</typeparam>
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

        /// <summary>
        /// some helper methods to easily create a result object
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
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