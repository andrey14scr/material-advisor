namespace MaterialAdvisor.Application.Services;

public interface ISubmittedAnswerService
{
    Task<TModel> CreateOrUpdate<TModel>(TModel model);
}
