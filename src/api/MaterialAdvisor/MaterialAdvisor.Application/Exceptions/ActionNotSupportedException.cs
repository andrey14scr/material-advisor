namespace MaterialAdvisor.Application.Exceptions;

public class ActionNotSupportedException : Exception
{
    public IEnumerable<ErrorCode> ErrorCodes { get; set; } = [];

    public ActionNotSupportedException(ErrorCode errorCode)
    {
        ErrorCodes = [errorCode];
    }

    public ActionNotSupportedException()
    {
        
    }
}
