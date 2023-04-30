namespace MTConfigurations.Abstractions.Discoveries
{
    public interface IKeyColumnDiscovery
    {
        Column Discovery(object entity);
    }
}