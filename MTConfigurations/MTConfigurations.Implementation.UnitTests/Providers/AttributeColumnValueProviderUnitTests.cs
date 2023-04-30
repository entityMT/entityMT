using MTConfigurations.Abstractions;
using MTConfigurations.Abstractions.Enums;
using MTConfigurations.Abstractions.Providers;
using MTConfigurations.Implementation.UnitTests.Fakers;

namespace MTConfigurations.Implementation.UnitTests.Providers
{
    public sealed class AttributeColumnValueProviderUnitTests
    {
        [Fact(DisplayName = "Get value from invalid column (not found in entity configuration)")]
        public void GetValue_InvalidColumn_ThrowException()
        {
            // prepare
            var columnValueProvider = Activator.CreateInstance(
                Constants.ASSEMBLY_NAME,
                $"{Constants.BASIC_TYPE_PROVIDERS_PATH}AttributeColumnValueProvider")
                ?.Unwrap() as IColumnValueProvider;

            var column = new Column("Invalid", ValueGenerated.Automatic);

            // act
            var exception = Assert.Throws<ApplicationException>(
                () => columnValueProvider?.GetValue(new FakeBasicEntity(), column));
            
            // assert
            Assert.Equal($"Column '{column.Name}' not bound to any property.", exception.Message);
        }

        [Fact(DisplayName = "Get value from valid column (found in entity configuration)")]
        public void GetValue_ValidColumn_Success()
        {
            // prepare
            var columnValueProvider = Activator.CreateInstance(
                    Constants.ASSEMBLY_NAME,
                    $"{Constants.BASIC_TYPE_PROVIDERS_PATH}AttributeColumnValueProvider")
                ?.Unwrap() as IColumnValueProvider;
            
            var column = new Column("FieldBoolean", ValueGenerated.Manual);

            var entity = new FakeBasicEntity()
            {
                FieldBoolean = true
            };

            // act
            var columnValue = columnValueProvider?.GetValue(entity, column);

            // assert
            Assert.Equal(entity.FieldBoolean, Convert.ToBoolean(columnValue));
        }
    }
}