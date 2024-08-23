using MaterialAdvisor.Application.Models.Editable;

namespace MaterialAdvisor.Application.Services;

public interface ITopicService
{
    Task<EditableTopic> Create(EditableTopic topicModel);

    Task<EditableTopic> Update(EditableTopic topicModel);
}
