using MaterialAdvisor.Application.TableGenerator;

namespace MaterialAdvisor.Application.QueueStorage.Messages;

public class GenerateTablesMessage : QueueMessage
{
    public List<TableGenerationParameter> TableGenerationParameters { get; set; } = [];
}