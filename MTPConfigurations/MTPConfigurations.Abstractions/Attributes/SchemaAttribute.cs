using System;

namespace MTPConfigurations.Abstractions.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class SchemaAttribute : Attribute
    {
        public SchemaAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public override string ToString()
        {
            return Name;
        }
    }
}