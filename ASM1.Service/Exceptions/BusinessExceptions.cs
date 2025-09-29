namespace ASM1.Service.Exceptions
{
    public class BusinessException : Exception
    {
        public List<string> Errors { get; }

        public BusinessException(string message) : base(message)
        {
            Errors = new List<string>();
        }

        public BusinessException(string message, List<string> errors) : base(message)
        {
            Errors = errors ?? new List<string>();
        }

        public BusinessException(string message, string error) : base(message)
        {
            Errors = new List<string> { error };
        }

        public BusinessException(string message, Exception innerException) : base(message, innerException)
        {
            Errors = new List<string>();
        }
    }

    public class ValidationException : BusinessException
    {
        public ValidationException(string message) : base(message)
        {
        }

        public ValidationException(string message, List<string> errors) : base(message, errors)
        {
        }

        public ValidationException(string message, string error) : base(message, error)
        {
        }
    }

    public class NotFoundException : BusinessException
    {
        public NotFoundException(string message) : base(message)
        {
        }

        public NotFoundException(string resource, object key) : base($"{resource} với ID {key} không được tìm thấy")
        {
        }
    }
}