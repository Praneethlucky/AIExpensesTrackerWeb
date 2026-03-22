using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTracker.BusinessLogic.Exceptions
{
    public class DuplicateException : ApiException
    {
        public DuplicateException(string message)
            : base(message, StatusCodes.Status409Conflict,"DUPLICATE")
        {
        }
    }
}
