using MTPConfigurations.Abstractions.Enums;

namespace MTPConfigurations.Abstractions
{
    public sealed class Column
    {
        public Column(string name, ValueGenerated valueGenerated)
        {
            Name = name;
            ValueGenerated = valueGenerated;
        }
        
        public string Name { get; }
        public ValueGenerated ValueGenerated { get; }
    }
}