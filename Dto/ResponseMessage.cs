namespace OnlineBookShop.Dto
{
    public class ResponseMessage
    {
        public long StatusCode { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
        public DateTime? DateTime { get; set; }
        public long? DataCount { get; set; }
        public long? Pages { get; set; }

        public ResponseMessage() { }

        public ResponseMessage(long statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message;
        }

        public ResponseMessage(long statusCode, string message, object data)
        {
            StatusCode = statusCode;
            Message = message;
            Data = data;
        }

        public ResponseMessage(long statusCode, string message, object data, long dataCount)
        {
            StatusCode = statusCode;
            Message = message;
            Data = data;
            DataCount = dataCount;
        }

        public ResponseMessage(long statusCode, string message, object data, long dataCount, long pages)
        {
            StatusCode = statusCode;
            Message = message;
            Data = data;
            DataCount = dataCount;
            Pages = pages;
        }

        public ResponseMessage(long statusCode, string message, DateTime dateTime, long dataCount)
        {
            StatusCode = statusCode;
            Message = message;
            DateTime = dateTime;
            DataCount = dataCount;
        }

        public ResponseMessage(long statusCode, string message, object data, DateTime dateTime, long dataCount)
        {
            StatusCode = statusCode;
            Message = message;
            Data = data;
            DateTime = dateTime;
            DataCount = dataCount;
        }

        public ResponseMessage(long statusCode)
        {
            StatusCode = statusCode;
        }

        public ResponseMessage(string message)
        {
            Message = message;
        }
    }
}
