using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTracker.BusinessLogic.Exceptions
{
    public class UserInActiveException : ApiException
    {
        public UserInActiveException(string email)
            : base($"User with email '{email}' is inactive. Please contact support.",
                   StatusCodes.Status403Forbidden,
                   "USER_INACTIVE")
        {
        }
    }
}
