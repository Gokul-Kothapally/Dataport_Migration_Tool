using System.Net;

namespace ArchivingTool.Model.Arms
{
    public class CommonResponse<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string url { get; set; }

        public static CommonResponse<T> CreateError(HttpStatusCode statusCode, string? message = null) => new()
        { Success = false, Message = message, StatusCode = statusCode };
    }
}
