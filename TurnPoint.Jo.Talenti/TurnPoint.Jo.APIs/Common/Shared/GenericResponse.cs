namespace TurnPoint.Jo.APIs.Common.Shared
{
    public class GenericResponse<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
    }
}
