using System.Net;
using System.Text.Json;

namespace InventoryApi.Middleware
{
    public class ExceptionHandlingMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            try { await _next(context); }
            catch (Exception ex)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                var result = JsonSerializer.Serialize(new { error = "An unexpected error occurred.", detail = ex.Message });
                await context.Response.WriteAsync(result);
            }
        }
    }
}