using System.Reflection;
using MTPConfigurations.Abstractions.Attributes;
using MTQueries.SqlServer.UnitTests.Fakers;

namespace MTQueries.SqlServer.UnitTests;

public sealed class DefaultOrderByClausuleManagerUnitTests
{
    [Fact(DisplayName = "Order By (ASC) without entity column (reference type)")]
    public void AddOrder_WithoutEntityColumn_Success()
    {
        // prepare
        var orderClausuleManager = new DefaultOrderByClausuleManager<LeftJoinEntity>();
        orderClausuleManager.AddOrder(leftJoinEntity => leftJoinEntity.SimpleEntity);

        // act
        string order = orderClausuleManager.GetOrderBy;

        // assert
        Assert.Equal(string.Empty, order);
    }

    [Fact(DisplayName = "Order By (ASC) with entity column attribute (with primary types)")]
    public void AddOrder_WithEntityColumn_Success()
    {
        // prepare
        var orderByClausuleManager = new DefaultOrderByClausuleManager<SimpleEntity>();
        orderByClausuleManager.AddOrder(entity => entity.Id);
        
        // act
        string order = orderByClausuleManager.GetOrderBy;

        // assert
        Assert.Equal($"ORDER BY {nameof(SimpleEntity)}.{nameof(SimpleEntity.Id)} ASC", order);
    }

    [Fact(DisplayName = "Order By (ASC) with nested entity column attribute")]
    public void AddOrder_WithNestedEntityColumn_Success()
    {
        // prepare
        var orderByClausuleManager = new DefaultOrderByClausuleManager<LeftJoinEntity>();
        orderByClausuleManager.AddOrder(entity => entity.SimpleEntity.Name);
        
        // act
        string order = orderByClausuleManager.GetOrderBy;
        
        // assert
        Assert.Equal($"ORDER BY {nameof(LeftJoinEntity.SimpleEntity)}.{nameof(LeftJoinEntity.SimpleEntity.Name)} ASC", order);
    }

    [Fact(DisplayName = "Order By (ASC) more than one column")]
    public void AddOrder_MoreThanOneColumn_Success()
    {
        // prepare
        var orderByClausuleManager = new DefaultOrderByClausuleManager<SimpleEntity>();
        orderByClausuleManager.AddOrder(entity => entity.Id);
        orderByClausuleManager.AddOrder(entity => entity.Years);
        
        // act
        string order = orderByClausuleManager.GetOrderBy;
        
        // assert
        var columnYearName = typeof(SimpleEntity)
            .GetProperties()
            .First(p => p.Name.Equals(nameof(SimpleEntity.Years)))
            .GetCustomAttribute<ColumnAttribute>()!
            .Name;
        
        Assert.Equal(
            $"ORDER BY {nameof(SimpleEntity)}.{nameof(SimpleEntity.Id)} ASC,{nameof(SimpleEntity)}.{columnYearName} ASC", 
            order);
    }

    [Fact(DisplayName = "Order By Desc")]
    public void AddOrder_Desc_Success()
    {
        // prepare
        var orderByClausuleManager = new DefaultOrderByClausuleManager<SimpleEntity>();
        orderByClausuleManager.AddOrder(entity => entity.Id, false);
        
        // act
        var order = orderByClausuleManager.GetOrderBy;

        // assert
        Assert.Equal(
            $"ORDER BY {nameof(SimpleEntity)}.{nameof(SimpleEntity.Id)} DESC",
            order);
    }
}