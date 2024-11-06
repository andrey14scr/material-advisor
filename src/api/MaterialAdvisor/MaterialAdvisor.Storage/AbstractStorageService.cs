namespace MaterialAdvisor.Storage;

public abstract class AbstractStorageService
{
    private const string Delimiter = "_";
    private string Prefix => Guid.NewGuid().ToString();

    protected string GetUniqueFileName(string name)
    {
        return $"{Prefix}{Delimiter}{name}";
    }

    protected string GetOriginalFileName(string name)
    {
        var fullPrefixLength = Prefix.Length + Delimiter.Length;

        if (name.Length > fullPrefixLength)
        {
            return name.Substring(fullPrefixLength);
        }

        return name;
    }
}