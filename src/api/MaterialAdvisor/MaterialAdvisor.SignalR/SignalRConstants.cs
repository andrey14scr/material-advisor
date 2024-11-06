namespace MaterialAdvisor.SignalR;

public static class SignalRConstants
{
    public const string TopicGenerationHubName = "/topic-generation-hub";

    public static class Messages
    {
        public const string TopicGeneratedMessage = "TopicGenerated";
        public const string AnswersVerifiedMessage = "AnswersVerified";
    }
}
