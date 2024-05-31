using api_csharp_uplink.DirException;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace api_csharp_uplink.Controllers
{
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("error")]
    public class ErrorController : ControllerBase
    {
        public IActionResult HandleError()
        {
            var feature = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var exception = feature?.Error;

            Console.WriteLine("%s %s", exception is BusAlreadyCreateException, exception.Message);

            if (exception is BusNotFoundException)
            {
                return NotFound(exception.Message);
            }
            if (exception is BusAlreadyCreateException)
            {
                return Conflict(exception.Message);
            }
            else
            {
                return Problem(
                    detail: exception?.Message,
                    statusCode: 500,
                    title: "An error occurred while processing your request.");
            }
        }
    }
}
