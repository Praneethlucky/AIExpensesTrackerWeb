using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTracker.Domain.Entities
{
    public class DebtSummary
    {
        public decimal GivenPending { get; set; }

        public decimal TakenPending { get; set; }

        public decimal Net => TakenPending - GivenPending;
    }
}
