namespace MaterialAdvisor.Application.Exceptions;

public class ActionNotAllowedException : Exception
{
    public IEnumerable<ErrorCode> ErrorCodes { get; set; } = [];

    public ActionNotAllowedException(ErrorCode errorCode)
    {
        ErrorCodes = [errorCode];
    }

    public ActionNotAllowedException()
    {

    }
}
