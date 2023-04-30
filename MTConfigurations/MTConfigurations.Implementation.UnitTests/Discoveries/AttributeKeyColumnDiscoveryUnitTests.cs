using FluentAssertions;
using MTConfigurations.Abstractions;
using MTConfigurations.Abstractions.Discoveries;
using MTConfigurations.Abstractions.Enums;
using MTConfigurations.Implementation.UnitTests.Fakers;

namespace MTConfigurations.Implementation.UnitTests.Discoveries
{
    public sealed class AttributeKeyColumnDiscoveryUnitTests
    {
        [Fact(DisplayName = "Key property not configurated")]
        public void Discovery_KeyAttributeNotConfigurated_ThrowException()
        {
            // prepare
            var keyDiscovery =
                Activator.CreateInstance(
                        Constants.ASSEMBLY_NAME,
                        $"{Constants.BASIC_TYPE_DISCOVERIES_PATH}AttributeKeyColumnDiscovery")
                    ?.Unwrap() as IKeyColumnDiscovery;

            // act
            var exception = Assert.Throws<ApplicationException>(
                () => keyDiscovery?.Discovery(new FakeInvalidEntity()));

            // assert
            Assert.Equal("Key was not configurated.", exception.Message);
        }

        [Fact(DisplayName = "Key property configurated but column not")]
        public void Discovery_KeyAttributeWithoutColumn_ThrowException()
        {
            // prepare
            var keyDiscovery =
                Activator.CreateInstance(
                        Constants.ASSEMBLY_NAME,
                        $"{Constants.BASIC_TYPE_DISCOVERIES_PATH}AttributeKeyColumnDiscovery")
                    ?.Unwrap() as IKeyColumnDiscovery;

            var entity = new FakeKeyWithoutColumnEntity();
            
            // act
            var exception = Assert.Throws<ApplicationException>(
                () => keyDiscovery?.Discovery(entity));

            // assert
            Assert.Equal("Column configuration for key was not defined.", exception.Message);
        }

        [Fact(DisplayName = "Get valid key column")]
        public void Discovery_ValidKeyColumn_Success()
        {
            // prepare
            var keyDiscovery =
                Activator.CreateInstance(
                        Constants.ASSEMBLY_NAME,
                        $"{Constants.BASIC_TYPE_DISCOVERIES_PATH}AttributeKeyColumnDiscovery")
                    ?.Unwrap() as IKeyColumnDiscovery;
            
            var entity = new FakeBasicEntity();
            
            // act
            var column = keyDiscovery?.Discovery(entity);

            // assert
            Assert.NotNull(column);
            column.Should().BeEquivalentTo(new Column("Id", ValueGenerated.Automatic));
        }
    }
}