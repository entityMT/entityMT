using System.Linq.Expressions;
using System.Reflection;
using FluentAssertions;
using Moq;
using MTConfigurations.Abstractions.Attributes;
using MTQueries.Abstractions.ClausuleManagers.MemberAccessHandlers;
using MTQueries.SqlServer.UnitTests.Fakers;

namespace MTQueries.SqlServer.UnitTests;

public sealed class DefaultWhereClausuleManagerUnitTests
{
    private delegate void MemberAccessHandler(MemberExpression node, out object value);
    
    [Fact(DisplayName = "Only one 'BinaryExpression' with equal node type (Simple Member Access)")]
    public void SetCondition_OneBinaryExpressionWithEqualSimpleMemberAccess_Success()
    {
        // prepare
        Expression<Func<SimpleEntity, bool>> filter = e => e.Name == "SimpleEntity";
        var whereManager = new DefaultWhereClausuleManager<SimpleEntity>(default!);

        // act
        whereManager.SetCondition(filter);
        var where = whereManager.GetWhere;

        // assert
        var tableAttribute = typeof(SimpleEntity).GetCustomAttribute<TableAttribute>();
        var nameProperty = typeof(SimpleEntity)
            .GetProperties()
            .FirstOrDefault(p
                => p.Name == nameof(SimpleEntity.Name));
        var nameColumn = nameProperty!.GetCustomAttribute<ColumnAttribute>();
        
        Assert.NotNull(where);
        Assert.Equal($"WHERE (({tableAttribute!.Name}.{nameColumn!.Name} = @param_1 ))", where);
    }
    
    [Fact(DisplayName = "Only one 'BinaryExpression' with equal node type (Nested Member Access)")]
    public void SetCondition_OneBinaryExpressionWithEqualNestedMemberAccess_Success()
    {
        // prepare
        Expression<Func<CrossJoinEntity, bool>> filter = e => e.SimpleEntity.Name == "SimpleEntity";
        var whereManager = new DefaultWhereClausuleManager<CrossJoinEntity>(new List<IMemberAccessHandler>());

        // act
        whereManager.SetCondition(filter);
        var where = whereManager.GetWhere;

        // assert
        var tableAttribute = typeof(SimpleEntity).GetCustomAttribute<TableAttribute>();
        var nameProperty = typeof(SimpleEntity)
            .GetProperties()
            .FirstOrDefault(p
                => p.Name == nameof(SimpleEntity.Name));
        var nameColumn = nameProperty!.GetCustomAttribute<ColumnAttribute>();
        
        Assert.NotNull(where);
        Assert.Equal($"WHERE (({tableAttribute!.Name}.{nameColumn!.Name} = @param_1 ))", where);
    }
    
    [Fact(DisplayName = "More than one 'BinaryExpression' with equal node type (Nested Member Access)")]
    public void SetCondition_MoreThanOneBinaryExpressionWithEqualNestedMemberAccess_Success()
    {
        // prepare
        Expression<Func<CrossJoinEntity, bool>> filter = 
            e => 
                e.SimpleEntity.Name == "SimpleEntity"
                || e.SimpleEntity.Name == "TwoConditionalAccess";
        
        var whereManager = new DefaultWhereClausuleManager<CrossJoinEntity>(new List<IMemberAccessHandler>());

        // act
        whereManager.SetCondition(filter);
        var where = whereManager.GetWhere;

        // assert
        var tableAttribute = typeof(SimpleEntity).GetCustomAttribute<TableAttribute>();
        var nameProperty = typeof(SimpleEntity)
            .GetProperties()
            .FirstOrDefault(p
                => p.Name == nameof(SimpleEntity.Name));
        var nameColumn = nameProperty!.GetCustomAttribute<ColumnAttribute>();
        
        Assert.NotNull(where);
        Assert.Equal(
            $"WHERE (({tableAttribute!.Name}.{nameColumn!.Name} = @param_1 ) OR ({tableAttribute!.Name}.{nameColumn!.Name} = @param_2 ))", 
            where);
    }

