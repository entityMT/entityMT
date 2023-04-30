using MTConfigurations.Abstractions.Enums;

namespace MTConfigurations.Abstractions
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