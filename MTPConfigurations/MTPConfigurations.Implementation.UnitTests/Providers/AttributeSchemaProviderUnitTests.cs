using System;
using MTPConfigurations.Abstractions.Providers;
using MTPConfigurations.Implementation.UnitTests.Fakers;

namespace MTPConfigurations.Implementation.UnitTests.Providers
{
    public sealed class AttributeSchemaProviderUnitTests
    {
        [Fact(DisplayName = "Entity without schema attribute")]
        public void GetSchema_WithoutAttribute_ThrowException()
        {
            // prepare
            var schemaProvider = Activator.CreateInstance(
                    Constants.ASSEMBLY_NAME,
                    $"{Constants.BASIC_TYPE_PROVIDERS_PATH}AttributeSchemaProvider")
                ?.Unwrap() as ISchemaProvider;

            // act
            var exception = Assert.Throws<ApplicationException>(
                () => schemaProvider?.GetSchema(new FakeInvalidEntity()));

            // assert
            Assert.Equal("Schema attribute was not configurated.", exception.Message);
        }

        [Fact(DisplayName = "Get schema from valid entity")]
        public void GetSchema_ValidEntity_Success()
        {
            // prepare
            var schemaProvider = Activator.CreateInstance(
                    Constants.ASSEMBLY_NAME,
                    $"{Constants.BASIC_TYPE_PROVIDERS_PATH}AttributeSchemaProvider")
                ?.Unwrap() as ISchemaProvider;
            
            // act
            var schema = schemaProvider?.GetSchema(new FakeBasicEntity());

            // assert
            Assert.NotNull(schema);
            Assert.Equal("dbo", schema);
        }
    }
}