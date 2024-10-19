namespace MaterialAdvisor.Application.AI;

public interface IMaterialAdvisorAIAssistant
{
    Task<string> GenerateQuestions(string filename, string prompt);
}