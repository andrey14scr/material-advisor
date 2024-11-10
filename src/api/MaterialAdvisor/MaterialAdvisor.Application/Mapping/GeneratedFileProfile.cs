using AutoMapper;

using MaterialAdvisor.Application.Models.KnowledgeChecks;
using MaterialAdvisor.Data.Entities;

namespace MaterialAdvisor.Application.Mapping;

public class GeneratedFileProfile : Profile
{
    public GeneratedFileProfile()
    {
        CreateMap<GeneratedFile, GeneratedFileEntity>().ReverseMap();
    }
}