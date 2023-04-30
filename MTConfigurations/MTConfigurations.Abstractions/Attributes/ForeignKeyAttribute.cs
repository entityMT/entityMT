using System;

namespace MTConfigurations.Abstractions.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public sealed class ForeignKeyAttribute : Attribute
    {
        public ForeignKeyAttribute(Type destinationType)
        {
            DestinationType = destinationType;
        }
        
        public Type DestinationType { get; }
    }
}