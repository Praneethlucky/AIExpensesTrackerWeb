using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTracker.BusinessLogic.Exceptions
{
    public class CategoryNotFoundException : ApiException
    {
        public CategoryNotFoundException() : base("Category Not Found", StatusCodes.Status404NotFound,
                   "CATEGORY_NOT_FOUND")
        {
        }
    }
}
