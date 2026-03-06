namespace ExpenseTracker.Domain.Interfaces;

using ExpenseTracker.Domain.Entities;

public interface IBillRepository
{
    Task<bool> InsertAsync(Bill bill);
    Task<List<Bill>> GetActiveBillsAsync(int userId);
    Task<List<Bill>> GetMonthlyBillsAsync(int userId, int year, int month);
    Task<bool> DeleteAsync(int billId, int userId);
}