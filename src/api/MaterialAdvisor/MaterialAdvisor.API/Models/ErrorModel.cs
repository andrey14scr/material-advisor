using MaterialAdvisor.Application.Exceptions;

namespace MaterialAdvisor.API.Models;

public class ErrorModel
{
    public string Message { get; set; } = null!;

    public string CorrelationId { get; set; } = null!;

    public IEnumerable<ErrorCode> Codes { get; set; } = [];
}
