using System.Net;

using Newtonsoft.Json;

namespace MovieSeeker.API.Middleware
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex}");

                context.Response.ContentType = "application/json";
                
                switch (ex)
                {
                    case InvalidOperationException invalidOperationException:
                        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;

                    case UnauthorizedAccessException unauthorizedAccessException:
                        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        break;

                    default:
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                var jsonResponse = JsonConvert.SerializeObject(new { error = ex.Message });
                
                await context.Response.WriteAsync(jsonResponse);
            }
        }
    }
}