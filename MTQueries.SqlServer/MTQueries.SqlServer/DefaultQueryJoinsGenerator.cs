using System.Collections;
using System.Reflection;
using MTPConfigurations.Abstractions.Attributes;
using MTPConfigurations.Abstractions.Enums;
using MTQueries.Abstractions;

namespace MTQueries.SqlServer;

public sealed class DefaultQueryJoinsGenerator<T> : IQueryJoinsGenerator<T>
{
    public Task<string[]> GetJoinsAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        return Task.Run<string[]>(GetJoins, cancellationToken);
    }

    private string[] GetJoins()
    {
        var type = typeof(T);
        return GetJoins(type);
    }

    private string[] GetJoins(Type type)
    {
        var properties = type.GetProperties(
            BindingFlags.Instance 
            | BindingFlags.Public 
            | BindingFlags.DeclaredOnly);

        var joins = new List<string>();

        foreach (var property in properties)
        {
            if ((property.PropertyType.IsClass
                && property.PropertyType != typeof(string)) || 
                (property.PropertyType.IsAssignableTo(typeof(IEnumerable))
                && property.PropertyType.IsGenericType))
            {
                var foreignKeyProperty = this.GetForeignKeyProperty(property, properties);
                var sourceForeignKeyColumn = foreignKeyProperty.GetCustomAttribute<ColumnAttribute>();
                var targetPrimaryKeyProperty = this.GetTargetPrimaryKeyProperty(foreignKeyProperty);
                var targetPrimaryKeyColumn = targetPrimaryKeyProperty.GetCustomAttribute<ColumnAttribute>();
                var joinType = this.GetJoinType(foreignKeyProperty);
                var sourceTable = type.GetCustomAttribute<TableAttribute>();
                var targetTable = targetPrimaryKeyProperty.DeclaringType!.GetCustomAttribute<TableAttribute>();

                joins.Add(
                    $"{joinType} {targetTable!.Name} ON {targetTable.Name}.{targetPrimaryKeyColumn!.Name} = {sourceTable}.{sourceForeignKeyColumn!.Name}");

                var nestedJoins = GetJoins(property.PropertyType);
                
                if(nestedJoins.Any())
                    joins.AddRange(nestedJoins);
            }
        }
        
        return joins.ToArray();
    }

    private PropertyInfo GetForeignKeyProperty(PropertyInfo property, PropertyInfo[] properties)
    {
        Type entityType = property.PropertyType;
        
        if(property.PropertyType.IsGenericType)
         entityType = property.PropertyType.GenericTypeArguments[0];
        
        var foreignKeyProperty = properties.FirstOrDefault(
            p =>
                p.GetCustomAttribute<ForeignKeyAttribute>()
                    ?.DestinationType == entityType != default);

        if (foreignKeyProperty == default)
            throw new ApplicationException($"Reference property {property.Name} does not have corresponding Foreign Key attribute.");

        return foreignKeyProperty;
    }

    private PropertyInfo GetTargetPrimaryKeyProperty(PropertyInfo sourceForeignKeyProperty)
    {
        var targetType = sourceForeignKeyProperty.GetCustomAttribute<ForeignKeyAttribute>()!.DestinationType;
        
        var targetPrimaryKey = targetType
                .GetProperties(BindingFlags.Instance
                               | BindingFlags.Public
                               | BindingFlags.DeclaredOnly)
                .FirstOrDefault(p =>
                    p.GetCustomAttribute<KeyAttribute>() != default);

        if (targetPrimaryKey == default)
            throw new ApplicationException(
                $"Target type of relationship between {sourceForeignKeyProperty.DeclaringType!.Name} and {targetType.Name} does not has primary key configuration.");

        return targetPrimaryKey;
    }

    private string GetJoinType(PropertyInfo sourceForeignKeyProperty)
    {
        var joinTypeAttribute = sourceForeignKeyProperty.GetCustomAttribute<JoinAttribute>();

        if (joinTypeAttribute == default)
            throw new ApplicationException($"Source foreign key property {sourceForeignKeyProperty.Name} does not has join type configuration.");

        if (joinTypeAttribute.JoinType == JoinType.Cross)
            return "CROSS JOIN";
        else if (joinTypeAttribute.JoinType == JoinType.Inner)
            return "INNER JOIN";
        else if (joinTypeAttribute.JoinType == JoinType.Left)
            return "LEFT JOIN";
        else if (joinTypeAttribute.JoinType == JoinType.Right)
            return "RIGHT JOIN";
        else
            throw new ApplicationException($"Join attribute of source foreign key property {sourceForeignKeyProperty.Name} has invalid join type value.");
    }
}