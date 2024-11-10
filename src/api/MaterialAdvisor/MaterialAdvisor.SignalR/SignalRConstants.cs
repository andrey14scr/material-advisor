namespace MaterialAdvisor.SignalR;

public static class SignalRConstants
{
    public const string TopicGenerationHubName = "/topic-generation-hub";
    public const string AnswerVerificationHubName = "/answer-verification-hub";
    public const string ReportGenerationHubName = "/report-generation-hub";

    public static class Messages
    {
        public const string TopicGeneratedMessage = "TopicGenerated";
        public const string AnswersVerifiedMessage = "AnswersVerified";
        public const string ReportGeneratedMessage = "ReportGenerated";
    }
}
