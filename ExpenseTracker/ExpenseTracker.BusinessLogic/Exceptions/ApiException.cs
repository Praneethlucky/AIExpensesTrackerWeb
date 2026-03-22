using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace ExpenseTracker.BusinessLogic.Exceptions
{
    public class ApiException : Exception
    {
        public int StatusCode { get; }

        public string ErrorCode { get; }

        public ApiException(string message,
                            int statusCode = StatusCodes.Status400BadRequest,
                            string errorCode = null)
            : base(message)
        {
            StatusCode = statusCode;
            ErrorCode = errorCode;
        }
    }   
}
