namespace MaterialAdvisor.Data.Extensions;

public static class EnumConverter
{
    public static IDictionary<int, string> ToDictionary<T>() where T : Enum
    {
        var result = new Dictionary<int, string>();
        var values = Enum.GetValues(typeof(T));

        foreach (var item in values)
        {
            try
            {
                result.Add((int)item, Enum.GetName(typeof(T), item)!);
            }
            catch
            {
                try
                {
                    result.Add((short)item, Enum.GetName(typeof(T), item)!);
                }
                catch
                {
                    try
                    {
                        result.Add((byte)item, Enum.GetName(typeof(T), item)!);
                    }
                    catch
                    {
                        throw;
                    }
                }
            }
        }

        return result;
    }
}