    [Fact(DisplayName = "More than one 'BinaryExpression' with different node types")]
    public void SetCondition_MoreThanOneBinaryExpressionWithDifferentNodeTypes_Success()
    {
        // prepare
        var whereManager = new DefaultWhereClausuleManager<SimpleEntity>(default!);
        
        whereManager.SetCondition(
            entity => 
                entity.Name.Contains("SimpleEntity") ||
                entity.Name.Contains("Name") ||
                entity.Name.Contains("LeftJoin"));
        
        whereManager.SetCondition(
            entity => 
                entity.Years > 0);
        
        // act
        var where = whereManager.GetWhere;
        var parameters = whereManager.Parameters;

        // assert
        var parameterKeys = new List<KeyValuePair<string, object>>();
        parameterKeys.Add(new KeyValuePair<string, object>("param_1", nameof(SimpleEntity)));
        parameterKeys.Add(new KeyValuePair<string, object>("param_2", nameof(SimpleEntity.Name)));
        parameterKeys.Add(new KeyValuePair<string, object>("param_3", "LeftJoin"));
        parameterKeys.Add(new KeyValuePair<string, object>("param_4", 0));
        
        Assert.Equal($"WHERE (({nameof(SimpleEntity)}.{nameof(SimpleEntity.Name)} LIKE '%' + @param_1 + '%' ) OR ({nameof(SimpleEntity)}.{nameof(SimpleEntity.Name)} LIKE '%' + @param_2 + '%' ) OR ({nameof(SimpleEntity)}.{nameof(SimpleEntity.Name)} LIKE '%' + @param_3 + '%' )) AND (({nameof(SimpleEntity)}.Year > @param_4 ))",
            where);

        parameters
            .Should()
            .BeEquivalentTo(new Dictionary<string, object>(parameterKeys));
    }
    
    [Fact(DisplayName = "Only one 'BinaryExpression' with variable and equal node type (Simple Member Access)")]
    public void SetCondition_VariableSimpleMemberAccess_Success()
    {
        // prepare
        var id = Guid.NewGuid();
        
        Expression<Func<SimpleEntity, bool>> filter = 
            e => 
                e.Id == id;
        
        var whereManager = new DefaultWhereClausuleManager<SimpleEntity>(new List<IMemberAccessHandler>());

        // act
        whereManager.SetCondition(filter);
        var where = whereManager.GetWhere;
        var parameters = whereManager.Parameters;

        // assert
        var tableAttribute = typeof(SimpleEntity).GetCustomAttribute<TableAttribute>();
        var idProperty = typeof(SimpleEntity)
            .GetProperties()
            .FirstOrDefault(p
                => p.Name == nameof(SimpleEntity.Id));
        var nameColumn = idProperty!.GetCustomAttribute<ColumnAttribute>();
        var parameterKeys = new List<KeyValuePair<string, object>>();
        
        parameterKeys.Add(new KeyValuePair<string, object>("param_1", id));
        
        Assert.NotNull(where);
        Assert.Equal(
            $"WHERE (({tableAttribute!.Name}.{nameColumn!.Name} = @param_1 ))", 
            where);

        parameters
            .Should()
            .BeEquivalentTo(
                new Dictionary<string, object>(parameterKeys));
    }

    [Fact(DisplayName = "Method Call 'Contains' operation")]
    public void SetCondition_MethodCallExpression_Success()
    {
        // prepare
        var whereManager = new DefaultWhereClausuleManager<SimpleEntity>(default!);

        // act
        whereManager.SetCondition(entity => entity.Name.Contains(nameof(SimpleEntity)));
        var where = whereManager.GetWhere;
        
        // assert
        Assert.Equal($"WHERE (({nameof(SimpleEntity)}.{nameof(SimpleEntity.Name)} LIKE '%' + @param_1 + '%' ))", where.Trim());
    }

    [Fact(DisplayName = "Only one 'BinaryExpression' with object filter property.")]
    public void SetCondition_ObjectFilter_Success()
    {
        // prepare
        var memberAccessHandlerMock = new Mock<IMemberAccessHandler>();
        memberAccessHandlerMock.Setup(
                mah =>
                    mah.Handle(
                        It.Is<MemberExpression>(mexp => mexp.Member.Name.Equals("Name")),
                        out It.Ref<object>.IsAny))
            .Callback(
                new MemberAccessHandler(
                    (MemberExpression node, out object value) => value = 0))
            .Returns(true);
        
        var whereManager = new DefaultWhereClausuleManager<SimpleEntity>(
            new List<IMemberAccessHandler>() { memberAccessHandlerMock.Object} );
        
        var filter = new
        {
            Name = nameof(SimpleEntity)
        };

        // act
        whereManager.SetCondition(entity => entity.Name == filter.Name);
        var where = whereManager.GetWhere;

        // assert
        Assert.Equal($"WHERE (({nameof(SimpleEntity)}.{nameof(SimpleEntity.Name)} = @param_1 ))", where);
    }
}