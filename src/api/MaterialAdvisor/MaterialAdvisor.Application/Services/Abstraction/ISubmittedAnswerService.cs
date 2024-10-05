namespace MaterialAdvisor.Application.Services.Abstraction;

public interface ISubmittedAnswerService
{
    Task<TModel> CreateOrUpdate<TModel>(TModel model);
}
