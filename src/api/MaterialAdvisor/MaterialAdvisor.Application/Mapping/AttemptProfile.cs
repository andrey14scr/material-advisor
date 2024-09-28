using AutoMapper;
using MaterialAdvisor.Application.Models.KnowledgeChecks;
using MaterialAdvisor.Data.Entities;

namespace MaterialAdvisor.Application.Mapping;

public class AttemptProfile : Profile
{
    public AttemptProfile()
    {
        CreateMap<Attempt, AttemptEntity>().ReverseMap();
    }
}