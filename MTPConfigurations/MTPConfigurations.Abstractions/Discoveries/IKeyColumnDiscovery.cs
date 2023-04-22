namespace MTPConfigurations.Abstractions.Discoveries
{
    public interface IKeyColumnDiscovery
    {
        Column Discovery(object entity);
    }
}