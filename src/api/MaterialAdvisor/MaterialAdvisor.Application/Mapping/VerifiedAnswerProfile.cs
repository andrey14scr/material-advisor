using AutoMapper;

using MaterialAdvisor.Application.Models.KnowledgeChecks;
using MaterialAdvisor.Data.Entities;

namespace MaterialAdvisor.Application.Mapping;

public class VerifiedAnswerProfile : Profile
{
    public VerifiedAnswerProfile()
    {
        CreateMap<VerifiedAnswerEntity, VerifiedAnswer>().ReverseMap();
    }
}