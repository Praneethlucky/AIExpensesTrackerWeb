using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTracker.BusinessLogic.Exceptions
{
    public class CategoryAlreadyExists:ApiException
    {
        public CategoryAlreadyExists(string categoryName)
            : base($"Category with name '{categoryName}' already exists.",
                   StatusCodes.Status400BadRequest,
                   "CATEGORY_ALREADY_EXISTS")
        {
        }
    }
}
