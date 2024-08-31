namespace MaterialAdvisor.Application.Services;

public interface IAttemptService
{
    Task<TModel> Create<TModel>(TModel model);

    Task<bool> SetIsSubmit(Guid id);
}