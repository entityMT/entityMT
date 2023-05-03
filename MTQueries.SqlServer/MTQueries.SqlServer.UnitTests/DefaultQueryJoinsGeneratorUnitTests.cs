using System.Reflection;
using FluentAssertions;
using MTConfigurations.Abstractions.Attributes;
using MTQueries.SqlServer.UnitTests.Fakers;

namespace MTQueries.SqlServer.UnitTests;

public sealed class DefaultQueryJoinsGeneratorUnitTests
{
    [Fact(DisplayName = "Reference Property without Foreign Key attribute configuration")]
    public async Task GetJoins_ReferenceTableWithoutForeignKeyAttribute_ApplicationException()
    {
        // prepare
        var joinGenerator = new DefaultQueryJoinsGenerator<WithoutForeignAttributeEntity>();
        var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;

        // act
        var exception = await Assert.ThrowsAsync<ApplicationException>(() =>
            joinGenerator.GetJoinsAsync(cancellationToken));

        // assert
        var propertyName = nameof(WithoutForeignAttributeEntity.SimpleEntities);
        Assert.Equal($"Reference property {propertyName} does not have corresponding Foreign Key attribute.", exception.Message);
    }
    
    [Fact(DisplayName = "Reference Property without Join attribute configuration")]
    public async Task GetJoins_ReferenceTableWithoutJoinAttribute_ApplicationException()
    {
        // prepare
        var joinGenerator = new DefaultQueryJoinsGenerator<WithoutJoinAttributeEntity>();
        var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;

        // act
        var exception = await Assert.ThrowsAsync<ApplicationException>(() =>
            joinGenerator.GetJoinsAsync(cancellationToken));

        // assert
        var propertyName = nameof(WithoutJoinAttributeEntity.SimpleEntityId);
        Assert.Equal($"Source foreign key property {propertyName} does not has join type configuration.", exception.Message);
    }
    
    [Fact(DisplayName = "Inner join")]
    public async Task GetJoins_InnerJoin_Success()
    {
        // prepare
        var joinGenerator = new DefaultQueryJoinsGenerator<InnerJoinEntity>();
        var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;

        // act
        var joins = await joinGenerator.GetJoinsAsync(cancellationToken);

        // assert
        var sourceTable = typeof(InnerJoinEntity).GetCustomAttribute<TableAttribute>();
        
        var targetTable = typeof(SimpleEntity).GetCustomAttribute<TableAttribute>();
        
        var sourceForeignKeyProperty = typeof(InnerJoinEntity)
            .GetProperties()
            .FirstOrDefault(p =>
                p.GetCustomAttribute<ForeignKeyAttribute>() != default);
        
        var targetPrimaryKeyProperty = typeof(SimpleEntity)
            .GetProperties()
            .FirstOrDefault(p =>
                p.GetCustomAttribute<KeyAttribute>() != default);
        
        var targetPrimaryKeyColumn = targetPrimaryKeyProperty!.GetCustomAttribute<ColumnAttribute>();
        
        var sourceForeignKeyColumn = sourceForeignKeyProperty!.GetCustomAttribute<ColumnAttribute>();
        
        joins.Should().BeEquivalentTo(
            new[]
            {
                $"INNER JOIN {targetTable!.Name} ON {targetTable.Name}.{targetPrimaryKeyColumn!.Name} = {sourceTable!.Name}.{sourceForeignKeyColumn!.Name}"
            });
    }

