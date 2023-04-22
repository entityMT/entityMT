using System;
using MTPConfigurations.Abstractions.Providers;
using MTPConfigurations.Implementation.UnitTests.Fakers;

namespace MTPConfigurations.Implementation.UnitTests.Providers
{
    public sealed class AttributeTableNameProviderUnitTests
    {
        [Fact(DisplayName = "Entity without table attribute")]
        public void GetTableName_InvalidEntity_ThrowException()
        {
            // prepare
            var tableNameProvider = Activator.CreateInstance(
                    Constants.ASSEMBLY_NAME,
                    $"{Constants.BASIC_TYPE_PROVIDERS_PATH}AttributeTableNameProvider")
                ?.Unwrap() as ITableNameProvider;

            // act
            var exception = Assert.Throws<ApplicationException>(
                () => tableNameProvider?.GetTableName(new FakeInvalidEntity()));

            // assert
            Assert.Equal("Table name attribute was not configurated", exception.Message);
        }

        [Fact(DisplayName = "Get table name from valid entity")]
        public void GetTableName_ValidConfiguration_Success()
        {
            // prepare
            var tableNameProvider = Activator.CreateInstance(
                    Constants.ASSEMBLY_NAME,
                    $"{Constants.BASIC_TYPE_PROVIDERS_PATH}AttributeTableNameProvider")
                ?.Unwrap() as ITableNameProvider;
            
            // act
            var tableName = tableNameProvider?.GetTableName(new FakeBasicEntity());

            // assert
            Assert.NotNull(tableName);
            Assert.Equal("FakeTable", tableName);
        }
    }
}