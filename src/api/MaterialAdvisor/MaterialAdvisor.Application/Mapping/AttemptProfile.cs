using AutoMapper;

using MaterialAdvisor.Application.Models.Shared;
using MaterialAdvisor.Data.Entities;

namespace MaterialAdvisor.Application.Mapping;

public class AttemptProfile : Profile
{
    public AttemptProfile()
    {
        CreateMap<Attempt, AttemptEntity>().ReverseMap();
    }
}