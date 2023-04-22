using System;
using System.Reflection;
using Bogus;
using Moq;
using MTConfigurations.Abstractions;
using MTConfigurations.Abstractions.Discoveries;
using MTConfigurations.Abstractions.Enums;
using MTConfigurations.Abstractions.Providers;
using MtPersistency.CommandFormatters;
using MtPersistency.Providers;

namespace MtPersistency.SqlServer.UnitTests.Formatters
{
    public sealed class DefaultSqlServerDeleteFormatterUnitTests
    {
        [Fact(DisplayName = "Format delete command")]
        public void FormatCommand_Delete_Success()
        {
            // prepare
            var faker = new Faker();
            string schema = faker.Random.Word();
            string tableName = faker.Random.Word();
            var schemaProviderMock = new Mock<ISchemaProvider>();
            var tableNameProviderMock = new Mock<ITableNameProvider>();
            var parameterPrefixProviderMock = new Mock<IParameterPrefixProvider>();
            var keyColumnDiscoveryMock = new Mock<IKeyColumnDiscovery>();
            var keyColumn = new Column("Id", ValueGenerated.Automatic);

            schemaProviderMock.Setup(sp => sp.GetSchema(It.IsAny<object>())).Returns(schema);
            tableNameProviderMock.Setup(tnp => tnp.GetTableName(It.IsAny<object>())).Returns(tableName);
            parameterPrefixProviderMock.Setup(ppp => ppp.GetPrefix()).Returns("@");
            keyColumnDiscoveryMock.Setup(kcd => kcd.Discovery(It.IsAny<object>())).Returns(keyColumn);

            // act
            var deleteFormatter =
                Activator.CreateInstance(
                    Constants.ASSEMBLY_NAME,
                    $"{Constants.BASIC_FORMATTERS_TYPE_PATH}DefaultSqlServerDeleteFormatter",
                    true,
                    BindingFlags.CreateInstance,
                    null,
                    new object[]
                    {
                        schemaProviderMock.Object,
                        tableNameProviderMock.Object,
                        parameterPrefixProviderMock.Object,
                        keyColumnDiscoveryMock.Object
                    },
                    null,
                    null)?.Unwrap() as IDeleteFormatter;

            var deleteCommand = deleteFormatter?.FormatCommand(new());

            // assert
            var deleteExpectedCommand = 
                $"DELETE FROM {schema}.{tableName} WHERE {keyColumn.Name} = @{keyColumn.Name}";
            
            Assert.NotNull(deleteCommand);
            Assert.Equal(deleteExpectedCommand, deleteCommand);
        }
    }
}