namespace eVoucher.Handlers
{
    public class ApiResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }
        public object Data { get; set; }
        public long TotalRecords { get; set; }

        public ApiResponse() { }

        public ApiResponse(bool isSuccess, string message) : this(isSuccess, 0, message)
        {

        }

        public ApiResponse(bool isSuccess, int statusCode, string message) : this(isSuccess, statusCode, message, null)
        {

        }

        public ApiResponse(bool isSuccess, int statusCode, string message, object data)
        {
            Success = isSuccess;
            Message = message;
            StatusCode = statusCode;
            Data = data;
        }
    }
}
