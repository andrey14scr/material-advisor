namespace MaterialAdvisor.AI;

public interface IMaterialAdvisorAIAssistant
{
    Task<string> CallAssistant(string filename, string prompt, string assistantId);
}