using System;
using MTConfigurations.Abstractions.Enums;

namespace MTConfigurations.Abstractions.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public sealed class ColumnAttribute : Attribute
    {
        public ColumnAttribute(string name, ValueGenerated valueGenerated)
        {
            Name = name;
            ValueGenerated = valueGenerated;
        }
        
        public string Name { get; }
        public ValueGenerated ValueGenerated { get; }
    }
}