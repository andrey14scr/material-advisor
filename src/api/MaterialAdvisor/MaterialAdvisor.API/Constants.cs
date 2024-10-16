namespace MaterialAdvisor.API;

public static class Constants
{
    public static class Headers
    {
        public const string CorrelationIdHeader = "X-Correlation-ID";
    }

    public static class Configuration
    {
        public const string AuthSection = "Auth";
    }

    public static class Entities
    {
        public const string Topic = "topic";
    }

    public static class Actions
    {
        public const string Login = "login";
        public const string Refresh = "refresh";
        public const string Register = "register";
    }
}
