using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTracker.BusinessLogic.Exceptions
{
    public class InvalidCredentialsException :ApiException
    {
        public InvalidCredentialsException() : base("Invalid username or password.", StatusCodes.Status401Unauthorized,
                   "INVALID_CREDENTIALS")
        {
        }
       
    }
}
