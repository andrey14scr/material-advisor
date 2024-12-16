using MaterialAdvisor.Application.TableGenerator;
using MaterialAdvisor.QueueStorage;

namespace MaterialAdvisor.Application.QueueStorage.Messages;

public class GenerateTablesMessage : QueueMessage
{
    public List<TableGenerationParameter> TableGenerationParameters { get; set; } = [];
}