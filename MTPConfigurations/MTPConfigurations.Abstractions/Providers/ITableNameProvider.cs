namespace MTPConfigurations.Abstractions.Providers
{
    public interface ITableNameProvider
    {
        string GetTableName(object entity);
    }
}