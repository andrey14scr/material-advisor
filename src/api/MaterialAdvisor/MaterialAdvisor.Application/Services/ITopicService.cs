using MaterialAdvisor.Application.Models.Editable;

namespace MaterialAdvisor.Application.Services;

public interface ITopicService
{
    Task<EditableTopic> Create(EditableTopic topicModel);

    Task<bool> Delete(Guid topicId);

    Task<EditableTopic> Get(Guid topicId);

    Task<EditableTopic> Update(EditableTopic topicModel);
}
