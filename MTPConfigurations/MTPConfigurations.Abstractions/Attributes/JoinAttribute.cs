using System;
using MTPConfigurations.Abstractions.Enums;

namespace MTPConfigurations.Abstractions.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public sealed class JoinAttribute : Attribute
    {
        public JoinAttribute(JoinType joinType)
        {
            JoinType = joinType;
        }
        
        public JoinType JoinType { get; }
    }
}