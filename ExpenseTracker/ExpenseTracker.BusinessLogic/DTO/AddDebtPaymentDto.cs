using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTracker.BusinessLogic.DTO
{
    public class AddDebtPaymentDto
    {
        public int DebtId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
    }
}
