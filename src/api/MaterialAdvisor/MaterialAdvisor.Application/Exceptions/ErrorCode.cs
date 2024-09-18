namespace MaterialAdvisor.Application.Exceptions;

public enum ErrorCode
{
    CannotChangeSubmittedAttempt = 1001,

    CannotAnswerForAnotherUser = 2001,
    CannotCreateMoreThanMaxAttempts = 2002,
    CannotAnswerAfterEndDate = 2003,
    TopicVersionIsOutdated = 2004,
}