namespace Lesson3_CNLTWeb.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.ToString();

            // Chỉ chặn nếu đường dẫn là Book/Detail bị lỗi
            if (path == "/Book/Detail/0" || path == "/Book/Detail/-1")
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("Book id khong hop le");
                return; // Dừng lại, không cho đi tiếp
            }

            // BẮT BUỘC PHẢI CÓ DÒNG NÀY: Cho phép các request (như RoomTypes) đi qua
            await _next(context);
        }
    }
}