using System.Reflection;
using FluentAssertions;
using MTConfigurations.Abstractions.Attributes;
using MTQueries.Abstractions.ClausuleManagers.MemberAccessHandlers;
using MTQueries.SqlServer.IntegrationTests.Fakers;
using MTQueries.SqlServer.MemberAccessHandlers;

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

    [Fact(DisplayName = "More than one join.")]
    public void Build_MoreThanOneJoin_Success()
    {
        // prepare
        var queryBuilder = new DefaultQueryBuilder<MoreThanOneJoinEntity>(
            new DefaultGroupByClausuleManager<MoreThanOneJoinEntity>(),
            new DefaultOrderByClausuleManager<MoreThanOneJoinEntity>(),
            new DefaultWhereClausuleManager<MoreThanOneJoinEntity>(null!),
            new DefaultQuerySelectedColumnsProvider<MoreThanOneJoinEntity>(),
            new DefaultQueryJoinsGenerator<MoreThanOneJoinEntity>());
        
        // act
        var query = queryBuilder.Build();

        // assert
        var moreThanOneJoinTable = typeof(MoreThanOneJoinEntity).GetCustomAttribute<TableAttribute>();
        var moreThanOneJoinProperties = typeof(MoreThanOneJoinEntity).GetProperties();
        var moreThanOneJoinIdColumn = moreThanOneJoinProperties
            .First(p => p.GetCustomAttribute<KeyAttribute>() != default)
            .GetCustomAttribute<ColumnAttribute>();
        var moreThanOneJoinNameColumn = moreThanOneJoinProperties
            .First(p => p.Name == nameof(MoreThanOneJoinEntity.Name))
            .GetCustomAttribute<ColumnAttribute>();
        var moreThanOneJoinFkColumn = moreThanOneJoinProperties
            .First(p => p.Name == nameof(MoreThanOneJoinEntity.InnerJoinId))
            .GetCustomAttribute<ColumnAttribute>();

        var innerJoinTable = typeof(InnerJoinEntity).GetCustomAttribute<TableAttribute>();
        var innerJoinProperties = typeof(InnerJoinEntity).GetProperties();
        var innerJoinIdColumn = innerJoinProperties
            .First(p => p.GetCustomAttribute<KeyAttribute>() != default)
            .GetCustomAttribute<ColumnAttribute>();
        var innerJoinDescriptionColumn = innerJoinProperties
            .First(p => p.Name == nameof(InnerJoinEntity.Description))
            .GetCustomAttribute<ColumnAttribute>();
        var innerJoinFkColumn = innerJoinProperties
            .First(p => p.Name == nameof(InnerJoinEntity.SimpleEntityId))
            .GetCustomAttribute<ColumnAttribute>();

        var simpleTable = typeof(SimpleEntity).GetCustomAttribute<TableAttribute>();
        var simpleProperties = typeof(SimpleEntity).GetProperties();
        var simpleIdColumn = simpleProperties
            .First(p => p.GetCustomAttribute<KeyAttribute>() != default)
            .GetCustomAttribute<ColumnAttribute>();
        var simpleNameColumn = simpleProperties
            .First(p => p.Name == nameof(SimpleEntity.Name))
            .GetCustomAttribute<ColumnAttribute>();
        var simpleYearColumn = simpleProperties
            .First(p => p.Name == nameof(SimpleEntity.Years))
            .GetCustomAttribute<ColumnAttribute>();
        
        Assert.NotNull(query);
        Assert.Equal($"select " +
                     $"{moreThanOneJoinTable!.Name}.{moreThanOneJoinIdColumn!.Name}," +
                     $"{moreThanOneJoinTable!.Name}.{moreThanOneJoinNameColumn!.Name}," +
                     $"{moreThanOneJoinTable!.Name}.{moreThanOneJoinFkColumn!.Name}," +
                     $"{innerJoinTable!.Name}.{innerJoinIdColumn!.Name}," +
                     $"{innerJoinTable!.Name}.{innerJoinDescriptionColumn!.Name}," +
                     $"{innerJoinTable!.Name}.{innerJoinFkColumn!.Name}," +
                     $"{simpleTable!.Name}.{simpleIdColumn!.Name}," +
                     $"{simpleTable!.Name}.{simpleNameColumn!.Name}," +
                     $"{simpleTable!.Name}.{simpleYearColumn!.Name} " +
                     $"from " +
                     $"{moreThanOneJoinTable!.Name} " +
                     $"INNER JOIN {innerJoinTable!.Name} ON {innerJoinTable!.Name}.{innerJoinIdColumn!.Name} = {moreThanOneJoinTable!.Name}.{moreThanOneJoinFkColumn.Name} " +
                     $"INNER JOIN {simpleTable!.Name} ON {simpleTable!.Name}.{simpleIdColumn!.Name} = {innerJoinTable!.Name}.{innerJoinFkColumn!.Name}", query.Content.Trim());
        
        Assert.Empty(query.Parameters);
    }
    
    [Fact(DisplayName = "Member Access Handler with DateTime.")]
    public void Build_MemberAccessHandlerDateTime_Success()
    {
        // prepare
        var memberAccessHandlers= new List<IMemberAccessHandler>(){new PropertyInfoMemberAccessHandler()};
            
        var queryBuilder = new DefaultQueryBuilder<MemberAccessHandlerEntity>(
            new DefaultGroupByClausuleManager<MemberAccessHandlerEntity>(),
            new DefaultOrderByClausuleManager<MemberAccessHandlerEntity>(),
            new DefaultWhereClausuleManager<MemberAccessHandlerEntity>(memberAccessHandlers),
            new DefaultQuerySelectedColumnsProvider<MemberAccessHandlerEntity>(),
            new DefaultQueryJoinsGenerator<MemberAccessHandlerEntity>());

        var filter = new
        {
            Date = DateTime.Now
        };
        
        // act
        queryBuilder.SetFilter(e => e.Date == filter.Date);
        var query = queryBuilder.Build();

        // assert
        Assert.NotNull(query);

        var parameters = new Dictionary<string, object>();
        parameters.Add("param_1", filter.Date);

        query.Parameters.Should().BeEquivalentTo(parameters);
    }
    
    [Fact(DisplayName = "Member Access Handler with integer.")]
    public void Build_MemberAccessHandlerInt_Success()
    {
        // prepare
        var memberAccessHandlers= new List<IMemberAccessHandler>(){new PropertyInfoMemberAccessHandler()};
            
        var queryBuilder = new DefaultQueryBuilder<MemberAccessHandlerEntity>(
            new DefaultGroupByClausuleManager<MemberAccessHandlerEntity>(),
            new DefaultOrderByClausuleManager<MemberAccessHandlerEntity>(),
            new DefaultWhereClausuleManager<MemberAccessHandlerEntity>(memberAccessHandlers),
            new DefaultQuerySelectedColumnsProvider<MemberAccessHandlerEntity>(),
            new DefaultQueryJoinsGenerator<MemberAccessHandlerEntity>());

        var filter = new
        {
            Integer = 3
        };
        
        // act
        queryBuilder.SetFilter(e => e.Int == filter.Integer);
        var query = queryBuilder.Build();

        // assert
        Assert.NotNull(query);

        var parameters = new Dictionary<string, object>();
        parameters.Add("param_1", filter.Integer);

        query.Parameters.Should().BeEquivalentTo(parameters);
    }
    
    [Fact(DisplayName = "Member Access Handler with bool.")]
    public void Build_MemberAccessHandlerBool_Success()
    {
        // prepare
        var memberAccessHandlers= new List<IMemberAccessHandler>(){new PropertyInfoMemberAccessHandler()};
            
        var queryBuilder = new DefaultQueryBuilder<MemberAccessHandlerEntity>(
            new DefaultGroupByClausuleManager<MemberAccessHandlerEntity>(),
            new DefaultOrderByClausuleManager<MemberAccessHandlerEntity>(),
            new DefaultWhereClausuleManager<MemberAccessHandlerEntity>(memberAccessHandlers),
            new DefaultQuerySelectedColumnsProvider<MemberAccessHandlerEntity>(),
            new DefaultQueryJoinsGenerator<MemberAccessHandlerEntity>());

        var filter = new
        {
            Bool = true
        };
        
        // act
        queryBuilder.SetFilter(e => e.Bool == filter.Bool);
        var query = queryBuilder.Build();

        // assert
        Assert.NotNull(query);

        var parameters = new Dictionary<string, object>();
        parameters.Add("param_1", filter.Bool);

        query.Parameters.Should().BeEquivalentTo(parameters);
    }
    
    [Fact(DisplayName = "Member Access Handler with byte.")]
    public void Build_MemberAccessHandlerByte_Success()
    {
        // prepare
        var memberAccessHandlers= new List<IMemberAccessHandler>(){new PropertyInfoMemberAccessHandler()};
            
        var queryBuilder = new DefaultQueryBuilder<MemberAccessHandlerEntity>(
            new DefaultGroupByClausuleManager<MemberAccessHandlerEntity>(),
            new DefaultOrderByClausuleManager<MemberAccessHandlerEntity>(),
            new DefaultWhereClausuleManager<MemberAccessHandlerEntity>(memberAccessHandlers),
            new DefaultQuerySelectedColumnsProvider<MemberAccessHandlerEntity>(),
            new DefaultQueryJoinsGenerator<MemberAccessHandlerEntity>());

        var filter = new
        {
            Byte = (byte)2
        };
        
        // act
        queryBuilder.SetFilter(e => e.Byte == filter.Byte);
        var query = queryBuilder.Build();

        // assert
        Assert.NotNull(query);

        var parameters = new Dictionary<string, object>();
        parameters.Add("param_1", filter.Byte);

        query.Parameters.Should().BeEquivalentTo(parameters);
    }
    
    [Fact(DisplayName = "Member Access Handler with char.")]
    public void Build_MemberAccessHandlerChar_Success()
    {
        // prepare
        var memberAccessHandlers= new List<IMemberAccessHandler>(){new PropertyInfoMemberAccessHandler()};
            
        var queryBuilder = new DefaultQueryBuilder<MemberAccessHandlerEntity>(
            new DefaultGroupByClausuleManager<MemberAccessHandlerEntity>(),
            new DefaultOrderByClausuleManager<MemberAccessHandlerEntity>(),
            new DefaultWhereClausuleManager<MemberAccessHandlerEntity>(memberAccessHandlers),
            new DefaultQuerySelectedColumnsProvider<MemberAccessHandlerEntity>(),
            new DefaultQueryJoinsGenerator<MemberAccessHandlerEntity>());

        var filter = new
        {
            Char = 'a'
        };
        
        // act
        queryBuilder.SetFilter(e => e.Byte == filter.Char);
        var query = queryBuilder.Build();

        // assert
        Assert.NotNull(query);

        var parameters = new Dictionary<string, object>();
        parameters.Add("param_1", filter.Char);

        query.Parameters.Should().BeEquivalentTo(parameters);
    }
    
    [Fact(DisplayName = "Member Access Handler with decimal.")]
    public void Build_MemberAccessHandlerDecimal_Success()
    {
        // prepare
        var memberAccessHandlers= new List<IMemberAccessHandler>(){new PropertyInfoMemberAccessHandler()};
            
        var queryBuilder = new DefaultQueryBuilder<MemberAccessHandlerEntity>(
            new DefaultGroupByClausuleManager<MemberAccessHandlerEntity>(),
            new DefaultOrderByClausuleManager<MemberAccessHandlerEntity>(),
            new DefaultWhereClausuleManager<MemberAccessHandlerEntity>(memberAccessHandlers),
            new DefaultQuerySelectedColumnsProvider<MemberAccessHandlerEntity>(),
            new DefaultQueryJoinsGenerator<MemberAccessHandlerEntity>());

        var filter = new
        {
            Decimal = (decimal)13.2
        };
        
        // act
        queryBuilder.SetFilter(e => e.Decimal == filter.Decimal);
        var query = queryBuilder.Build();

        // assert
        Assert.NotNull(query);

        var parameters = new Dictionary<string, object>();
        parameters.Add("param_1", filter.Decimal);

        query.Parameters.Should().BeEquivalentTo(parameters);
    }
    
    [Fact(DisplayName = "Member Access Handler with double.")]
    public void Build_MemberAccessHandlerDouble_Success()
    {
        // prepare
        var memberAccessHandlers= new List<IMemberAccessHandler>(){new PropertyInfoMemberAccessHandler()};
            
        var queryBuilder = new DefaultQueryBuilder<MemberAccessHandlerEntity>(
            new DefaultGroupByClausuleManager<MemberAccessHandlerEntity>(),
            new DefaultOrderByClausuleManager<MemberAccessHandlerEntity>(),
            new DefaultWhereClausuleManager<MemberAccessHandlerEntity>(memberAccessHandlers),
            new DefaultQuerySelectedColumnsProvider<MemberAccessHandlerEntity>(),
            new DefaultQueryJoinsGenerator<MemberAccessHandlerEntity>());

        var filter = new
        {
            Double = 10.5
        };
        
        // act
        queryBuilder.SetFilter(e => e.Double == filter.Double);
        var query = queryBuilder.Build();

        // assert
        Assert.NotNull(query);

        var parameters = new Dictionary<string, object>();
        parameters.Add("param_1", filter.Double);

        query.Parameters.Should().BeEquivalentTo(parameters);
    }
    
    [Fact(DisplayName = "Member Access Handler with long.")]
    public void Build_MemberAccessHandlerLong_Success()
    {
        // prepare
        var memberAccessHandlers= new List<IMemberAccessHandler>(){new PropertyInfoMemberAccessHandler()};
            
        var queryBuilder = new DefaultQueryBuilder<MemberAccessHandlerEntity>(
            new DefaultGroupByClausuleManager<MemberAccessHandlerEntity>(),
            new DefaultOrderByClausuleManager<MemberAccessHandlerEntity>(),
            new DefaultWhereClausuleManager<MemberAccessHandlerEntity>(memberAccessHandlers),
            new DefaultQuerySelectedColumnsProvider<MemberAccessHandlerEntity>(),
            new DefaultQueryJoinsGenerator<MemberAccessHandlerEntity>());

        var filter = new
        {
            Long = (long)1000
        };
        
        // act
        queryBuilder.SetFilter(e => e.Long == filter.Long);
        var query = queryBuilder.Build();

        // assert
        Assert.NotNull(query);

        var parameters = new Dictionary<string, object>();
        parameters.Add("param_1", filter.Long);

        query.Parameters.Should().BeEquivalentTo(parameters);
    }
    
    [Fact(DisplayName = "Member Access Handler with string.")]
    public void Build_MemberAccessHandlerString_Success()
    {
        // prepare
        var memberAccessHandlers= new List<IMemberAccessHandler>(){new PropertyInfoMemberAccessHandler()};
            
        var queryBuilder = new DefaultQueryBuilder<MemberAccessHandlerEntity>(
            new DefaultGroupByClausuleManager<MemberAccessHandlerEntity>(),
            new DefaultOrderByClausuleManager<MemberAccessHandlerEntity>(),
            new DefaultWhereClausuleManager<MemberAccessHandlerEntity>(memberAccessHandlers),
            new DefaultQuerySelectedColumnsProvider<MemberAccessHandlerEntity>(),
            new DefaultQueryJoinsGenerator<MemberAccessHandlerEntity>());

        var filter = new
        {
            String = "abcdefghijklmnopqrstuvxz"
        };
        
        // act
        queryBuilder.SetFilter(e => e.String == filter.String);
        var query = queryBuilder.Build();

        // assert
        Assert.NotNull(query);

        var parameters = new Dictionary<string, object>();
        parameters.Add("param_1", filter.String);

        query.Parameters.Should().BeEquivalentTo(parameters);
    }
    
    [Fact(DisplayName = "Member Access Handler with Guid.")]
    public void Build_MemberAccessHandlerGuid_Success()
    {
        // prepare
        var memberAccessHandlers= new List<IMemberAccessHandler>(){new PropertyInfoMemberAccessHandler()};
            
        var queryBuilder = new DefaultQueryBuilder<MemberAccessHandlerEntity>(
            new DefaultGroupByClausuleManager<MemberAccessHandlerEntity>(),
            new DefaultOrderByClausuleManager<MemberAccessHandlerEntity>(),
            new DefaultWhereClausuleManager<MemberAccessHandlerEntity>(memberAccessHandlers),
            new DefaultQuerySelectedColumnsProvider<MemberAccessHandlerEntity>(),
            new DefaultQueryJoinsGenerator<MemberAccessHandlerEntity>());

        var filter = new
        {
            Guid = Guid.NewGuid()
        };
        
        // act
        queryBuilder.SetFilter(e => e.Guid == filter.Guid);
        var query = queryBuilder.Build();

        // assert
        Assert.NotNull(query);

        var parameters = new Dictionary<string, object>();
        parameters.Add("param_1", filter.Guid);

        query.Parameters.Should().BeEquivalentTo(parameters);
    }
}