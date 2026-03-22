using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTracker.BusinessLogic.Exceptions
{
    public class UserDoesNotExistsException : ApiException
    {
        public UserDoesNotExistsException(string email)
            : base($"User with email '{email}' doesnot exists",
                   StatusCodes.Status404NotFound,
                   "USER_NOT_FOUND")
        {
        }
    }
}
