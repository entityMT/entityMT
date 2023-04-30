namespace MTConfigurations.Abstractions.Providers
{
    public interface ITableNameProvider
    {
        string GetTableName(object entity);
    }
}