using System.Collections;
using System.Reflection;
using MTConfigurations.Abstractions.Attributes;
using MTQueries.Abstractions;

namespace MTQueries.SqlServer;

public sealed class DefaultQuerySelectedColumnsProvider<T> : IQuerySelectedColumnsProvider<T>
{
    public Task<string[]> GetColumnsAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        return Task.Run<string[]>(GetColumns, cancellationToken);
    }

    private string[] GetColumns()
    {
        var type = typeof(T);
        return GetColumns(type);
    }

    private string[] GetColumns(Type type)
    {
        var properties = type.GetProperties(
            BindingFlags.Instance 
            | BindingFlags.Public 
            | BindingFlags.DeclaredOnly);

        var columns = new List<string>();

        foreach (var property in properties)
        {
            if (property.GetCustomAttribute<GhostAttribute>() != default)
                continue;
            
            if (property.PropertyType.IsClass)
            {
                if (typeof(String) != property.PropertyType)
                    columns.AddRange(
                        GetColumns(property.PropertyType));
            }

            if (property.PropertyType.IsAssignableTo(typeof(IEnumerable))
                && property.PropertyType.IsGenericType)
            {
                columns.AddRange(
                    GetColumns(property.PropertyType.GetGenericArguments()[0]));
            }

            var column = property.GetCustomAttribute<ColumnAttribute>();

            if (column != default)
            {
                var table = property.DeclaringType!.GetCustomAttribute<TableAttribute>();

                if (table == default)
                    throw new ApplicationException("Table Attribute was not configurated for entity: " + property.DeclaringType!.Name);
                
                columns.Add($"{table.Name}.{column.Name}");
            }
        }
        
        return columns.ToArray();
    }
}