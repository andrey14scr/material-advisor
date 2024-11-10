namespace MaterialAdvisor.Application.TableGenerator;

public interface ITableGenerator
{
    Task<IEnumerable<TableGenerationResponse>> GenerateTable(IEnumerable<TableGenerationParameter> tableGenerationParameters);
}
