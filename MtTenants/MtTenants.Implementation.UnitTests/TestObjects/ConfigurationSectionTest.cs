using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace MtTenants.Implementation.UnitTests.TestObjects
{
    internal sealed class ConfigurationSectionTest : IConfigurationSection
    {
        public IConfigurationSection GetSection(string key)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<IConfigurationSection> GetChildren()
        {
            throw new System.NotImplementedException();
        }

        public IChangeToken GetReloadToken()
        {
            throw new System.NotImplementedException();
        }

        public string this[string key]
        {
            get => throw new System.NotImplementedException();
            set => throw new System.NotImplementedException();
        }

        public string Key { get; } = null!;
        public string Path { get; } = null!;
        public string Value { get; set; } = null!;
    }
}