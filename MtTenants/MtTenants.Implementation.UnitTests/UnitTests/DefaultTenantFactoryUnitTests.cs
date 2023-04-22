using System;
using System.Reflection;
using Bogus;
using MtTenants.Abstractions;

namespace MtTenants.Implementation.UnitTests.UnitTests
{
    public sealed class DefaultTenantFactoryUnitTests
    {
        [Fact(DisplayName = "Valida a criação da Tenant pelo Factory")]
        public void Create_PropriedadesTenant_Sucesso()
        {
            //prepare
            var faker = new Faker();
            var tenantId = faker.Random.Guid();
            var tenantName = faker.Name.Prefix();

            //act
            var tenantFactory = Activator.CreateInstance(
                Constants.IMPLEMENTATION_ASSEMBLY_NAME,
                $"{Constants.IMPLEMENTATION_ASSEMBLY_NAME}.DefaultTenantFactory",
                false, BindingFlags.CreateInstance,
                null,
                null,
                null,
                null)?.Unwrap() as ITenantFactory;

            var tenant = tenantFactory?.Create(tenantId, tenantName);

            //assert
            Assert.NotNull(tenant);
            Assert.Equal(tenantId, tenant?.Id);
            Assert.Equal(tenantName, tenant?.Name);
        }
    }
}