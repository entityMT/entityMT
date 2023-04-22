using System.Reflection;
using FluentAssertions;
using MTPConfigurations.Abstractions.Attributes;
using MTQueries.SqlServer.UnitTests.Fakers;

namespace MTQueries.SqlServer.UnitTests;

public sealed class DefaultQuerySelectedColumnsProviderUnitTests
{
    [Fact(DisplayName = "Simple Entity (without nested objects) columns select")]
    public async Task GetColumnsAsync_SimpleEntity_Success()
    {
        // prepare
        var provider = new DefaultQuerySelectedColumnsProvider<SimpleEntity>();
        
        // act
        var columns = await provider.GetColumnsAsync();

        // assert
        columns.Should().BeEquivalentTo(
            typeof(SimpleEntity)
                .Properties()
                .Select(
                    p => $"{nameof(SimpleEntity)}.{p.GetCustomAttribute<ColumnAttribute>()!.Name}"));
    }

    [Fact(DisplayName = "Nested Type Entity columns select")]
    public async Task GetColumnsAsync_NestedEntity_Success()
    {
        // prepare
        var provider = new DefaultQuerySelectedColumnsProvider<NestedTypeEntity>();

        // act
        var columns = await provider.GetColumnsAsync();
        
        // assert
        string[] assert = typeof(NestedTypeEntity)
            .Properties()
            .Where(
                p => p.GetCustomAttribute<ColumnAttribute>() != default)
            .Select(
                p => $"{nameof(NestedTypeEntity)}.{p.GetCustomAttribute<ColumnAttribute>()!.Name}")
            .Union(
                typeof(SimpleEntity)
                    .Properties()
                    .Select(
                        p => $"{nameof(SimpleEntity)}.{p.GetCustomAttribute<ColumnAttribute>()!.Name}")).ToArray();
        
        columns.Should().BeEquivalentTo(assert);
    }

    [Fact(DisplayName = "Collection Type Entity columns select")]
    public async Task GetColumnsAsync_CollectionEntity_Success()
    {
        // prepare
        var provider = new DefaultQuerySelectedColumnsProvider<CollectionTypeEntity>();

        // act
        var columns = await provider.GetColumnsAsync();

        // assert
        var assert =
            typeof(CollectionTypeEntity)
                .Properties()
                .Where(
                    p => p.GetCustomAttribute<ColumnAttribute>() != default)
                .Select(
                    p => $"{nameof(CollectionTypeEntity)}.{p.GetCustomAttribute<ColumnAttribute>()!.Name}")
                .Union(
                    typeof(SimpleEntity)
                        .Properties()
                        .Select(
                            p => $"{nameof(SimpleEntity)}.{p.GetCustomAttribute<ColumnAttribute>()!.Name}"))
                .ToArray();
        
        columns.Should().BeEquivalentTo(assert);
    }
}