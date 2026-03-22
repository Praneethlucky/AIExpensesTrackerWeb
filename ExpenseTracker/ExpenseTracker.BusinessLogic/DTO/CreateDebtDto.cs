using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTracker.BusinessLogic.DTO
{
    public class CreateDebtDto
    {
        public string PersonName { get; set; }
        public string Type { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
    }   
}