    [Fact(DisplayName = "Left join")]
    public async Task GetJoins_LeftJoin_Success()
    {
        // prepare
        var joinGenerator = new DefaultQueryJoinsGenerator<LeftJoinEntity>();
        var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;
        
        // act
        var joins = await joinGenerator.GetJoinsAsync(cancellationToken);
        
        // assert
        var sourceTable = typeof(LeftJoinEntity).GetCustomAttribute<TableAttribute>();
        
        var targetTable = typeof(SimpleEntity).GetCustomAttribute<TableAttribute>();
        
        var sourceForeignKeyProperty = typeof(LeftJoinEntity)
            .GetProperties()
            .FirstOrDefault(p =>
                p.GetCustomAttribute<ForeignKeyAttribute>() != default);
        
        var targetPrimaryKeyProperty = typeof(SimpleEntity)
            .GetProperties()
            .FirstOrDefault(p =>
                p.GetCustomAttribute<KeyAttribute>() != default);
        
        var targetPrimaryKeyColumn = targetPrimaryKeyProperty!.GetCustomAttribute<ColumnAttribute>();
        
        var sourceForeignKeyColumn = sourceForeignKeyProperty!.GetCustomAttribute<ColumnAttribute>();
        
        joins.Should().BeEquivalentTo(
            new[]
            {
                $"LEFT JOIN {targetTable!.Name} ON {targetTable.Name}.{targetPrimaryKeyColumn!.Name} = {sourceTable!.Name}.{sourceForeignKeyColumn!.Name}"
            });
    }
    
    [Fact(DisplayName = "Right join")]
    public async Task GetJoins_RightJoin_Success()
    {
        // prepare
        var joinGenerator = new DefaultQueryJoinsGenerator<RightJoinEntity>();
        var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;
        
        // act
        var joins = await joinGenerator.GetJoinsAsync(cancellationToken);
        
        // assert
        var sourceTable = typeof(RightJoinEntity).GetCustomAttribute<TableAttribute>();
        
        var targetTable = typeof(SimpleEntity).GetCustomAttribute<TableAttribute>();
        
        var sourceForeignKeyProperty = typeof(RightJoinEntity)
            .GetProperties()
            .FirstOrDefault(p =>
                p.GetCustomAttribute<ForeignKeyAttribute>() != default);
        
        var targetPrimaryKeyProperty = typeof(SimpleEntity)
            .GetProperties()
            .FirstOrDefault(p =>
                p.GetCustomAttribute<KeyAttribute>() != default);
        
        var targetPrimaryKeyColumn = targetPrimaryKeyProperty!.GetCustomAttribute<ColumnAttribute>();
        
        var sourceForeignKeyColumn = sourceForeignKeyProperty!.GetCustomAttribute<ColumnAttribute>();
        
        joins.Should().BeEquivalentTo(
            new[]
            {
                $"RIGHT JOIN {targetTable!.Name} ON {targetTable.Name}.{targetPrimaryKeyColumn!.Name} = {sourceTable!.Name}.{sourceForeignKeyColumn!.Name}"
            });
    }
    
    [Fact(DisplayName = "Cross join")]
    public async Task GetJoins_CrossJoin_Success()
    {
        // prepare
        var joinGenerator = new DefaultQueryJoinsGenerator<CrossJoinEntity>();
        var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;
        
        // act
        var joins = await joinGenerator.GetJoinsAsync(cancellationToken);
        
        // assert
        var sourceTable = typeof(CrossJoinEntity).GetCustomAttribute<TableAttribute>();
        
        var targetTable = typeof(SimpleEntity).GetCustomAttribute<TableAttribute>();
        
        var sourceForeignKeyProperty = typeof(CrossJoinEntity)
            .GetProperties()
            .FirstOrDefault(p =>
                p.GetCustomAttribute<ForeignKeyAttribute>() != default);
        
        var targetPrimaryKeyProperty = typeof(SimpleEntity)
            .GetProperties()
            .FirstOrDefault(p =>
                p.GetCustomAttribute<KeyAttribute>() != default);
        
        var targetPrimaryKeyColumn = targetPrimaryKeyProperty!.GetCustomAttribute<ColumnAttribute>();
        
        var sourceForeignKeyColumn = sourceForeignKeyProperty!.GetCustomAttribute<ColumnAttribute>();
        
        joins.Should().BeEquivalentTo(
            new[]
            {
                $"CROSS JOIN {targetTable!.Name} ON {targetTable.Name}.{targetPrimaryKeyColumn!.Name} = {sourceTable!.Name}.{sourceForeignKeyColumn!.Name}"
            });
    }
}