namespace MaterialAdvisor.Application.Quartz.Configuration.Options;

public class VerifyKnowledgeCheckJobOptions
{
    public int MaxAnswersSize { get; set; }

    public int AnswersInBulk { get; set; }

    public string Cron { get; set; } = null!;
}
