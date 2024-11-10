namespace MaterialAdvisor.Application.Services.Abstraction;

public interface IGeneratedFileService
{
    Task<IList<TModel>> GetByKnowledgeCheckId<TModel>(Guid id);

    Task<TModel> Get<TModel>(Guid id);

    Task<TModel> AddPreGeneratedFile<TModel>(Guid knowledgeCheckId);
}