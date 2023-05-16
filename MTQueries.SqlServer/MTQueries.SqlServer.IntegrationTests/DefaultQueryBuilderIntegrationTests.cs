using System.Reflection;
using FluentAssertions;
using MTConfigurations.Abstractions;
using MTConfigurations.Abstractions.Attributes;
using MTQueries.SqlServer.IntegrationTests.Fakers;

namespace MTQueries.SqlServer.IntegrationTests;

public sealed class DefaultQueryBuilderIntegrationTests
{
    [Fact(DisplayName = "Without joins, where, group by and order.")]
    public void Build_WithoutAnyClauses_Success()
    {
        // prepare
        var queryBuilder = new DefaultQueryBuilder<SimpleEntity>(
            new DefaultGroupByClausuleManager<SimpleEntity>(),
            new DefaultOrderByClausuleManager<SimpleEntity>(),
            new DefaultWhereClausuleManager<SimpleEntity>(null!),
            new DefaultQuerySelectedColumnsProvider<SimpleEntity>(),
            new DefaultQueryJoinsGenerator<SimpleEntity>());

        // act
        var query = queryBuilder.Build();

        // assert
        var table = typeof(SimpleEntity).GetCustomAttribute<TableAttribute>();
        var properties = typeof(SimpleEntity).GetProperties();
        var idColumn = properties
            .First(p => p.Name == nameof(SimpleEntity.Id))
            .GetCustomAttribute<ColumnAttribute>();
        var nameColumn = properties
            .First(p => p.Name == nameof(SimpleEntity.Name))
            .GetCustomAttribute<ColumnAttribute>();
        var yearColumn = properties
            .First(p => p.Name == nameof(SimpleEntity.Years))
            .GetCustomAttribute<ColumnAttribute>();
        
        Assert.NotNull(query);
        Assert.Equal(
            $"select {table!.Name}.{idColumn!.Name},{table.Name}.{nameColumn!.Name},{table.Name}.{yearColumn!.Name} from {table.Name}",
            query.Content.Trim());
    }
    
    [Fact(DisplayName = "With one inner join.")]
    public void Build_WithOneInnerJoin_Success()
    {
        // prepare
        var queryBuilder = new DefaultQueryBuilder<InnerJoinEntity>(
            new DefaultGroupByClausuleManager<InnerJoinEntity>(),
            new DefaultOrderByClausuleManager<InnerJoinEntity>(),
            new DefaultWhereClausuleManager<InnerJoinEntity>(null!),
            new DefaultQuerySelectedColumnsProvider<InnerJoinEntity>(),
            new DefaultQueryJoinsGenerator<InnerJoinEntity>());

        // act
        var query = queryBuilder.Build();

        // assert
        var joinTable = typeof(InnerJoinEntity).GetCustomAttribute<TableAttribute>();
        var properties = typeof(InnerJoinEntity).GetProperties();
        var idColumn = properties
            .First(p => p.Name == nameof(InnerJoinEntity.Id))
            .GetCustomAttribute<ColumnAttribute>();
        var descriptionColumn = properties
            .First(p => p.Name == nameof(InnerJoinEntity.Description))
            .GetCustomAttribute<ColumnAttribute>();
        var simpleEntityFkColumn = properties
            .First(p => p.Name == nameof(InnerJoinEntity.SimpleEntityId))
            .GetCustomAttribute<ColumnAttribute>();

        var simpleTable = typeof(SimpleEntity).GetCustomAttribute<TableAttribute>();

        properties = typeof(SimpleEntity).GetProperties();
            
        var simpleIdColumn = properties
            .First(p => p.Name == nameof(SimpleEntity.Id))
            .GetCustomAttribute<ColumnAttribute>();
        var simpleNameColumn = properties
            .First(p => p.Name == nameof(SimpleEntity.Name))
            .GetCustomAttribute<ColumnAttribute>();
        var simpleYearColumn = properties
            .First(p => p.Name == nameof(SimpleEntity.Years))
            .GetCustomAttribute<ColumnAttribute>();
        
        Assert.NotNull(query);
        Assert.Equal(
            $"select {joinTable!.Name}.{idColumn!.Name},{joinTable.Name}.{descriptionColumn!.Name},{joinTable.Name}.{simpleEntityFkColumn!.Name},{simpleTable!.Name}.{simpleIdColumn!.Name},{simpleTable!.Name}.{simpleNameColumn!.Name},{simpleTable!.Name}.{simpleYearColumn!.Name} from {joinTable.Name} INNER JOIN {simpleTable.Name} ON {simpleTable.Name}.{simpleIdColumn.Name} = {joinTable.Name}.{simpleEntityFkColumn.Name}",
            query.Content.Trim());
    }
    
