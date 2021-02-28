namespace ASPCoreMVC._Commons
{
    public class ResponseWrapper<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int ErrorCode { get; set; }
        public T Data { get; set; }

        public ResponseWrapper<T> SetMessage(string message)
        {
            Message = message;
            return this;
        }

        public ResponseWrapper() { }

        public ResponseWrapper(T data, string message)
        {
            Success = true;
            Message = message;
            ErrorCode = -1;
            Data = data;
        }

        public ResponseWrapper(T data, string message, int errorCode)
        {
            Success = false;
            Message = message;
            ErrorCode = errorCode;
            Data = data;
        }

        public ResponseWrapper<T> SuccessReponseWrapper(T data, string message)
        {
            return new ResponseWrapper<T>(data, message);
        }
        public ResponseWrapper<T> ErrorReponseWrapper(T data, string message, int errorCode)
        {
            return new ResponseWrapper<T>(data, message, errorCode);
        }
    }
}
