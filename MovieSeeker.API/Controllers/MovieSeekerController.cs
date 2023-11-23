using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;

using MovieSeeker.API.Filters;
using MovieSeeker.Application.Services;

namespace MovieSeeker.API.Controllers
{
    [CustomResponseFilter]
    [ApiController]
    [Authorize]
    public abstract class MovieSeekerController : ControllerBase
    {
        protected internal IActionResult Response(ResponseService response) {
            return new ObjectResult(response)
            {
                StatusCode = (int)response.GetStatusCode(),
                ContentTypes = new MediaTypeCollection { "application/json" }
            };
        }
    }
}