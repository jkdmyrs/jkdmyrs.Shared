namespace jkdmyrs.Extensions.Functions.Extensions
{
    using jkdmyrs.Extensions.Functions.Exceptions;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using System;

    public static class ExceptionExtensions
    {
        public static IActionResult ToErrorResponse(this Exception ex)
        {
            if (ex is FunctionFailedException funcFailed)
            {
                return new ContentResult
                {
                    StatusCode = funcFailed.StatusCode,
                    Content = funcFailed.Message,
                    ContentType = "text/plain"
                };
            }
            else
            {
                return new ContentResult
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Content = ex.Message,
                    ContentType = "text/plain"
                };
            }
        }
    }
}
