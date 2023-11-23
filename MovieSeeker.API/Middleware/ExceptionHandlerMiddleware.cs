using System.Net;

using MovieSeeker.Application.Services;

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
                Console.WriteLine("ERRO: " + ex.Message);

                var response = new ResponseService<Object>();
                response.AddError(ex.Message);

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int) HttpStatusCode.BadRequest;
                
                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}