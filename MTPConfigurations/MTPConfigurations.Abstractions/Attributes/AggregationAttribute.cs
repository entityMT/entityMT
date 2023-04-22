using MTPConfigurations.Abstractions.Enums;

namespace MTPConfigurations.Abstractions.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public sealed class AggregationAttribute : Attribute
    {
        public AggregationAttribute(AggregationType aggregationType)
        {
            AggregationType = aggregationType;
        }
        
        public AggregationType AggregationType { get; }
    }
}