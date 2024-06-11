﻿using Microsoft.AspNetCore.Mvc;

namespace api_csharp_uplink.DirException
{
    public class ErrorManager : ControllerBase
    {
        public static IActionResult HandleError(Exception exception)
        {
            return exception switch
            {
                BusNotFoundException => new NotFoundObjectResult(exception.Message),
                BusAlreadyCreateException => new ConflictObjectResult(exception.Message),
                DbException => new ObjectResult(new ProblemDetails
                {
                    Detail = exception.Message, Status = 500, Title = "Error DB."
                }),
                _ => new ObjectResult(new ProblemDetails
                {
                    Detail = exception.Message,
                    Status = 500,
                    Title = "An error occurred while processing your request."
                })
            };
        }
    }
}