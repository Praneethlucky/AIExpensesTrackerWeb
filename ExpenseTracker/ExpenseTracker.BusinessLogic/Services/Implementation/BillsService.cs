using ExpenseTracker.BusinessLogic.Common;
using ExpenseTracker.BusinessLogic.DTOs.Bills;
using ExpenseTracker.BusinessLogic.Interfaces;
using ExpenseTracker.Domain.DTOs.Bills;
using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Infrastructure.Entities;
using ExpenseTracker.Infrastructure.Helper;
using ExpenseTracker.Infrastructure.Interfaces;

public class BillService : IBillService
{
    private readonly IBillRepository _repo;

    public BillService(IBillRepository repo)
    {
        _repo = repo;
    }

    public async Task<int> CreateAsync(int userId, CreateBillDto dto)
    {
        if (await _repo.ExistsAsync(userId, dto.Name))
            throw new Exception("Duplicate bill");

        RecurrenceHelper.FillRuleValues(dto);

        var bill = new Bill
        {
            UserId = userId,
            Name = dto.Name,
            Amount = dto.Amount,
            PaymentTypeId = dto.PaymentTypeId,
            Frequency = dto.Frequency,
            CategoryId = dto.CategoryId
        };

        var billId = await _repo.InsertBillAsync(bill);
        try
        {
            var nextRunDate = RecurrenceHelper.CalculateNextRunDate(dto);


            var rule = new RecurringRule
            {
                BillId = billId,
                Frequency = dto.Frequency,
                StartDate = dto.StartDate,
                DayOfMonth = dto.DayOfMonth,
                DayOfWeek = dto.DayOfWeek,
                MonthOfYear = dto.MonthOfYear,
                EndDate = dto.EndDate,
                NextRunDate = nextRunDate
            };

            await _repo.InsertRuleAsync(rule);
        }
        catch(Exception e)
        {
            await _repo.DeleteAsync(billId, userId);
        }
        return billId;
    }

    public async Task<List<BillDto>> GetAllAsync(int userId)
    {
        var list = await _repo.GetAllAsync(userId);

        return list.Select(x => new BillDto
        {
            BillId = x.BillId,
            Name = x.Name,
            Amount = x.Amount,
            PaymentTypeId = (int)x.PaymentTypeId,
            CategoryId = x.CategoryId,
            Frequency = x.Frequency
        }).ToList();
    }

    public async Task UpdateAsync(int userId, UpdateBillDto dto)
    {
        await _repo.UpdateAsync(new Bill
        {
            BillId = dto.BillId,
            UserId = userId,
            Name = dto.Name,
            Amount = dto.Amount,
            PaymentTypeId = dto.PaymentTypeId,
            CategoryId = dto.CategoryId,
            Frequency = dto.Frequency
        });
    }

    public async Task DeleteAsync(int userId, int billId)
    {
        await _repo.DeleteAsync(billId, userId);
    }
}