    [Fact(DisplayName = "With one left join.")]
    public void Build_WithOneLeftJoin_Success()
    {
        // prepare
        var queryBuilder = new DefaultQueryBuilder<LeftJoinEntity>(
            new DefaultGroupByClausuleManager<LeftJoinEntity>(),
            new DefaultOrderByClausuleManager<LeftJoinEntity>(),
            new DefaultWhereClausuleManager<LeftJoinEntity>(null!),
            new DefaultQuerySelectedColumnsProvider<LeftJoinEntity>(),
            new DefaultQueryJoinsGenerator<LeftJoinEntity>());

        // act
        var query = queryBuilder.Build();

        // assert
        var joinTable = typeof(LeftJoinEntity).GetCustomAttribute<TableAttribute>();
        var properties = typeof(LeftJoinEntity).GetProperties();
        var idColumn = properties
            .First(p => p.Name == nameof(LeftJoinEntity.Id))
            .GetCustomAttribute<ColumnAttribute>();
        var descriptionColumn = properties
            .First(p => p.Name == nameof(LeftJoinEntity.Description))
            .GetCustomAttribute<ColumnAttribute>();
        var simpleEntityFkColumn = properties
            .First(p => p.Name == nameof(LeftJoinEntity.SimpleEntityId))
            .GetCustomAttribute<ColumnAttribute>();

        var simpleTable = typeof(SimpleEntity).GetCustomAttribute<TableAttribute>();

        properties = typeof(SimpleEntity).GetProperties();
            
        var simpleIdColumn = properties
            .First(p => p.Name == nameof(SimpleEntity.Id))
            .GetCustomAttribute<ColumnAttribute>();
        var simpleNameColumn = properties
            .First(p => p.Name == nameof(SimpleEntity.Name))
            .GetCustomAttribute<ColumnAttribute>();
        var simpleYearColumn = properties
            .First(p => p.Name == nameof(SimpleEntity.Years))
            .GetCustomAttribute<ColumnAttribute>();
        
        Assert.NotNull(query);
        Assert.Equal(
            $"select {joinTable!.Name}.{idColumn!.Name},{joinTable.Name}.{descriptionColumn!.Name},{joinTable.Name}.{simpleEntityFkColumn!.Name},{simpleTable!.Name}.{simpleIdColumn!.Name},{simpleTable!.Name}.{simpleNameColumn!.Name},{simpleTable!.Name}.{simpleYearColumn!.Name} from {joinTable.Name} LEFT JOIN {simpleTable.Name} ON {simpleTable.Name}.{simpleIdColumn.Name} = {joinTable.Name}.{simpleEntityFkColumn.Name}",
            query.Content.Trim());
    }
    
    [Fact(DisplayName = "With filter.")]
    public void Build_WithWhere_Success()
    {
        // prepare
        var queryBuilder = new DefaultQueryBuilder<SimpleEntity>(
            new DefaultGroupByClausuleManager<SimpleEntity>(),
            new DefaultOrderByClausuleManager<SimpleEntity>(),
            new DefaultWhereClausuleManager<SimpleEntity>(null!),
            new DefaultQuerySelectedColumnsProvider<SimpleEntity>(),
            new DefaultQueryJoinsGenerator<SimpleEntity>());

        // act
        queryBuilder.SetFilter(se => se.Years > 10);
        var query = queryBuilder.Build();

        // assert
        var table = typeof(SimpleEntity).GetCustomAttribute<TableAttribute>();
        var properties = typeof(SimpleEntity).GetProperties();
        var idColumn = properties
            .First(p => p.Name == nameof(SimpleEntity.Id))
            .GetCustomAttribute<ColumnAttribute>();
        var nameColumn = properties
            .First(p => p.Name == nameof(SimpleEntity.Name))
            .GetCustomAttribute<ColumnAttribute>();
        var yearColumn = properties
            .First(p => p.Name == nameof(SimpleEntity.Years))
            .GetCustomAttribute<ColumnAttribute>();

        var parameters = new Dictionary<string, object>();
        parameters.Add("param_1", 10);
        
        Assert.NotNull(query);
        Assert.Equal(
            $"select {table!.Name}.{idColumn!.Name},{table!.Name}.{nameColumn!.Name},{table!.Name}.{yearColumn!.Name} from {table.Name}  WHERE (({table.Name}.{yearColumn.Name} > @param_1 ))",
            query.Content.Trim());

        parameters.Should().BeEquivalentTo(query.Parameters);
    }
    
