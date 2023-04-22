using System.Collections.Generic;
using Bogus;
using MtTenants.Abstractions;
using MtTenants.Implementation.UnitTests.TestObjects;

namespace MtTenants.Implementation.UnitTests.Fakers
{
    internal sealed class TenantFaker
    {
        public IEnumerable<ITenant> GetTenants(int quantity = 1)
        {
            var faker = new Faker<ITenant>();

            var tenants = faker.CustomInstantiator(
                fk => new TenantTest(fk.Random.Guid(), fk.Name.Prefix()))
                .Generate(quantity);

            return tenants;
        }
    }
}