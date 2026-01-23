namespace IMS.WebApi.Common
{
    public class ApiResponse<T>
    {
        public bool Succeeded { get; set; }
        public T? Data { get; set; }
        public List<string>? Errors { get; set; }
        public string Message { get; set; } = default!;

        public static ApiResponse<T> Success(T? data = default, string message = "Success")
            => new() { Succeeded = true, Data = data, Message = message };

        public static ApiResponse<T> Failure(List<string> errors, string message = "Failure")
            => new() { Succeeded = false, Errors = errors, Message = message };
    }
}
