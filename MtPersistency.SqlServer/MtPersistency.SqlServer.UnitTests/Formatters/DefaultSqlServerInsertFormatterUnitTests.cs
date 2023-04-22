using System;
using System.Linq;
using System.Reflection;
using Bogus;
using Moq;
using MTConfigurations.Abstractions;
using MTConfigurations.Abstractions.Enums;
using MTConfigurations.Abstractions.Providers;
using MtPersistency.CommandFormatters;
using MtPersistency.Providers;

namespace MtPersistency.SqlServer.UnitTests.Formatters
{
    public sealed class DefaultSqlServerInsertFormatterUnitTests
    {
        [Fact(DisplayName = "Format insert command")]
        public void FormatCommand_Insert_Success()
        {
            // prepare
            var faker = new Faker();
            string schema = faker.Random.Word();
            string tableName = faker.Random.Word();
            string[] columnsNames = faker.Random.WordsArray(2, 10);
            var schemaProviderMock = new Mock<ISchemaProvider>();
            var tableNameProviderMock = new Mock<ITableNameProvider>();
            var columnsProviderMock = new Mock<IColumnsProvider>();
            var parameterPrefixProviderMock = new Mock<IParameterPrefixProvider>();

            schemaProviderMock
                .Setup(sp => sp.GetSchema(It.IsAny<object>()))
                .Returns(schema);

            tableNameProviderMock
                .Setup(tnp => tnp.GetTableName(It.IsAny<object>()))
                .Returns(tableName);

            columnsProviderMock
                .Setup(cp => cp.GetColumns(It.IsAny<object>()))
                .Returns(columnsNames.Select(c => new Column(c, ValueGenerated.Manual)));

            parameterPrefixProviderMock
                .Setup(ppp => ppp.GetPrefix())
                .Returns("@");

            // act
            var insertFormatter =
                Activator.CreateInstance(
                    Constants.ASSEMBLY_NAME,
                    $"{Constants.BASIC_FORMATTERS_TYPE_PATH}DefaultSqlServerInsertFormatter",
                    true,
                    BindingFlags.CreateInstance,
                    null,
                    new object[]
                    {
                        schemaProviderMock.Object,
                        tableNameProviderMock.Object,
                        columnsProviderMock.Object,
                        parameterPrefixProviderMock.Object
                    },
                    null,
                    null)?.Unwrap() as IInsertFormatter;

            var insertCommand = insertFormatter?.FormatCommand(new());

            // assert
            var expectedInsert = 
                $"INSERT INTO {schema}.{tableName} VALUES(";

            foreach (var columnName in columnsNames)
                expectedInsert += $"@{columnName},";

            expectedInsert = expectedInsert.Remove(expectedInsert.Length - 1, 1);
            expectedInsert += ")";
            
            Assert.NotNull(insertCommand);
            Assert.Equal(expectedInsert, insertCommand);
        }
    }
}