using System;
using System.Linq;
using System.Reflection;
using Bogus;
using Moq;
using MtTenants.Implementation.UnitTests.Fakers;
using Microsoft.Extensions.Configuration;
using MtTenants.Abstractions;
using MtTenants.Implementation.UnitTests.TestObjects;

namespace MtTenants.Implementation.UnitTests.UnitTests
{
    public sealed class DefaultConnectionStringProviderUnitTests
    {
        [Fact(DisplayName = "Valida a obtenção da string de conexão através da tenant")]
        public void GetConnectionString_ObtencaoDaStringDeConexao_Sucesso()
        {
            //prepare
            var faker = new TenantFaker();
            var tenant = faker.GetTenants().First();
            var configMock = new Mock<IConfiguration>();
            var connectionString = new Faker().Random.String(minLength: 100, maxLength: 150);

            configMock.Setup(config =>
                    config.GetSection($"{tenant.Name}_CONNECTIONSTRING"))
                .Returns(new ConfigurationSectionTest() {Value = connectionString});

            //act
            var connectionStringProvider = Activator
                .CreateInstance(
                    Constants.IMPLEMENTATION_ASSEMBLY_NAME,
                    $"{Constants.IMPLEMENTATION_ASSEMBLY_NAME}.DefaultConnectionStringProvider",
                    false,
                    BindingFlags.CreateInstance,
                    null,
                    new[] {configMock.Object},
                    null,
                    null)
                ?.Unwrap() as IConnectionStringProvider;

            var connectionStringResult = connectionStringProvider?.GetConnectionString(tenant);

            //assert
            Assert.NotNull(connectionStringResult);
            Assert.Equal(connectionString, connectionStringResult);
        }
    }
}