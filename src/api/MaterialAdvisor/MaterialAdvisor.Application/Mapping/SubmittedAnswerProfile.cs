using AutoMapper;
using MaterialAdvisor.Application.Models.KnowledgeChecks;
using MaterialAdvisor.Data.Entities;

namespace MaterialAdvisor.Application.Mapping;

public class SubmittedAnswerProfile : Profile
{
    public SubmittedAnswerProfile()
    {
        CreateMap<SubmittedAnswerEntity, SubmittedAnswer>().ReverseMap();
    }
}