    [Fact(DisplayName = "With order.")]
    public void Build_WithOrder_Success()
    {
        // prepare
        var queryBuilder = new DefaultQueryBuilder<SimpleEntity>(
            new DefaultGroupByClausuleManager<SimpleEntity>(),
            new DefaultOrderByClausuleManager<SimpleEntity>(),
            new DefaultWhereClausuleManager<SimpleEntity>(null!),
            new DefaultQuerySelectedColumnsProvider<SimpleEntity>(),
            new DefaultQueryJoinsGenerator<SimpleEntity>());

        // act
        queryBuilder.SetOrder(se => se.Years);
        var query = queryBuilder.Build();

        // assert
        var table = typeof(SimpleEntity).GetCustomAttribute<TableAttribute>();
        var properties = typeof(SimpleEntity).GetProperties();
        var idColumn = properties
            .First(p => p.Name == nameof(SimpleEntity.Id))
            .GetCustomAttribute<ColumnAttribute>();
        var nameColumn = properties
            .First(p => p.Name == nameof(SimpleEntity.Name))
            .GetCustomAttribute<ColumnAttribute>();
        var yearColumn = properties
            .First(p => p.Name == nameof(SimpleEntity.Years))
            .GetCustomAttribute<ColumnAttribute>();

        Assert.NotNull(query);
        Assert.Equal(
            $"select {table!.Name}.{idColumn!.Name},{table!.Name}.{nameColumn!.Name},{table!.Name}.{yearColumn!.Name} from {table.Name}    ORDER BY {table.Name}.{yearColumn.Name} ASC",
            query.Content.Trim());
    }
    
    [Fact(DisplayName = "With filter and order.")]
    public void Build_WithFilterAndOrder_Success()
    {
        // prepare
        var queryBuilder = new DefaultQueryBuilder<SimpleEntity>(
            new DefaultGroupByClausuleManager<SimpleEntity>(),
            new DefaultOrderByClausuleManager<SimpleEntity>(),
            new DefaultWhereClausuleManager<SimpleEntity>(null!),
            new DefaultQuerySelectedColumnsProvider<SimpleEntity>(),
            new DefaultQueryJoinsGenerator<SimpleEntity>());

        // act
        queryBuilder.SetFilter(se => se.Years > 5);
        queryBuilder.SetFilter(se => se.Name.Contains("Entity"));
        queryBuilder.SetOrder(se => se.Years);
        
        var query = queryBuilder.Build();

        // assert
        var table = typeof(SimpleEntity).GetCustomAttribute<TableAttribute>();
        var properties = typeof(SimpleEntity).GetProperties();
        var idColumn = properties
            .First(p => p.Name == nameof(SimpleEntity.Id))
            .GetCustomAttribute<ColumnAttribute>();
        var nameColumn = properties
            .First(p => p.Name == nameof(SimpleEntity.Name))
            .GetCustomAttribute<ColumnAttribute>();
        var yearColumn = properties
            .First(p => p.Name == nameof(SimpleEntity.Years))
            .GetCustomAttribute<ColumnAttribute>();

        var parameters = new Dictionary<string, object>();
        parameters.Add("param_1", 5);
        parameters.Add("param_2", "Entity");
        
        Assert.NotNull(query);
        Assert.Equal(
            $"select {table!.Name}.{idColumn!.Name},{table!.Name}.{nameColumn!.Name},{table!.Name}.{yearColumn!.Name} from {table.Name}  WHERE (({table!.Name}.{yearColumn!.Name} > @param_1 )) AND (({table!.Name}.{nameColumn.Name} LIKE '%' + @param_2 + '%' ))  ORDER BY {table.Name}.{yearColumn.Name} ASC",
            query.Content.Trim());

        parameters.Should().BeEquivalentTo(query.Parameters);
    }
}