using System;
using System.Linq;
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
    public sealed class DefaultSqlServerUpdateFormatterUnitTests
    {
        [Fact(DisplayName = "Format update command")]
        public void FormatCommand_Update_Success()
        {
            // prepare
            var faker = new Faker();
            var schema = faker.Random.Word();
            var tableName = faker.Random.Word();
            var columnsNames = faker.Random.WordsArray(2, 10);
            var schemaProviderMock = new Mock<ISchemaProvider>();
            var tableNameProviderMock = new Mock<ITableNameProvider>();
            var columnsProviderMock = new Mock<IColumnsProvider>();
            var parameterPrefixProviderMock = new Mock<IParameterPrefixProvider>();
            var keyColumnDiscoveryMock = new Mock<IKeyColumnDiscovery>();

            schemaProviderMock
                .Setup(sp => sp.GetSchema(It.IsAny<object>()))
                .Returns(schema);

            tableNameProviderMock
                .Setup(tnp => tnp.GetTableName(It.IsAny<object>()))
                .Returns(tableName);

            parameterPrefixProviderMock
                .Setup(ppp => ppp.GetPrefix())
                .Returns("@");

            keyColumnDiscoveryMock
                .Setup(kcd => kcd.Discovery(It.IsAny<object>()))
                .Returns(new Column("Id", ValueGenerated.Automatic));

            columnsProviderMock
                .Setup(cp => cp.GetColumns(It.IsAny<object>()))
                .Returns(columnsNames.Select(c => new Column(c, ValueGenerated.Manual)));

            // act
            var updateFormatter =
                Activator.CreateInstance(
                    Constants.ASSEMBLY_NAME,
                    $"{Constants.BASIC_FORMATTERS_TYPE_PATH}DefaultSqlServerUpdateFormatter",
                    true,
                    BindingFlags.CreateInstance,
                    null,
                    new object[]
                    {
                        schemaProviderMock.Object,
                        tableNameProviderMock.Object,
                        columnsProviderMock.Object,
                        parameterPrefixProviderMock.Object,
                        keyColumnDiscoveryMock.Object
                    },
                    null,
                    null)?.Unwrap() as IUpdateFormatter;

            var updateCommand = updateFormatter?.FormatCommand(new());

            // assert
            string expectedCommand =
                $"UPDATE {schema}.{tableName} SET ";

            foreach (var columnName in columnsNames)
                expectedCommand += $"{columnName} = @{columnName},";

            expectedCommand = expectedCommand.Remove(expectedCommand.Length - 1, 1);
            expectedCommand += " WHERE Id = @Id";
            
            Assert.NotNull(updateCommand);
            Assert.Equal(expectedCommand, updateCommand);
        }
    }
}