namespace jkdmyrs.Extensions.Functions
{
    using jkdmyrs.Extensions.Functions.Exceptions;
    using jkdmyrs.Extensions.Functions.Extensions;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Threading.Tasks;

    public abstract class BaseFunction<T> where T : class
    {
        private readonly ILogger<T> _logger;

        public BaseFunction(ILogger<T> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IActionResult> Execute(Task<IActionResult> operation)
        {
            try
            {
                return await operation.ConfigureAwait(false);
            }
            catch (FunctionFailedException funcFailed)
            {
                _logger.LogInformation("Function failed. Status Code: {statusCode}, Message: {message}", funcFailed.StatusCode, funcFailed.Message);
                return funcFailed.ToErrorResponse();
            }
            catch (Exception e)
            {
                _logger.LogInformation("Function failed with unknown error. Message: {message}", e.Message);
                return e.ToErrorResponse();
            }
        }
    }
}
