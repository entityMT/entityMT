using System.Reflection;
using MTConfigurations.Abstractions.Attributes;
using MTQueries.SqlServer.UnitTests.Fakers;

namespace MTQueries.SqlServer.UnitTests;

public sealed class DefaultGroupByClausuleManagerUnitTests
{
    [Fact(DisplayName = "Group by one column only")]
    public void AddGroup_OnlyOneColumn_Success()
    {
        // prepare
        var groupManager = new DefaultGroupByClausuleManager<SimpleEntity>();
        groupManager.AddGroup(entity => entity.Years);

        // act
        var groupBy = groupManager.GetGroupBy;

        // assert
        var entityType = typeof(SimpleEntity);

        var tableConfiguration = entityType.GetCustomAttribute<TableAttribute>();
        
        var columnConfiguration = entityType
            .GetProperties()
            .First(p => p.Name.Equals(nameof(SimpleEntity.Years)))
            .GetCustomAttribute<ColumnAttribute>();
        
        Assert.Equal($"GROUP BY {tableConfiguration!.Name}.{columnConfiguration!.Name}", groupBy);
    }

    [Fact(DisplayName = "Group by more than one column")]
    public void AddGroup_MoreThanOneColumn_Success()
    {
        // prepare
        var groupManager = new DefaultGroupByClausuleManager<SimpleEntity>();
        groupManager.AddGroup(entity => entity.Name);
        groupManager.AddGroup(entity => entity.Years);

        // act
        var groupBy = groupManager.GetGroupBy;

        // assert
        var entityType = typeof(SimpleEntity);

        var tableConfiguration = entityType.GetCustomAttribute<TableAttribute>();

        var nameColumnConfiguration = entityType
            .GetProperties()
            .First(p => p.Name.Equals(nameof(SimpleEntity.Name)))
            .GetCustomAttribute<ColumnAttribute>();

        var yearColumnConfiguration = entityType
            .GetProperties()
            .First(p => p.Name.Equals(nameof(SimpleEntity.Years)))
            .GetCustomAttribute<ColumnAttribute>();
        
        Assert.Equal(
            $"GROUP BY {tableConfiguration!.Name}.{nameColumnConfiguration!.Name}, {tableConfiguration.Name}.{yearColumnConfiguration!.Name}",
            groupBy);
    }

    [Fact(DisplayName = "Group by property without column configuration")]
    public void AddGroup_ColumnWithoutConfiguration_Success()
    {
        // prepare
        var groupManager = new DefaultGroupByClausuleManager<NestedTypeEntity>();
        groupManager.AddGroup(entity => entity.SimpleEntity);

        // act
        var groupBy = groupManager.GetGroupBy;

        // assert
        Assert.Equal(string.Empty, groupBy);
    }

    [Fact(DisplayName = "Group by with nested column")]
    public void AddGroup_NestedColumn_Success()
    {
        // prepare
        var groupManager = new DefaultGroupByClausuleManager<NestedTypeEntity>();
        groupManager.AddGroup(entity => entity.SimpleEntity.Years);

        // act
        var groupBy = groupManager.GetGroupBy;

        // assert
        var entityType = typeof(SimpleEntity);

        var tableConfiguration = entityType.GetCustomAttribute<TableAttribute>();

        var yearColumnConfiguration = entityType
            .GetProperties()
            .First(p => p.Name.Equals(nameof(SimpleEntity.Years)))
            .GetCustomAttribute<ColumnAttribute>();
        
        Assert.Equal(
            $"GROUP BY {tableConfiguration!.Name}.{yearColumnConfiguration!.Name}",
            groupBy);
    }
}