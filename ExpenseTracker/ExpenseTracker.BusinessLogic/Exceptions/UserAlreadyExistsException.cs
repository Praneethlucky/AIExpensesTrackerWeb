using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTracker.BusinessLogic.Exceptions
{
    public class UserAlreadyExistsException : ApiException
    {
        public UserAlreadyExistsException(string email)
            : base($"User with email '{email}' already exists",
                   StatusCodes.Status409Conflict,
                   "USER_EXISTS")
        {
        }
    }
}
