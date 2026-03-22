using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTracker.BusinessLogic.DTO
{
    public class CreateCategoryRequest
    {
        public string Name { get; set; }

        public string Type { get; set; }

        public string Icon { get; set; }

        public string Color { get; set; }
    }
}
