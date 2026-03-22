using AutoMapper;
using ExpenseTracker.API.Security;
using ExpenseTracker.BusinessLogic.DTO;
using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Infrastructure.Entities;

namespace ExpenseTracker.BusinessLogic.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Bill mappings
        CreateMap<Bill, BillResponseDto>();

        // User mappings
        CreateMap<User, LoginResponseDto>();
        CreateMap<User, UserProfileResponseDTO>();
        CreateMap<CreateCategoryRequest, Category>();
        CreateMap<CategoryDto, Category>();
        CreateMap<Category, CategoryDto>();
        CreateMap<Category, CreateCategoryRequest>();
        CreateMap<RegisterUserRequestDTO, User>()
            .ForMember(dest => dest.PasswordHash,
                opt => opt.MapFrom(src => PasswordHasher.Hash(src.Password)));
        // Add more mappings here as project grows
    }
}