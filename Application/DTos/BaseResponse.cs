namespace Application.DTos
{
    public class BaseResponse<T> where T : class
    {
        public bool IsSuccess { get; init; }
        public T Data { get; init; }
        public string? ErrorMessage { get; init; }
    }
}
