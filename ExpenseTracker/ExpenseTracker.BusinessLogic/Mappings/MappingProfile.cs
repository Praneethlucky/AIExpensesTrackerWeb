using AutoMapper;
using ExpenseTracker.Domain.Entities;

namespace ExpenseTracker.BusinessLogic.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Bill mappings
        CreateMap<BillCreateDto, Bill>();
        CreateMap<Bill, BillResponseDto>();
        CreateMap<Bill, BillCreateDto>();

        // User mappings
        CreateMap<User, LoginResponseDto>();

        // Add more mappings here as project grows
    }
}