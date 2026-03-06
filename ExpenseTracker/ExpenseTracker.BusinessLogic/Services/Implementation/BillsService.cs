using AutoMapper;
using ExpenseTracker.Domain.Interfaces;
using ExpenseTracker.BusinessLogic.Interfaces;

public class BillService : IBillService
{
    private readonly IBillRepository _repository;
    private readonly IMapper _mapper;

    public BillService(IBillRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<BillResponseDto> CreateBillAsync(BillCreateDto dto)
    {
        var entity = _mapper.Map<ExpenseTracker.Domain.Entities.Bill>(dto);

        await _repository.InsertAsync(entity);

        return _mapper.Map<BillResponseDto>(entity);
    }

    public async Task<List<BillCreateDto>> GetUserBillsAsync(int userId)
    {
        var bills = await _repository.GetActiveBillsAsync(userId);

        return bills.Select(b => _mapper.Map<BillCreateDto>(b)).ToList();
    }

    public async Task<bool> DeleteBillAsync(int billId, int userId)
    {
        return await _repository.DeleteAsync(billId, userId);
    }
}