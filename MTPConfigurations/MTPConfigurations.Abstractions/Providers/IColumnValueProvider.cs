namespace MTPConfigurations.Abstractions.Providers
{
    public interface IColumnValueProvider
    {
        object GetValue(object entity, Column column);
    }
}