namespace jkdmyrs.Extensions.Functions.Exceptions
{
    using System;

    public class FunctionFailedException : Exception
    {
        public int StatusCode { get; private set; }

        public FunctionFailedException(int statusCode, string message)
            : base(message)
        {
            StatusCode = statusCode;
        }

        public FunctionFailedException(int statusCode, string message, Exception innerEx)
            : base(message, innerEx)
        {
            StatusCode = statusCode;
        }
    }
}
