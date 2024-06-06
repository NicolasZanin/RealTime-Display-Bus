using Microsoft.AspNetCore.Mvc;

namespace api_csharp_uplink.DirException
{
    public class ErrorManager : ControllerBase
    {
        public static IActionResult HandleError(Exception exception)
        {

            if (exception is BusNotFoundException)
            {
                return new NotFoundObjectResult(exception.Message);
            }
            if (exception is BusAlreadyCreateException)
            {
                return new ConflictObjectResult(exception.Message);
            }
            else
            {
                return new ObjectResult(new ProblemDetails {
                    Detail = exception?.Message,
                    Status = 500,
                    Title = "An error occurred while processing your request."
                });
            }
        }
    }
}
