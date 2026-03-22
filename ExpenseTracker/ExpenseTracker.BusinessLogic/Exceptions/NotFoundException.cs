using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTracker.BusinessLogic.Exceptions
{
    public class NotFoundException : ApiException
    {
        public NotFoundException(string message)
            : base(message,StatusCodes.Status404NotFound, "NOT_FOUND")
        {
        }
    }
}
