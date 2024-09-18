namespace MaterialAdvisor.Data.Entities.Anstraction;

public interface IVersion
{
    public Guid PersistentId { get; set; }

    public uint Version { get; set; }
}