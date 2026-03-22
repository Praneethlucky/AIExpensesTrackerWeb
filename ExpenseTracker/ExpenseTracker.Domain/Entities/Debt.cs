using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTracker.Domain.Entities
{
    public class Debt
    {
        public int DebtId { get; set; }
        public int UserId { get; set; }
        public string PersonName { get; set; }
        public string Type { get; set; }
        public decimal TotalAmount { get; set; }
        public string Description { get; set; }
        public decimal Paid { get; set; }
        public decimal Pending { get; set; }
    }
}
