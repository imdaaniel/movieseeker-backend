using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Formatters;

using MovieSeeker.Application.Services;

namespace MovieSeeker.API.Filters
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class CustomResponseFilter : ResultFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                var response = new ResponseService();
                response.AddError(errors);

                context.Result = new ObjectResult(response)
                {
                    StatusCode = (int)response.GetStatusCode(),
                    ContentTypes = new MediaTypeCollection { "application/json" }
                };

                base.OnResultExecuting(context);
            }
        }
    }
}