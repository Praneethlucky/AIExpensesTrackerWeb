using ExpenseTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTracker.Domain.Interfaces
{
    public interface IOccurrenceRepository
    {
        Task<bool> Exists(int billId, DateTime dueDate);

        Task InsertOccurrence(BillOccurrence occ);

        Task<List<BillOccurrence>> GetMonthOccurrences(int userId, int year, int month);
    }
}
