namespace ASM1.Service.Models
{
    public class ServiceResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public List<string> Errors { get; set; } = new List<string>();

        public static ServiceResponse<T> SuccessResponse(T data, string message = "Thành công")
        {
            return new ServiceResponse<T>
            {
                Success = true,
                Message = message,
                Data = data,
                Errors = new List<string>()
            };
        }

        public static ServiceResponse<T> ErrorResponse(string message, List<string>? errors = null)
        {
            return new ServiceResponse<T>
            {
                Success = false,
                Message = message,
                Data = default(T),
                Errors = errors ?? new List<string>()
            };
        }

        public static ServiceResponse<T> ErrorResponse(string message, string error)
        {
            return new ServiceResponse<T>
            {
                Success = false,
                Message = message,
                Data = default(T),
                Errors = new List<string> { error }
            };
        }
    }

    // Response không có data
    public class ServiceResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<string> Errors { get; set; } = new List<string>();

        public static ServiceResponse SuccessResponse(string message = "Thành công")
        {
            return new ServiceResponse
            {
                Success = true,
                Message = message,
                Errors = new List<string>()
            };
        }

        public static ServiceResponse ErrorResponse(string message, List<string>? errors = null)
        {
            return new ServiceResponse
            {
                Success = false,
                Message = message,
                Errors = errors ?? new List<string>()
            };
        }

        public static ServiceResponse ErrorResponse(string message, string error)
        {
            return new ServiceResponse
            {
                Success = false,
                Message = message,
                Errors = new List<string> { error }
            };
        }
    }
}