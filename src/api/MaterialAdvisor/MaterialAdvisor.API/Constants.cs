namespace MaterialAdvisor.API;

public class Constants
{
    public class Headers
    {
        public const string CorrelationIdHeader = "X-Correlation-ID";
    }

    public class Claims
    {
        public const string RolesGroups = "roles_groups";
    }

    public class Configuration
    {
        public const string AuthSection = "Auth";
    }

    public class Entities
    {
        public const string Topic = "topic";
    }

    public class Actions
    {
        public const string Login = "login";
        public const string Refresh = "refresh";
        public const string Register = "register";
    }
}
