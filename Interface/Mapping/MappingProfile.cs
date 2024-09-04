using accountSystem.Domain.Entities;
using Application.Dto;
using AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Account, AccountDto>().ReverseMap(); // Map both ways if needed
    }
}
