using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTracker.BusinessLogic.DTO
{
    public class DebtSummaryDto
    {
        public decimal GivenPending { get; set; }
        public decimal TakenPending { get; set; }

        public decimal Net => TakenPending - GivenPending;
    }
}
