using FluentAssertions;
using MTConfigurations.Abstractions;
using MTConfigurations.Abstractions.Enums;
using MTConfigurations.Abstractions.Providers;
using MTConfigurations.Implementation.UnitTests.Fakers;

namespace MTConfigurations.Implementation.UnitTests.Providers
{
    public sealed class AttributeColumnsProviderUnitTests
    {
        [Fact(DisplayName = "Entity without any columns configurated")]
        public void GetColumns_EntityWithoutAnyColumns_ThrowApplicationException()
        {
            // prepare
            var columnProvider = Activator.CreateInstance(
                    Constants.ASSEMBLY_NAME,
                    $"{Constants.BASIC_TYPE_PROVIDERS_PATH}AttributeColumnsProvider")
                ?.Unwrap() as IColumnsProvider;

            // act
            var exception = Assert.Throws<ApplicationException>(
                () => columnProvider?.GetColumns(new FakeInvalidEntity()));
            
            // assert
            Assert.Equal("Table doesn't have any columns configurated", exception.Message);
        }

        [Fact(DisplayName = "Entity with valid 'Columns' attributes")]
        public void GetColumns_ValidConfiguration_TableColumns()
        {
            // prepare
            var columnProvider = Activator.CreateInstance(
                    Constants.ASSEMBLY_NAME,
                    $"{Constants.BASIC_TYPE_PROVIDERS_PATH}AttributeColumnsProvider")
                ?.Unwrap() as IColumnsProvider;
            
            // act
            var fakeEntity = new FakeBasicEntity();
            var columns = columnProvider?.GetColumns(fakeEntity);

            // assert
            columns.Should().BeEquivalentTo(new List<Column>()
            {
                new Column("Id", ValueGenerated.Automatic),
                new Column("FieldInteger", ValueGenerated.Manual),
                new Column("FieldString", ValueGenerated.Manual),
                new Column("FieldDate", ValueGenerated.Manual),
                new Column("FieldBoolean", ValueGenerated.Manual)
            });
        }
    }
}