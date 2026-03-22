using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTracker.BusinessLogic.Exceptions
{
    public class SystemCategoryException : ApiException
    {
        public SystemCategoryException(string message) : base($"Cannot delete {message}. It is default category", StatusCodes.Status401Unauthorized,
                   "SYSTEM_CATEGORY")
        {
        }
    }
}
